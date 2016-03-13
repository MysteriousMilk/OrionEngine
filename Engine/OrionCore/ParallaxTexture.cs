using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class ParallaxSurface
    {
        public Texture2D Texture { get; set; }
        public Vector2 Scroll { get; set; }
        public float ScrollFactor { get; set; }

        public ParallaxSurface(Texture2D texture)
        {
            this.Texture = texture;
            this.Scroll = Vector2.Zero;
            this.ScrollFactor = 5.0f;
        }

        public ParallaxSurface(string textureRef)
        {
            this.Texture = ContentManager.Instance.Get(textureRef, ContentType.Texture) as Texture2D;
            this.Scroll = Vector2.Zero;
            this.ScrollFactor = 5.0f;
        }

        public void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float x = this.Scroll.X + this.ScrollFactor * delta;
            float y = this.Scroll.Y + this.ScrollFactor * delta;

            if (this.Scroll.X < 0)
            {
                x += this.Texture.Width;
            }
            else if (this.Scroll.X >= this.Texture.Width)
            {
                x -= this.Texture.Width;
            }
            if (this.Scroll.Y < 0)
            {
                y += this.Texture.Height;
            }
            else if (this.Scroll.Y >= this.Texture.Height)
            {
                y -= this.Texture.Height;
            }

            this.Scroll = new Vector2(x, y);
        }
    }
}
