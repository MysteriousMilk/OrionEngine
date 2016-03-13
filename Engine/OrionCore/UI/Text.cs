using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.UI
{
    public class Text
    {
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; }
        public Vector2 Position { get; set; }
        public string DisplayText { get; set; }

        public Text(string text, Vector2 position, SpriteFont font)
        {
            this.DisplayText = text;
            this.Position = position;
            this.Font = font;
            this.Alpha = 1.0f;
        }

        public Text(string text, Vector2 position, SpriteFont font, Color color)
            : this(text, position, font)
        {
            this.Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                this.Font,
                this.DisplayText,
                this.Position + new Vector2(2, 2),
                new Color(Color.Black, this.Alpha));

            spriteBatch.DrawString(
                this.Font,
                this.DisplayText,
                this.Position,
                new Color(this.Color, this.Alpha));
        }
    }
}
