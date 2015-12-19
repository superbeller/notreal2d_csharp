using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{    
    public class Line2D
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly double pseudoLength;

        public Line2D(double d2, double d3, double d4)
        {
            this.a = d2;
            this.b = d3;
            this.c = d4;
            this.pseudoLength = Utils.Hypot(this.a, this.b);
        }

        public virtual Line2D getParallelLine(double d2, double d3)
        {
            double d4 = this.a * d2 + this.b * d3 + this.c;
            return new Line2D(this.a, this.b, this.c - d4);
        }

        public virtual Line2D getParallelLine(Point2D point2D)
        {
            return this.getParallelLine(point2D.X, point2D.Y);
        }

        public virtual double getDistanceFrom(double d2, double d3)
        {
            return Math.Abs((this.a * d2 + this.b * d3 + this.c) / this.pseudoLength);
        }

        public virtual double getDistanceFrom(Point2D point2D)
        {
            return this.getDistanceFrom(point2D.X, point2D.Y);
        }

        public virtual double getDistanceFrom(Line2D line2D, double d2)
        {
            if (this.getIntersectionPoint(line2D, d2) != null)
            {
                return double.NaN;
            }
            return Math.Abs(this.c - line2D.c) / this.pseudoLength;
        }

        public virtual double getSignedDistanceFrom(double d2, double d3)
        {
            return (this.a * d2 + this.b * d3 + this.c) / this.pseudoLength;
        }

        public virtual double getSignedDistanceFrom(Point2D point2D)
        {
            return this.getSignedDistanceFrom(point2D.X, point2D.Y);
        }

        public virtual Vector2D UnitNormal
        {
            get
            {
                return new Vector2D(this.a / this.pseudoLength, this.b / this.pseudoLength);
            }
        }

        public virtual Vector2D getUnitNormalFrom(double d2, double d3, double d4)
        {
            double d5 = this.getSignedDistanceFrom(d2, d3);
            if (d5 <= -d4)
            {
                return new Vector2D(this.a / this.pseudoLength, this.b / this.pseudoLength);
            }
            if (d5 >= d4)
            {
                return new Vector2D((-this.a) / this.pseudoLength, (-this.b) / this.pseudoLength);
            }
            throw new System.ArgumentException(string.Format("Point {{x={0}, y={1}}} is on the {2}.", d2, d3, this));
        }

        public virtual Vector2D getUnitNormalFrom(Point2D point2D, double d2)
        {
            return this.getUnitNormalFrom(point2D.X, point2D.Y, d2);
        }

        public virtual Vector2D getUnitNormalFrom(Point2D point2D)
        {
            return this.getUnitNormalFrom(point2D.X, point2D.Y, 1.0E-6);
        }

        public virtual Point2D getProjectionOf(double d2, double d3, double d4)
        {
            double d5 = this.getDistanceFrom(d2, d3);
            if (d5 < d4)
            {
                return new Point2D(d2, d3);
            }
            Vector2D vector2D = this.getUnitNormalFrom(d2, d3, d4);
            return new Point2D(d2 + vector2D.X * d5, d3 + vector2D.Y * d5);
        }

        public virtual Point2D getProjectionOf(Point2D point2D, double d2)
        {
            return this.getProjectionOf(point2D.X, point2D.Y, d2);
        }

        public virtual Point2D getProjectionOf(Point2D point2D)
        {
            return this.getProjectionOf(point2D.X, point2D.Y, 1.0E-6);
        }

        public virtual Point2D getIntersectionPoint(Line2D line2D, double d2)
        {
            double d3 = this.a * line2D.b - line2D.a * this.b;
            return Math.Abs(d3) < Math.Abs(d2) ? null : new Point2D((this.b * line2D.c - line2D.b * this.c) / d3, (line2D.a * this.c - this.a * line2D.c) / d3);
        }

        //public override string ToString()
        //{
        //    return StringUtil.ToString((object)this, false, "a", "b", "c");
        //}

        public static Line2D getLineByTwoPoints(double d2, double d3, double d4, double d5)
        {
            return new Line2D(d5 - d3, d2 - d4, (d3 - d5) * d2 + (d4 - d2) * d3);
        }

        public static Line2D getLineByTwoPoints(Point2D point2D, Point2D point2D2)
        {
            return Line2D.getLineByTwoPoints(point2D.X, point2D.Y, point2D2.X, point2D2.Y);
        }
    }

}
