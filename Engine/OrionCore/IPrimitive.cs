using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IPrimitive
    {
        /// <summary>
        /// The color to tint the primative.
        /// </summary>
        Color Tint { get; set; }

        /// <summary>
        /// The Color of the primative's border.
        /// </summary>
        Color BorderColor { get; set; }

        /// <summary>
        /// The draw order of the primative.
        /// </summary>
        int ZOrder { get; set; }

        /// <summary>
        /// The width of the border of the primative.
        /// </summary>
        int BorderWidth { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Determines how the primative is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
