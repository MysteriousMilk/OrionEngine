using System;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion.Core
{
    /// <summary>
    /// Represents a drawable rectangle.
    /// </summary>
    public class Rectangle2D : GameObject, IDrawable
    {
        #region IPrimative Properties
        /// <summary>
        /// The color to fill the primative with.
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// The Color of the primative's border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// The width of the border of the primative.
        /// </summary>
        public int BorderWidth { get; set; }
        #endregion

        #region IDrawable Properties
        /// <summary>
        /// The draw order of the primative.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation { get; set; }
        #endregion

        /// <summary>
        /// Center of the primative.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public int Height { get; set; }

        public Rectangle2D(Vector2 position, int width, int height)
        {
            Position = position;
            Origin = new Vector2(position.X + width / 2, position.Y + height / 2);
            Width = width;
            Height = height;
            BorderColor = Color.White;
            FillColor = Color.White;
            BorderWidth = 1;
        }

        public Rectangle2D(Vector2 position, int width, int height, Color fill, Color border)
        {
            Position = position;
            Origin = new Vector2(position.X + width / 2, position.Y + height / 2);
            Width = width;
            Height = height;
            BorderColor = border;
            FillColor = fill;
            BorderWidth = 1;
        }

        public Rectangle2D(Vector2 position, int width, int height, Color fill, Color border, int borderWidth)
        {
            Position = position;
            Origin = new Vector2(position.X + width / 2, position.Y + height / 2);
            Width = width;
            Height = height;
            BorderColor = border;
            FillColor = fill;
            BorderWidth = borderWidth;
        }

        /// <summary>
        /// Determines how the primative is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        public void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            int x = (int)(Position.X - Origin.X);
            int y = (int)(Position.Y - Origin.Y);

            Primitives2D.FillRectangle(
                spriteBatch,
                new Rectangle(x, y, Width, Height),
                FillColor,
                (float)OrionMath.ToRadians(Rotation)
                );

            Vector2 v1 = OrionMath.RotatePointPositive(new Vector2(x, y), Origin, Rotation);
            Vector2 v2 = OrionMath.RotatePointPositive(new Vector2(x + Width, y), Origin, Rotation);
            Vector2 v3 = OrionMath.RotatePointPositive(new Vector2(x + Width, y + Height), Origin, Rotation);
            Vector2 v4 = OrionMath.RotatePointPositive(new Vector2(x, y + Height), Origin, Rotation);

            Primitives2D.DrawLine(spriteBatch, v1, v2, BorderColor, BorderWidth);
            Primitives2D.DrawLine(spriteBatch, v2, v3, BorderColor, BorderWidth);
            Primitives2D.DrawLine(spriteBatch, v3, v4, BorderColor, BorderWidth);
            Primitives2D.DrawLine(spriteBatch, v4, v1, BorderColor, BorderWidth);
        }

        public Rectangle Bounds()
        {
            int x = (int)(Position.X - Origin.X);
            int y = (int)(Position.Y - Origin.Y);

            return new Rectangle(x, y, Width, Height);
        }
    }

    public class Circle2D : GameObject, IDrawable
    {
        #region IDrawable Properties
        /// <summary>
        /// The draw order of the primative.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation { get; set; }
        #endregion

        /// <summary>
        /// Center of the primative.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Distance from the edge of the circle to the 
        /// center of the circle.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// The Color of the primative's border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// The width of the border of the primative.
        /// </summary>
        public int BorderWidth { get; set; }

        public Circle2D(float radius, Color borderColor)
        {
            Radius = radius;
            Position = Vector2.Zero;
            Origin = new Vector2(radius, radius);
            BorderColor = borderColor;
            BorderWidth = 1;
        }

        public Circle2D(Vector2 position, float radius, Color borderColor, int borderWidth)
        {
            Radius = radius;
            Position = position;
            Origin = new Vector2(radius, radius);
            BorderColor = borderColor;
            BorderWidth = borderWidth;
        }

        /// <summary>
        /// Determines how the primative is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        public void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            int sides = (int)(Radius / 4);
            Primitives2D.DrawCircle(spriteBatch, Position, Radius, sides, BorderColor, BorderWidth);
        }

        public Rectangle Bounds()
        {
            int x = (int)(Position.X - Origin.X);
            int y = (int)(Position.Y - Origin.Y);

            return new Rectangle(x, y, (int)(Radius * 2), (int)(Radius * 2));
        }
    }

    public class Line2D : GameObject, IDrawable
    {
        #region Fields
        private float _rotation = 0.0f;
        #endregion

        #region IDrawable Properties
        /// <summary>
        /// The draw order of the primative.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// The position of the entity.  For a line, it returns the location
        /// half way between the two end points.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(
                    (EndPoint1.X + EndPoint2.X) / 2,
                    (EndPoint1.Y + EndPoint2.Y) / 2
                    );
            }

            set
            {
                // recalc end points
                Vector2 delta = value - Position;
                CalculateEndPoints(delta);
            }
        }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                CalculateEndPoints(Vector2.Zero);
            }
        }
        #endregion

        /// <summary>
        /// The first end point of the line.
        /// </summary>
        public Vector2 EndPoint1 { get; set; }

        /// <summary>
        /// The second end point of the line.
        /// </summary>
        public Vector2 EndPoint2 { get; set; }

        /// <summary>
        /// The color of the line.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The thickness of the line.
        /// </summary>
        public int Thickness { get; set; }

        public Line2D(Vector2 pt1, Vector2 pt2, Color color, int thickness)
        {
            EndPoint1 = pt1;
            EndPoint2 = pt2;
            Color = color;
            Thickness = thickness;
        }

        /// <summary>
        /// Determines how the primative is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        public void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            Primitives2D.DrawLine(spriteBatch, EndPoint1, EndPoint2, Color, Thickness);
        }

        /// <summary>
        /// TODO: Implement bounds function.
        /// </summary>
        /// <returns></returns>
        public Rectangle Bounds()
        {
            return Rectangle.Empty;
        }

        private void CalculateEndPoints(Vector2 positionDelta)
        {
            Vector2 pt1 = OrionMath.RotatePointPositive(EndPoint1, Position, Rotation);
            Vector2 pt2 = OrionMath.RotatePointPositive(EndPoint2, Position, Rotation);

            EndPoint1 = pt1 + positionDelta;
            EndPoint2 = pt2 + positionDelta;
        }
    }
}
