using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class MathArrays
    {
        public static double linearCombination(double d2, double d3, double d4, double d5)
        {
            double d6 = d2 * d3;
            double d7 = d4 * d5;
            double d8 = d6 + d7;
            double d9 = 1.34217729E8 * d2;
            double d10 = d9 - (d9 - d2);
            double d11 = d2 - d10;
            double d12 = 1.34217729E8 * d3;
            double d13 = d12 - (d12 - d3);
            double d14 = d3 - d13;
            double d15 = d11 * d14 - (d6 - d10 * d13 - d11 * d13 - d10 * d14);
            double d16 = 1.34217729E8 * d4;
            double d17 = d16 - (d16 - d4);
            double d18 = d4 - d17;
            double d19 = 1.34217729E8 * d5;
            double d20 = d19 - (d19 - d5);
            double d21 = d5 - d20;
            double d22 = d18 * d21 - (d7 - d17 * d20 - d18 * d20 - d17 * d21);
            double d23 = d8 - d7;
            double d24 = d7 - (d8 - d23) + (d6 - d23);
            double d25 = d8 + (d15 + d22 + d24);
            if (double.IsNaN(d25))
            {
                d25 = d2 * d3 + d4 * d5;
            }
            return d25;
        }

        public static double linearCombination(double d2, double d3, double d4, double d5, double d6, double d7)
        {
            double d8 = d2 * d3;
            double d9 = d4 * d5;
            double d10 = d8 + d9;
            double d11 = d6 * d7;
            double d12 = d10 + d11;
            double d13 = 1.34217729E8 * d2;
            double d14 = d13 - (d13 - d2);
            double d15 = d2 - d14;
            double d16 = 1.34217729E8 * d3;
            double d17 = d16 - (d16 - d3);
            double d18 = d3 - d17;
            double d19 = d15 * d18 - (d8 - d14 * d17 - d15 * d17 - d14 * d18);
            double d20 = 1.34217729E8 * d4;
            double d21 = d20 - (d20 - d4);
            double d22 = d4 - d21;
            double d23 = 1.34217729E8 * d5;
            double d24 = d23 - (d23 - d5);
            double d25 = d5 - d24;
            double d26 = d22 * d25 - (d9 - d21 * d24 - d22 * d24 - d21 * d25);
            double d27 = 1.34217729E8 * d6;
            double d28 = d27 - (d27 - d6);
            double d29 = d6 - d28;
            double d30 = 1.34217729E8 * d7;
            double d31 = d30 - (d30 - d7);
            double d32 = d7 - d31;
            double d33 = d29 * d32 - (d11 - d28 * d31 - d29 * d31 - d28 * d32);
            double d34 = d10 - d9;
            double d35 = d9 - (d10 - d34) + (d8 - d34);
            double d36 = d12 - d11;
            double d37 = d11 - (d12 - d36) + (d10 - d36);
            double d38 = d12 + (d19 + d26 + d33 + d35 + d37);
            if (double.IsNaN(d38))
            {
                d38 = d2 * d3 + d4 * d5 + d6 * d7;
            }
            return d38;
        }
    }

}
