using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Effect
{
    public class BloomEffect : IPostProcessEffect
    {
        private Microsoft.Xna.Framework.Graphics.Effect _ExtractEffect;
        private Microsoft.Xna.Framework.Graphics.Effect _BlurEffect;
        private Microsoft.Xna.Framework.Graphics.Effect _CombineEffect;

        private RenderTarget2D _bloomTarget1;
        private RenderTarget2D _bloomTarget2;
        private RenderTarget2D _finalTarget;

        public BloomSettings Settings
        {
            get;
            set;
        }

        public GraphicsDevice GraphicsDevice
        {
            get;
            set;
        }

        public BloomEffect(GraphicsDevice graphics)
        {
            this.Settings = BloomSettings.PresetSettings[0];
            this.GraphicsDevice = graphics;

            LoadEffects();
        }

        public BloomEffect(BloomSettings settings, GraphicsDevice graphics)
        {
            this.Settings = settings;
            this.GraphicsDevice = graphics;

            _bloomTarget1 = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width / 4,
                GraphicsDevice.Viewport.Height / 4);

            _bloomTarget2 = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width / 4,
                GraphicsDevice.Viewport.Height / 4);

            _finalTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            LoadEffects();
        }

        private void LoadEffects()
        {
            _ExtractEffect = (Microsoft.Xna.Framework.Graphics.Effect)ContentManager.Instance.Load(
                @"Data\shaders\Bloom.mgfxo", "Bloom", ContentType.Effect);

            _BlurEffect = (Microsoft.Xna.Framework.Graphics.Effect)ContentManager.Instance.Load(
                @"Data\shaders\GaussianBlur.mgfxo", "Blur", ContentType.Effect);

            _CombineEffect = (Microsoft.Xna.Framework.Graphics.Effect)ContentManager.Instance.Load(
                @"Data\shaders\Combine.mgfxo", "Combine", ContentType.Effect);
        }

        public void Update(GameTime gameTime)
        {

        }

        public RenderTarget2D RenderToTexture(Texture2D input)
        {
            Texture2D colorMap = input;

            // Pass 1: Extract the bright parts of the image.
            _ExtractEffect.Parameters["Threshold"].SetValue(Settings.BloomThreshold);
            PostProcessor.DrawFullscreenQuad(GraphicsDevice, input, _bloomTarget1, _ExtractEffect);

            // Pass 2: draw from bloomTarget1 into bloomTarget2,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(_BlurEffect, 1.0f / (float)_bloomTarget1.Width, 0, 4);
            PostProcessor.DrawFullscreenQuad(GraphicsDevice, _bloomTarget1, _bloomTarget2, _BlurEffect);

            // Pass 3: draw from bloomTarget2 back into bloomTarget1,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(_BlurEffect, 0, 1.0f / (float)_bloomTarget1.Height, 4);
            PostProcessor.DrawFullscreenQuad(GraphicsDevice, _bloomTarget2, _bloomTarget1, _BlurEffect);

            GraphicsDevice.SetRenderTarget(_finalTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            // Final Pass: Combine the colormap and the bloom texture.
            _CombineEffect.Parameters["ColorMap"].SetValue(colorMap);
            _CombineEffect.Parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            _CombineEffect.Parameters["OriginalIntensity"].SetValue(Settings.BaseIntensity);
            _CombineEffect.Parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            _CombineEffect.Parameters["OriginalSaturation"].SetValue(Settings.BaseSaturation);

            PostProcessor.DrawFullscreenQuad(
                GraphicsDevice,
                _bloomTarget1,
                _finalTarget.Width,
                _finalTarget.Height,
                _CombineEffect);

            return _finalTarget;
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(Microsoft.Xna.Framework.Graphics.Effect gaussianBlurEffect, float dx, float dy, float blurAmount)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0, blurAmount);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1, blurAmount);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeGaussian(float n, float blurAmount)
        {
            float theta = blurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
