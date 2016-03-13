using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class HueSaturationValue
    {
        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }

        public HueSaturationValue()
        {
            H = 0.0f;
            S = 0.0f;
            V = 0.0f;
        }

        public HueSaturationValue(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }
    }
}
