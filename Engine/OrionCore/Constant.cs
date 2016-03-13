using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class Constant<TValue> : IParticleProperty<TValue>
    {
        public TValue ConstantValue
        {
            get;
            set;
        }

        public Constant()
        {
            ConstantValue = default(TValue);
        }

        public Constant(TValue constant)
        {
            ConstantValue = constant;
        }

        public TValue GetNextValue()
        {
            return ConstantValue;
        }
    }
}
