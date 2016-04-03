using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Managers;

namespace Orion.Core
{
    public class ParallaxSurface
    {
        public Texture2D Texture { get; set; }
        public Vector2 Scroll { get; set; }
        public float ScrollFactor { get; set; }

        public ParallaxSurface(Texture2D texture)
        {
            Texture = texture;
            Scroll = Vector2.Zero;
            ScrollFactor = 5.0f;
        }

        public ParallaxSurface(string textureRef)
        {
            Texture = ContentManager.Instance.Get(textureRef, ContentType.Texture) as Texture2D;
            Scroll = Vector2.Zero;
            ScrollFactor = 5.0f;
        }

        public void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float x = Scroll.X + ScrollFactor * delta;
            float y = Scroll.Y + ScrollFactor * delta;

            if (Scroll.X < 0)
            {
                x += Texture.Width;
            }
            else if (Scroll.X >= Texture.Width)
            {
                x -= Texture.Width;
            }
            if (Scroll.Y < 0)
            {
                y += Texture.Height;
            }
            else if (Scroll.Y >= Texture.Height)
            {
                y -= Texture.Height;
            }

            Scroll = new Vector2(x, y);
        }
    }
}
