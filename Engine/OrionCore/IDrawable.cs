using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion.Core
{
    public interface IDrawable
    {
        /// <summary>
        /// The position of the drawable in the world.
        /// This is relative to its parent (specified by the draw call),
        /// or absolute within the world if the parent is null.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the drawable specified in degrees.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// The draw order othe drawable item.
        /// </summary>
        int ZOrder { get; set; }

        /// <summary>
        /// The resource reference of the texture.
        /// </summary>
        //string ResourceReference { get; }
        Rectangle Bounds();

        /// <summary>
        /// Determines how the drawable is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        /// <param name="parent">The parent drawable.  This value can be null if the drawable 
        /// does not have a parent.</param>
        void Draw(SpriteBatch spriteBatch, IDrawable parent);
    }
}
