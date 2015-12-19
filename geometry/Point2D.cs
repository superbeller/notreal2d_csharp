using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class Point2D : DoublePair
    {
        public Point2D()
            : base (0, 0)
        {
        }

        public Point2D(double d2, double d3)
            : base(d2, d3)
        {
        }

        public Point2D(Point2D point2D)
            : base(point2D.X, point2D.Y)
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


        public virtual Point2D add(Vector2D vector2D)
        {
            this.X = this.X + vector2D.X;
            this.Y = this.Y + vector2D.Y;
            return this;
        }

        public virtual Point2D add(double d2, double d3)
        {
            this.X = this.X + d2;
            this.Y = this.Y + d3;
            return this;
        }

        public virtual Point2D subtract(Vector2D vector2D)
        {
            this.X = this.X - vector2D.X;
            this.Y = this.Y - vector2D.Y;
            return this;
        }

        public virtual Point2D subtract(double d1, double d2)
        {
            this.X = this.X - d1;
            this.Y = this.Y - d2;
            return this;
        }

        public virtual double getDistanceTo(Point2D point2D)
        {
            return Utils.Hypot(this.X - point2D.X, this.Y - point2D.Y);
        }

        public virtual double getDistanceTo(double d1, double d2)
        {
            return Utils.Hypot(this.X - d1, this.Y - d2);
        }

        public virtual double getSquaredDistanceTo(Point2D point2D)
        {
            return Utils.SumSqr(this.X - point2D.X, this.Y - point2D.Y);
        }

        public virtual double getSquaredDistanceTo(double d1, double d2)
        {
            return Utils.SumSqr(this.X - d1, this.Y - d2);
        }

        public virtual Point2D copy()
        {
            return new Point2D(this);
        }

        public virtual bool nearlyEquals(Point2D point2D, double d2)
        {
            return point2D != null && NumberUtil.nearlyEquals(this.X, point2D.X, d2) && NumberUtil.nearlyEquals(this.Y, point2D.Y, d2);
        }

        //public override string ToString()
        //{
        //    return StringUtil.ToString((object)this, false, "x", "y");
        //}
    }

}
