using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Effect
{
    public class SceneFadeEffect : IPostProcessEffect
    {
        private RenderTarget2D _renderTarget;
        private RenderTarget2D _snapshotTarget;
        private float _elapsed = 0.0f;
        private float _fadeTime = 0.0f;
        
        public float FadeDuration
        {
            get;
            set;
        }

        public Microsoft.Xna.Framework.Graphics.Effect Effect
        {
            get;
            set;
        }

        public IScene PreviousScene
        {
            get;
            set;
        }

        public GraphicsDevice GraphicsDevice
        {
            get;
            set;
        }

        public event EventHandler Elapsed;

        public SceneFadeEffect(Microsoft.Xna.Framework.Graphics.Effect effect, IScene previousScene, float duration, GraphicsDevice graphics)
        {
            Effect = effect;
            PreviousScene = previousScene;
            FadeDuration = duration;
            GraphicsDevice = graphics;

            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            _snapshotTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
        }

        public void Reset()
        {
            _elapsed = 0.0f;
        }

        public virtual void Update(GameTime gameTime)
        {
            PreviousScene.Update(gameTime);

            _fadeTime = OrionMath.LinearInterpolate(0.0f, 1.0f, _elapsed);

            if (_elapsed <= 1.0f)
            {
                _elapsed += gameTime.ElapsedGameTime.Milliseconds / FadeDuration;
            }
            else
            {
                if (Elapsed != null)
                    Elapsed(this, new EventArgs());
            }
        }

        public virtual RenderTarget2D RenderToTexture(Texture2D input)
        {
            Texture2D snapshot = GetSceneSnapshot();

            Effect.Parameters["fFadeAmount"].SetValue(_fadeTime);
            Effect.Parameters["ColorMap2"].SetValue(snapshot);

            PostProcessor.DrawFullscreenQuad(GraphicsDevice, input, _renderTarget, Effect);

            return _renderTarget;
        }

        private RenderTarget2D GetSceneSnapshot()
        {
            GraphicsDevice.SetRenderTarget(_snapshotTarget);
            PreviousScene.Draw();
            GraphicsDevice.SetRenderTarget(null);

            return _snapshotTarget;
        }
    }
}
