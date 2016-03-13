using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class RandomNumber<TValue> : IParticleProperty<TValue>
    {
        private Random _Random;

        public bool AllowNegativeValues
        {
            get;
            set;
        }

        public float Scalar
        {
            get;
            set;
        }

        public RandomNumber(Random random)
        {
            _Random = random;
            Scalar = 1.0f;
            AllowNegativeValues = false;
        }

        public TValue GetNextValue()
        {
            TValue nextValue = default(TValue);

            float rand1;
            float rand2;
            float rand3;

            if (!AllowNegativeValues)
            {
                rand1 = Scalar * (float)_Random.NextDouble();
                rand2 = Scalar * (float)_Random.NextDouble();
                rand3 = Scalar * (float)_Random.NextDouble();
            }
            else
            {
                rand1 = Scalar * (float)(_Random.NextDouble() * 2 - 1);
                rand2 = Scalar * (float)(_Random.NextDouble() * 2 - 1);
                rand3 = Scalar * (float)(_Random.NextDouble() * 2 - 1);
            }

            if (typeof(TValue) == typeof(int))
            {
                nextValue = (TValue)Convert.ChangeType(rand1, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(float))
            {
                nextValue = (TValue)Convert.ChangeType(rand1, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(double))
            {
                nextValue = (TValue)Convert.ChangeType(rand1, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(Vector2))
            {
                Vector2 val = new Vector2(rand1, rand2);
                nextValue = (TValue)Convert.ChangeType(val, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(Color))
            {
                //float hue = (float)_Random.NextDouble() * 6.0f;
                //Color c = Utilities.HSVToColor(new HueSaturationValue(hue, 0.5f, 1.0f));
                Color c = new Color((int)rand1, (int)rand2, (int)rand3);
                nextValue = (TValue)Convert.ChangeType(c, typeof(TValue));
            }

            return nextValue;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Scalar.ToString() + " * ");

            if (AllowNegativeValues)
                builder.Append("(");

            builder.Append("RAND()");

            if (AllowNegativeValues)
                builder.Append(" * 2 - 1)");

            return builder.ToString();
        }
    }
}
