using C3.XNA;
using Orion.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class OrionRectangle : OrionObject, IPrimitive
    {
        #region IPrimative Properties
        /// <summary>
        /// The color to tint the primative.
        /// </summary>
        public Microsoft.Xna.Framework.Color Tint { get; set; }

        /// <summary>
        /// The Color of the primative's border.
        /// </summary>
        public Microsoft.Xna.Framework.Color BorderColor { get; set; }

        /// <summary>
        /// The draw order of the primative.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// The width of the border of the primative.
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Position { get; set; }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation { get; set; }
        #endregion

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Determines how the primative is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            int x = (int)(this.Position.X - (this.Width / 2));
            int y = (int)(this.Position.Y - (this.Height / 2));

            Primitives2D.FillRectangle(
                spriteBatch,
                new Microsoft.Xna.Framework.Rectangle(x, y, this.Width, this.Height),
                this.Tint,
                this.Rotation
                );
        }
    }
}
