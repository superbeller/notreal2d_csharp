using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class Vector2D : DoublePair
    {
        public Vector2D(double d2, double d3)
            : base(d2, d3)
        {
        }

        public Vector2D(double d2, double d3, double d4, double d5)
            : base(d4 - d2, d5 - d3)
        {
        }

        public Vector2D(Point2D point2D, Point2D point2D2)
            : base(point2D2.X - point2D.X, point2D2.Y - point2D.Y)
        {
        }

        public Vector2D(double d2, double d3, Point2D point2D)
            : base(point2D.X - d2, point2D.Y - d3)
        {
        }

        public Vector2D(Vector2D vector2D)
            : base(vector2D.X, vector2D.Y)
        {
        }

        public virtual double X
        {
            get
            {
                double? d2 = (double?)this.First;
                return d2 == null ? 0.0 : d2.Value;
            }
            set
            {
                this.First = value;
            }
        }


        public virtual double Y
        {
            get
            {
                double? d2 = (double?)this.Second;
                return d2 == null ? 0.0 : d2.Value;
            }
            set
            {
                this.Second = value;
            }
        }


        public virtual Vector2D add(Vector2D vector2D)
        {
            this.X = this.X + vector2D.X;
            this.Y = this.Y + vector2D.Y;
            return this;
        }

        public virtual Vector2D add(double d2, double d3)
        {
            this.X = this.X + d2;
            this.Y = this.Y + d3;
            return this;
        }

        public virtual Vector2D subtract(Vector2D vector2D)
        {
            this.X = this.X - vector2D.X;
            this.Y = this.Y - vector2D.Y;
            return this;
        }

        public virtual Vector2D multiply(double d2)
        {
            this.X = d2 * this.X;
            this.Y = d2 * this.Y;
            return this;
        }

        public virtual Vector2D rotate(double d2)
        {
            double d3 = Math.Cos(d2);
            double d4 = Math.Sin(d2);
            double d5 = this.X;
            double d6 = this.Y;
            this.X = d5 * d3 - d6 * d4;
            this.Y = d5 * d4 + d6 * d3;
            return this;
        }

        public virtual Vector2D rotateHalfPi()
        {
            double d2 = this.X;
            double d3 = this.Y;
            this.X = -d3;
            this.Y = d2;
            return this;
        }

        public virtual Vector2D rotateMinusHalfPi()
        {
            double d2 = this.X;
            double d3 = this.Y;
            this.X = d3;
            this.Y = -d2;
            return this;
        }

        public virtual double dotProduct(Vector2D vector2D)
        {
            return MathArrays.linearCombination(this.X, vector2D.X, this.Y, vector2D.Y);
        }

        public virtual Vector2D negate()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            return this;
        }

        public virtual Vector2D normalize()
        {
            double d2 = this.Length;
            if (d2 == 0.0)
            {
                throw new System.InvalidOperationException("Can't set angle of zero-width vector.");
            }
            this.X = this.X / d2;
            this.Y = this.Y / d2;
            return this;
        }

        public virtual double Angle
        {
            get
            {
                return Math.Atan2(this.Y, this.X);
            }
        }

        public virtual Vector2D setAngle(double d2)
        {
            double d3 = this.Length;
            if (d3 == 0.0)
            {
                throw new System.InvalidOperationException("Can't set angle of zero-width vector.");
            }
            this.X = Math.Cos(d2) * d3;
            this.Y = Math.Sin(d2) * d3;
            return this;
        }

        public virtual double Length
        {
            get
            {
                return Utils.Hypot(this.X, this.Y);
            }
        }

        public virtual Vector2D setLength(double d2)
        {
            double d3 = this.Length;
            if (d3 == 0.0)
            {
                throw new System.InvalidOperationException("Can't resize zero-width vector.");
            }
            return this.multiply(d2 / d3);
        }

        public virtual double SquaredLength
        {
            get
            {
                return this.X * this.X + this.Y * this.Y;
            }
        }

        public virtual Vector2D copy()
        {
            return new Vector2D(this);
        }

        public virtual Vector2D copyNegate()
        {
            return new Vector2D(-this.X, -this.Y);
        }

        public virtual bool nearlyEquals(Vector2D vector2D, double d2)
        {
            return vector2D != null && NumberUtil.nearlyEquals(this.X, vector2D.X, d2) && NumberUtil.nearlyEquals(this.Y, vector2D.Y, d2);
        }

        //public override string ToString()
        //{
        //    return StringUtil.ToString((object)this, false, "x", "y");
        //}
    }
}
