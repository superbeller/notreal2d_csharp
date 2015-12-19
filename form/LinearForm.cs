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
	public class LinearForm : ThinForm
	{
		private readonly double length;
		private readonly double halfLength;
		private readonly double angularMassFactor;

		private double lastAngle;
		private double lastEpsilon;
		private double? lastXOffset;
		private double? lastYOffset;

		public LinearForm(double length, bool endpointCollisionEnabled) : base(Shape.LINE, endpointCollisionEnabled)
		{

			if (double.IsNaN(length) || double.IsInfinity(length) || length <= 0.0D)
			{
				throw new System.ArgumentException(string.Format("Argument 'length' should be positive finite number but got {0}.", length));
			}

			this.length = length;
			this.halfLength = length / 2.0D;
			this.angularMassFactor = length * length / 12.0D;
		}

		public LinearForm(double length) : this(length, true)
		{
		}

		public virtual double Length
		{
			get
			{
				return length;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public com.codeforces.commons.geometry.Point2D getPoint1(@Nonnull com.codeforces.commons.geometry.Point2D position, double angle, double epsilon)
		public virtual Point2D getPoint1(Point2D position, double angle, double epsilon)
		{
			updateLastOffsets(angle, epsilon);
			return new Point2D(position.X - lastXOffset.Value, position.Y - lastYOffset.Value);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public com.codeforces.commons.geometry.Point2D getPoint2(@Nonnull com.codeforces.commons.geometry.Point2D position, double angle, double epsilon)
		public virtual Point2D getPoint2(Point2D position, double angle, double epsilon)
		{
			updateLastOffsets(angle, epsilon);
			return new Point2D(position.X + lastXOffset.Value, position.Y + lastYOffset.Value);
		}

		public override double CircumcircleRadius
		{
			get
			{
				return halfLength;
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
        //    return StringUtil.ToString(this, false, "length");
        //}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("FloatingPointEquality") private void updateLastOffsets(double angle, double epsilon)
		private void updateLastOffsets(double angle, double epsilon)
		{
			if (lastXOffset == null || lastYOffset == null || angle != lastAngle || epsilon != lastEpsilon)
			{
				if (double.IsNaN(angle) || double.IsInfinity(angle))
				{
					throw new System.ArgumentException("Argument 'angle' is not a finite number.");
				}

				if (double.IsNaN(epsilon) || double.IsInfinity(epsilon) || epsilon < 1.0E-100D || epsilon > 1.0D)
				{
					throw new System.ArgumentException("Argument 'epsilon' should be between 1.0E-100 and 1.0.");
				}

				lastAngle = angle;
				lastEpsilon = epsilon;

				if (Math.Abs(length) < epsilon)
				{
					lastXOffset = 0.0D;
					lastYOffset = 0.0D;
				}
				else
				{
                    if (Math.Abs(Utils.HALF_PI - Math.Abs(angle)) < epsilon)
					{
						lastXOffset = 0.0D;
					}
					else
					{
                        lastXOffset = normalizeSinCos(Math.Cos(angle), epsilon) * halfLength;
					}

                    if (Math.Abs(Utils.PI - Math.Abs(angle)) < epsilon || Math.Abs(angle) < epsilon)
					{
						lastYOffset = 0.0D;
					}
					else
					{
                        lastYOffset = normalizeSinCos(Math.Sin(angle), epsilon) * halfLength;
					}
				}
			}
		}
	}

}