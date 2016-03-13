using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Orion.Core.Effect
{
    public class PostProcessor
    {
        private SortedDictionary<int, IPostProcessEffect> _ActiveEffects = new SortedDictionary<int, IPostProcessEffect>();
        private Queue<int> _EffectsToRemove = new Queue<int>();

        public GraphicsDevice GraphicsDevice { get; internal set; }

        public PostProcessor(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;
        }

        public void AddEffect(IPostProcessEffect effect, PostProcessEffectType type)
        {
            _ActiveEffects.Add((int)type, effect);
        }

        public void AddEffect(IPostProcessEffect effect, int priority)
        {
            _ActiveEffects.Add(priority, effect);
        }

        public IPostProcessEffect GetEffect(PostProcessEffectType type)
        {
            return GetEffect((int)type);
        }

        public IPostProcessEffect GetEffect(int key)
        {
            IPostProcessEffect effect = null;
            _ActiveEffects.TryGetValue(key, out effect);
            return effect;
        }

        public void RemoveEffect(IPostProcessEffect effect)
        {
            int key = -1;

            foreach (KeyValuePair<int, IPostProcessEffect> pair in _ActiveEffects)
            {
                if (ReferenceEquals(pair.Value, effect))
                {
                    key = pair.Key;
                    break;
                }
            }

            if (key != -1)
                _EffectsToRemove.Enqueue(key);
        }

        public void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<int, IPostProcessEffect> pair in _ActiveEffects)
                pair.Value.Update(gameTime);

            while (_EffectsToRemove.Count > 0)
            {
                int key = _EffectsToRemove.Dequeue();
                _ActiveEffects.Remove(key);
            }
        }

        public void Apply(RenderTarget2D sceneTexture, Microsoft.Xna.Framework.Graphics.Effect finalEffect)
        {
            RenderTarget2D output = sceneTexture;

            foreach (KeyValuePair<int, IPostProcessEffect> pair in _ActiveEffects)
                output = pair.Value.RenderToTexture(output);

            GraphicsDevice.SetRenderTarget(null);

            DrawFullscreenQuad(
                GraphicsDevice,
                output,
                output.Width,
                output.Height,
                finalEffect);
        }

        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        internal static void DrawFullscreenQuad(GraphicsDevice graphics, Texture2D texture, RenderTarget2D renderTarget,
                                                Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            graphics.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(
                graphics,
                texture,
                renderTarget.Width,
                renderTarget.Height,
                effect);
        }


        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        internal static void DrawFullscreenQuad(GraphicsDevice graphics, Texture2D texture, int width, int height,
                                                Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            using (SpriteBatch spriteBatch = new SpriteBatch(graphics))
            {
                spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
                spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
                spriteBatch.End();
            }
        }
    }
}
