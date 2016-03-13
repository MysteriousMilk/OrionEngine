using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface ISprite
    {
        int Id { get; set; }
        string Tag { get; set; }
        Vector2 Position { get; set; }
        Vector2 Origin { get; set; }
        float Rotation { get; set; }
        Texture2D Texture { get; set; }
        SpriteDefinition Definition { get; set; }
        int ZOrder { get; set; }

        /// <summary>
        /// The scale of the drawable item.
        /// </summary>
        float Scale { get; set; }

        /// <summary>
        /// The alpha of the drawable item.
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// The color to tint the drawable item.
        /// </summary>
        Color Tint { get; set; }

        Rectangle Bounds();
        void Draw(SpriteBatch spriteBatch, IDrawable parent);
        object Clone();
    }
}
