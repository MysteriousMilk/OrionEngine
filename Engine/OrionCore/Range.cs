using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class Range<TValue> : IParticleProperty<TValue>
    {
        private Random _Randomizer;

        public TValue UpperBound
        {
            get;
            set;
        }

        public TValue LowerBound
        {
            get;
            set;
        }

        public Range(Random random)
        {
            UpperBound = LowerBound = default(TValue);
            _Randomizer = random;
        }

        public Range(TValue single, Random random)
        {
            UpperBound = LowerBound = single;
            _Randomizer = random;
        }

        public Range(TValue upper, TValue lower, Random random)
        {
            UpperBound = upper;
            LowerBound = lower;
            _Randomizer = random;
        }

        public TValue GetNextValue()
        {
            TValue random = default(TValue);

            if (typeof(TValue) == typeof(int))
            {
                int a = (int)Convert.ChangeType(UpperBound, typeof(int));
                int b = (int)Convert.ChangeType(LowerBound, typeof(int));
                double rand = _Randomizer.NextDouble();

                int val = OrionMath.LinearInterpolate(a, b, rand);
                random = (TValue)Convert.ChangeType(val, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(float))
            {
                float a = (float)Convert.ChangeType(UpperBound, typeof(float));
                float b = (float)Convert.ChangeType(LowerBound, typeof(float));
                double rand = _Randomizer.NextDouble();

                float val = OrionMath.LinearInterpolate(a, b, rand);
                random = (TValue)Convert.ChangeType(val, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(double))
            {
                double a = (double)Convert.ChangeType(UpperBound, typeof(double));
                double b = (double)Convert.ChangeType(LowerBound, typeof(double));
                double rand = _Randomizer.NextDouble();

                double val = OrionMath.LinearInterpolate(a, b, rand);
                random = (TValue)Convert.ChangeType(val, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(Vector2))
            {
                Vector2 a = (Vector2)Convert.ChangeType(UpperBound, typeof(Vector2));
                Vector2 b = (Vector2)Convert.ChangeType(LowerBound, typeof(Vector2));
                double rand = _Randomizer.NextDouble();

                Vector2 val = OrionMath.LinearInterpolate(a, b, rand);
                random = (TValue)Convert.ChangeType(val, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(Color))
            {
                Color a = (Color)Convert.ChangeType(UpperBound, typeof(Color));
                Color b = (Color)Convert.ChangeType(LowerBound, typeof(Color));
                double rand = _Randomizer.NextDouble();

                Color val = OrionMath.LinearInterpolate(a, b, rand);
                random = (TValue)Convert.ChangeType(val, typeof(TValue));
            }

            return random;
        }

        public Random GetRandomizer()
        {
            return _Randomizer;
        }

        public override string ToString()
        {
            return "[" + LowerBound.ToString() + " - " + UpperBound.ToString() + "]";
        }
    }
}
