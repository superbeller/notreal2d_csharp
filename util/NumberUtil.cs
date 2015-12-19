using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class NumberUtil
    {
        public static sbyte toByte(int n2)
        {
            sbyte by = (sbyte)n2;
            if (by == n2)
            {
                return by;
            }
            throw new System.ArgumentException("Can't convert int " + n2 + " to byte.");
        }

        public static int? toInt(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is sbyte?)
            {
                return (sbyte?)obj;
            }
            if (obj is short?)
            {
                return (int)((short?)obj);
            }
            if (obj is int?)
            {
                return (int?)obj;
            }
            if (obj is long?)
            {
                return NumberUtil.toInt((long?)obj);
            }
            if (obj is float?)
            {
                return NumberUtil.toInt(((float?)obj).Value);
            }
            if (obj is double?)
            {
                return NumberUtil.toInt((double?)obj);
            }            

            return NumberUtil.toInt(double.Parse(obj.ToString()));
        }

        public static int toInt(long l2)
        {
            int n2 = (int)l2;
            if ((long)n2 == l2)
            {
                return n2;
            }
            throw new System.ArgumentException("Can't convert long " + l2 + " to int.");
        }

        public static int toInt(float f2)
        {
            int n2 = (int)f2;
            if (Math.Abs((float)n2 - f2) < 1.0f)
            {
                return n2;
            }
            throw new System.ArgumentException("Can't convert float " + f2 + " to int.");
        }

        public static int toInt(double d2)
        {
            int n2 = (int)d2;
            if (Math.Abs((double)n2 - d2) < 1.0)
            {
                return n2;
            }
            throw new System.ArgumentException("Can't convert double " + d2 + " to int.");
        }

        public static bool Equals(int? n2, int? n3)
        {
            return n2 == null ? n3 == null : n2.Equals(n3);
        }

        public static bool Equals(long? l2, long? l3)
        {
            return l2 == null ? l3 == null : l2.Equals(l3);
        }

        public static bool Equals(double? d2, double? d3)
        {
            return d2 == null ? d3 == null : d2.Equals(d3);
        }

        public static bool nearlyEquals(double? d2, double? d3, double d4)
        {
            if (d2 == null)
            {
                return d3 == null;
            }
            if (d3 == null)
            {
                return false;
            }
            if (d2.Equals(d3))
            {
                return true;
            }
            if (double.IsInfinity(d2.Value) || double.IsNaN(d2.Value) || double.IsInfinity(d3.Value) || double.IsNaN(d3.Value))
            {
                return false;
            }
            return Math.Abs(d2.Value - d3.Value) < d4;
        }
    }

}
