using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion.Core
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
            DisplayText = text;
            Position = position;
            Font = font;
            Alpha = 1.0f;
        }

        public Text(string text, Vector2 position, SpriteFont font, Color color)
            : this(text, position, font)
        {
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                Font,
                DisplayText,
                Position + new Vector2(2, 2),
                new Color(Color.Black, Alpha));

            spriteBatch.DrawString(
                Font,
                DisplayText,
                Position,
                new Color(Color, Alpha));
        }
    }
}
