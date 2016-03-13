using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion.Core
{
    public class Particle
    {
        private float _maxLife;

        public Texture2D Texture { get; set; }
        public int Alpha { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 StartVelocity { get; set; }
        public Vector2 EndVelocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public float Size { get; set; }
        public float Life { get; set; }
        public bool Alive { get; set; }

        public Particle(Texture2D texture, Vector2 pos, Vector2 startVel, Vector2 endVel,
                        float angle, float aVel, Color start, Color end, int alpha, float size, float life)
        {
            Alive = true;
            Texture = texture;
            Position = pos;
            StartVelocity = startVel;
            EndVelocity = endVel;
            Angle = angle;
            AngularVelocity = aVel;
            StartColor = start;
            EndColor = end;
            Alpha = alpha;
            Size = size;
            Life = life;
            _maxLife = life;
        }

        public void Update(GameTime gameTime)
        {
            if (Alive)
            {
                if (Life > 0)
                {
                    Life -= gameTime.ElapsedGameTime.Milliseconds;

                    Vector2 vel = OrionMath.LinearInterpolate(EndVelocity, StartVelocity, Life / _maxLife);

                    Position += vel * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Angle += AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                    Alive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Color color = OrionMath.LinearInterpolate(EndColor, StartColor, Life / _maxLife);

            spriteBatch.Draw(Texture, Position, sourceRectangle, new Color(color, Alpha),
                Angle, origin, Size, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Color color = OrionMath.LinearInterpolate(EndColor, StartColor, Life / _maxLife);

            spriteBatch.Draw(Texture, offset + Position, sourceRectangle, new Color(color, Alpha),
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
