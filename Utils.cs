using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public static class Utils
    {
        private static long identNumber = 0;

        public readonly static double PI = Math.PI;
        public readonly static double DOUBLE_PI = Math.PI * 2;
        public readonly static double HALF_PI = Math.PI / 2;

        //maybe hypot is bad
        public static double Hypot(double d2, double d3)
        {
            return Math.Sqrt(Math.Pow(d2, 2) + Math.Pow(d3, 2));
        }

        public static long IncrementAndGet()
        {
            identNumber++;
            return identNumber;
        }

        //public static double hypot(double d2, double d3)
        //{
        //    int n2;
        //    if (double.IsInfinity(d2) || double.IsInfinity(d3))
        //    {
        //        return double.PositiveInfinity;
        //    }
        //    if (double.IsNaN(d2) || double.IsNaN(d3))
        //    {
        //        return double.NaN;
        //    }
        //    int n3 = FastMath.getExponent(d2);
        //    if (n3 > (n2 = FastMath.getExponent(d3)) + 27)
        //    {
        //        return FastMath.abs(d2);
        //    }
        //    if (n2 > n3 + 27)
        //    {
        //        return FastMath.abs(d3);
        //    }
        //    int n4 = (n3 + n2) / 2;
        //    double d4 = FastMath.scalb(d2, -n4);
        //    double d5 = FastMath.scalb(d3, -n4);
        //    double d6 = FastMath.sqrt(d4 * d4 + d5 * d5);
        //    return FastMath.scalb(d6, n4);
        //}

        public static double SumSqr(double d2, double d3)
        {
            return d2 * d2 + d3 * d3;
        }
    }
}
