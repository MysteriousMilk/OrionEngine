using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public static class OrionMath
    {
        public static double ToDegrees(double radians)
        {
            return radians * (180.0f / Math.PI);
        }

        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0f);
        }

        public static float Map(float value, float istart, float istop, float ostart, float ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }

        public static float AngleBetween(Vector2 v1, Vector2 v2)
        {
            float angle = (float)Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);

            // convert to degrees
            angle = (float)(angle * (180 / Math.PI));

            // need this correction for some reason
            angle -= 90;

            if (angle > 359)
                angle = angle - 360;
            else if (angle < 0)
                angle = 360 + angle;

            return angle;
        }

        public static Vector2 RotatePointPositive(Vector2 point, Vector2 origin, float angle)
        {
            Vector2 relative = point + origin;

            double rads = ToRadians(angle);
            double x = (relative.X * Math.Cos(rads)) - (relative.Y * Math.Sin(rads));
            double y = (relative.Y * Math.Cos(rads)) + (relative.X * Math.Sin(rads));

            return (new Vector2((float)x, (float)y) - origin);
        }

        public static Vector2 VectorTruncate(Vector2 original, float max)
        {
            Vector2 vec = original;

            if (vec.Length() > max)
            {
                vec.Normalize();

                vec *= max;
            }

            return vec;
        }

        public static Vector2 VectorNormalize(Vector2 v)
        {
            Vector2 vec = v;

            float length = vec.Length();

            if (length > 0)
            {
                vec.X /= length;
                vec.Y /= length;
            }

            return vec;
        }

        /* Begin Linear Interpolation method overloads */
        public static byte LinearInterpolate(byte a, byte b, double t)
        {
            return (byte)(a * (1 - t) + b * t);
        }
        public static float LinearInterpolate(float a, float b, double t)
        {
            return (float)(a * (1 - t) + b * t);
        }
        public static double LinearInterpolate(double a, double b, double t)
        {
            return (double)(a * (1 - t) + b * t);
        }
        public static int LinearInterpolate(int a, int b, double t)
        {
            return (int)(a * (1 - t) + b * t);
        }
        public static Vector2 LinearInterpolate(Vector2 a, Vector2 b, double t)
        {
            return new Vector2(LinearInterpolate(a.X, b.X, t),
                                     LinearInterpolate(a.Y, b.Y, t));
        }
        public static Vector4 LinearInterpolate(Vector4 a, Vector4 b, double t)
        {
            return new Vector4(LinearInterpolate(a.X, b.X, t),
                                     LinearInterpolate(a.Y, b.Y, t),
                                     LinearInterpolate(a.Z, b.Z, t),
                                     LinearInterpolate(a.W, b.W, t));
        }
        public static Color LinearInterpolate(Color a, Color b, double t)
        {
            return new Color(LinearInterpolate(a.R, b.R, t),
                                     LinearInterpolate(a.G, b.G, t),
                                     LinearInterpolate(a.B, b.B, t),
                                     LinearInterpolate(a.A, b.A, t));
        }
        /* End Linear Interpolation method overloads */
    }
}
