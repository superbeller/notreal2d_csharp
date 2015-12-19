

using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.*;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public class RectangularForm : Form
	{
		private readonly double width;
		private readonly double height;
		private readonly double halfWidth;
		private readonly double halfHeight;
		private readonly double circumcircleRadius;
		private readonly double angularMassFactor;

		public RectangularForm(double width, double height) : base(Shape.RECTANGLE)
		{

			if (double.IsNaN(width) || double.IsInfinity(width) || width <= 0.0D)
			{
				throw new System.ArgumentException(string.Format("Argument 'width' should be positive finite number but got {0}.", width));
			}

			if (double.IsNaN(height) || double.IsInfinity(height) || height <= 0.0D)
			{
				throw new System.ArgumentException(string.Format("Argument 'height' should be positive finite number but got {0}.", height));
			}

			this.width = width;
			this.height = height;
			this.halfWidth = width / 2.0D;
			this.halfHeight = height / 2.0D;
			this.circumcircleRadius = Utils.Hypot(width, height) / 2.0D;
            this.angularMassFactor = Utils.SumSqr(width, height) / 12.0D;
		}

		public virtual double Width
		{
			get
			{
				return width;
			}
		}

		public virtual double Height
		{
			get
			{
				return height;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public com.codeforces.commons.geometry.Point2D[] getPoints(@Nonnull com.codeforces.commons.geometry.Point2D position, double angle, double epsilon)
		public virtual Point2D[] getPoints(Point2D position, double angle, double epsilon)
		{
			if (double.IsNaN(angle) || double.IsInfinity(angle))
			{
				throw new System.ArgumentException("Argument 'angle' is not a finite number.");
			}

			if (double.IsNaN(epsilon) || double.IsInfinity(epsilon) || epsilon < 1.0E-100D || epsilon > 1.0D)
			{
				throw new System.ArgumentException("Argument 'epsilon' should be between 1.0E-100 and 1.0.");
			}

			double sin = normalizeSinCos(Math.Sin(angle), epsilon);
            double cos = normalizeSinCos(Math.Cos(angle), epsilon);

			double lengthwiseXOffset = cos * halfWidth;
			double lengthwiseYOffset = sin * halfWidth;

			double crosswiseXOffset = sin * halfHeight;
			double crosswiseYOffset = -cos * halfHeight;

			return new Point2D[]
			{
				new Point2D(position.X - lengthwiseXOffset + crosswiseXOffset, position.Y - lengthwiseYOffset + crosswiseYOffset),
				new Point2D(position.X + lengthwiseXOffset + crosswiseXOffset, position.Y + lengthwiseYOffset + crosswiseYOffset),
				new Point2D(position.X + lengthwiseXOffset - crosswiseXOffset, position.Y + lengthwiseYOffset - crosswiseYOffset),
				new Point2D(position.X - lengthwiseXOffset - crosswiseXOffset, position.Y - lengthwiseYOffset - crosswiseYOffset)
			};
		}

		public override double CircumcircleRadius
		{
			get
			{
				return circumcircleRadius;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public com.codeforces.commons.geometry.Point2D getCenterOfMass(@Nonnull com.codeforces.commons.geometry.Point2D position, double angle)
		public override Point2D getCenterOfMass(Point2D position, double angle)
		{
			return position;
		}

		public override double getAngularMass(double mass)
		{
			return mass * angularMassFactor;
		}

        //public override string ToString()
        //{
        //    return StringUtil.ToString(this, false, "width", "height");
        //}
	}

}