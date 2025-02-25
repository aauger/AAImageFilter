﻿namespace FilterDotNet.Utils
{
    public class Utilities
    {
        public static double Map(double x, double a, double b, double c, double d)
        {
            return (x - a) * ((d - c) / (b - a)) + c;
        }

        public static T Clamp<T>(T input, T min, T max) where T : IComparable<T>
        {
            if (input.CompareTo(min) < 0)
                return min;
            if (input.CompareTo(max) > 0)
                return max;
            return input;
        }

        public static float FloatClamp(int input)
        {
            return Clamp(input, 0.0f, 1.0f);
        }

        public static double DegToRad(double degrees)
        {
            return (degrees * Math.PI) / 180.0;
        }

        public static double Lerp(double v0, double v1, double t)
        {
            return v0 + t * (v1 - v0);
        }
    }
}
