using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion.Core.Effect
{
    public class GenericEffect : IPostProcessEffect
    {
        private RenderTarget2D _renderTarget;

        public Microsoft.Xna.Framework.Graphics.Effect Effect
        {
            get;
            set;
        }

        public GraphicsDevice GraphicsDevice
        {
            get;
            set;
        }

        public GenericEffect(Microsoft.Xna.Framework.Graphics.Effect effect, GraphicsDevice graphics)
        {
            this.Effect = effect;
            this.GraphicsDevice = graphics;

            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual RenderTarget2D RenderToTexture(Texture2D input)
        {
            PostProcessor.DrawFullscreenQuad(GraphicsDevice, input, _renderTarget, Effect);

            return _renderTarget;
        }
    }
}
