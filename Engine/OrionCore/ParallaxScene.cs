using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Orion.Core.Managers;

namespace Orion.Core
{
    public class ParallaxScene : Scene
    {
        private SpriteBatch _bkgSpriteBatch;
        private SpriteBatch _overlaySpriteBatch;

        #region Properties
        public Texture2D Background
        {
            get;
            set;
        }

        public List<ParallaxSurface> ParallaxLayers
        {
            get;
            set;
        }

        public BlendState BackgroundBlendState
        {
            get;
            set;
        }
        #endregion

        public ParallaxScene(GraphicsDevice graphics, ICamera2D camera)
            : base(graphics, camera)
        {
            _bkgSpriteBatch = new SpriteBatch(graphics);
            _overlaySpriteBatch = new SpriteBatch(graphics);
            BackgroundBlendState = BlendState.AlphaBlend;

            ParallaxLayers = new List<ParallaxSurface>();
        }

        public void SetBackground(string backgroundRef)
        {
            Background = ContentManager.Instance.Get(backgroundRef, ContentType.Texture) as Texture2D;
        }

        #region SceneBase Overrides
        public override void Update(GameTime gameTime)
        {
            foreach (ParallaxSurface parallax in ParallaxLayers)
                parallax.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            _bkgSpriteBatch.Begin(SpriteSortMode.Deferred, BackgroundBlendState, SamplerState.LinearWrap, null, null);

            if(Background != null)
                _bkgSpriteBatch.Draw(Background, Vector2.Zero, Color.White);

            foreach(ParallaxSurface parallax in ParallaxLayers)
            {
                _bkgSpriteBatch.Draw(parallax.Texture, Vector2.Zero,
                    new Rectangle(
                        (int)(this.Camera.Position.X * parallax.ScrollFactor),
                        (int)(this.Camera.Position.Y * parallax.ScrollFactor),
                        Settings.Instance.ResolutionX,
                        Settings.Instance.ResolutionY
                    ),
                    Color.White);
            }

            _bkgSpriteBatch.End();

            base.Draw();
        }
        #endregion
    }
}
