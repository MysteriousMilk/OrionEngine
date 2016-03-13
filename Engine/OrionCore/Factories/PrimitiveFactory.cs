using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Factories
{
    public static class PrimitiveFactory
    {
        public static IPrimitive CreateRectangle(Microsoft.Xna.Framework.Vector2 position, int width, int height, Microsoft.Xna.Framework.Color color)
        {
            OrionRectangle rect = new OrionRectangle();
            rect.Width = width;
            rect.Height = height;
            rect.Position = position;
            rect.Tint = color;
            return rect;
        }
    }
}
