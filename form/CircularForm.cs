namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public class CircularForm : Form
	{
		private readonly double radius;
		private readonly double angularMassFactor;

		public CircularForm(double radius) : base(Shape.CIRCLE)
		{

			if (double.IsNaN(radius) || double.IsInfinity(radius) || radius <= 0.0D)
			{
				throw new System.ArgumentException(string.Format("Argument 'radius' should be positive finite number but got {0}.", radius));
			}

			this.radius = radius;
			this.angularMassFactor = radius * radius / 2.0D;
		}

		public virtual double Radius
		{
			get
			{
				return radius;
			}
		}

		public override double CircumcircleRadius
		{
			get
			{
				return radius;
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
        //    return StringUtil.ToString(this, false, "radius");
        //}
	}

}