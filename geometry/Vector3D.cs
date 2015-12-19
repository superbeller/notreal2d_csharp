using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    [Serializable]
    public class Vector3D : ArrayList
    {
        public static readonly Vector3D ZERO = new Vector3D(0.0, 0.0, 0.0);
        public static readonly Vector3D PLUS_I = new Vector3D(1.0, 0.0, 0.0);
        public static readonly Vector3D MINUS_I = new Vector3D(-1.0, 0.0, 0.0);
        public static readonly Vector3D PLUS_J = new Vector3D(0.0, 1.0, 0.0);
        public static readonly Vector3D MINUS_J = new Vector3D(0.0, -1.0, 0.0);
        public static readonly Vector3D PLUS_K = new Vector3D(0.0, 0.0, 1.0);
        public static readonly Vector3D MINUS_K = new Vector3D(0.0, 0.0, -1.0);
        public static readonly Vector3D NaN_Renamed = new Vector3D(double.NaN, double.NaN, double.NaN);
        public static readonly Vector3D POSITIVE_INFINITY = new Vector3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public static readonly Vector3D NEGATIVE_INFINITY = new Vector3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        private readonly double x;
        private readonly double y;
        private readonly double z;

        public Vector3D(double d2, double d3, double d4)
        {
            this.x = d2;
            this.y = d3;
            this.z = d4;
        }

        public virtual double X
        {
            get
            {
                return this.x;
            }
        }

        public virtual double Y
        {
            get
            {
                return this.y;
            }
        }

        public virtual double Z
        {
            get
            {
                return this.z;
            }
        }

        public virtual double Norm
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        public virtual double NormSq
        {
            get
            {
                return this.x * this.x + this.y * this.y + this.z * this.z;
            }
        }

        public virtual Vector3D add(ArrayList vector)
        {
            Vector3D vector3D = (Vector3D)vector;
            return new Vector3D(this.x + vector3D.x, this.y + vector3D.y, this.z + vector3D.z);
        }

        public virtual Vector3D subtract(ArrayList vector)
        {
            Vector3D vector3D = (Vector3D)vector;
            return new Vector3D(this.x - vector3D.x, this.y - vector3D.y, this.z - vector3D.z);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public Vector3D normalize() throws MathArithmeticException
        public virtual Vector3D normalize()
        {
            double d2 = this.Norm;
            if (d2 == 0.0)
            {
                throw new ArgumentNullException("CANNOT_NORMALIZE_A_ZERO_NORM_VECTOR");
            }
            return this.scalarMultiply(1.0 / d2);
        }

        public virtual Vector3D scalarMultiply(double d2)
        {
            return new Vector3D(d2 * this.x, d2 * this.y, d2 * this.z);
        }

        public virtual bool NaN
        {
            get
            {
                return double.IsNaN(this.x) || double.IsNaN(this.y) || double.IsNaN(this.z);
            }
        }

        public override bool Equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (@object is Vector3D)
            {
                Vector3D vector3D = (Vector3D)@object;
                if (vector3D.NaN)
                {
                    return this.NaN;
                }
                return this.x == vector3D.x && this.y == vector3D.y && this.z == vector3D.z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (this.NaN)
            {
                return 642;
            }
            return 643 * (164 * this.x.GetHashCode() + 3 * this.y.GetHashCode() + this.z.GetHashCode());
        }

        public virtual double dotProduct(ArrayList vector)
        {
            Vector3D vector3D = (Vector3D)vector;
            return MathArrays.linearCombination(this.x, vector3D.x, this.y, vector3D.y, this.z, vector3D.z);
        }

        public virtual Vector3D crossProduct(ArrayList vector)
        {
            Vector3D vector3D = (Vector3D)vector;
            return new Vector3D(MathArrays.linearCombination(this.y, vector3D.z, -this.z, vector3D.y), MathArrays.linearCombination(this.z, vector3D.x, -this.x, vector3D.z), MathArrays.linearCombination(this.x, vector3D.y, -this.y, vector3D.x));
        }

        //public override string ToString()
        //{
        //    return Vector3DFormat.Instance.format(this);
        //}
    }

}
