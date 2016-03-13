using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IUpdatable
    {
        void Update(GameTime gameTime, IUpdatable parent);
    }
}
