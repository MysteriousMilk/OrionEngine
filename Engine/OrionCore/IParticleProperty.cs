using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IParticleProperty<TValue>
    {
        TValue GetNextValue();
    }
}
