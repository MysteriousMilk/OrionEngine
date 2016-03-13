using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IScene
    {
        ICamera2D Camera { get; }

        IEnumerable<OrionObject> EnumerateScene();
        void Add(OrionObject obj);
        void Update(GameTime gameTime);
        void Draw();
        List<OrionObject> HitTest(Vector2 screenPos);
    }
}
