namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;
    //using GeometryUtil = com.codegame.codeseries.notreal2d.util.GeometryUtil;

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.DOUBLE_PI;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.06.2015
	/// </summary>
	public class ArcForm : ThinForm
	{
		private readonly double radius;
		private readonly double angle;
		private readonly double sector;

		public ArcForm(double radius, double angle, double sector, bool endpointCollisionEnabled) : base(Shape.ARC, endpointCollisionEnabled)
		{

			if (double.IsNaN(radius) || double.IsInfinity(radius) || radius <= 0.0D)
			{
				throw new System.ArgumentException(string.Format("Argument 'radius' should be a positive finite number but got {0}.", radius));
			}

			if (double.IsNaN(angle) || double.IsInfinity(angle))
			{
				throw new System.ArgumentException(string.Format("Argument 'angle' should be a finite number but got {0}.", angle));
			}

			if (double.IsNaN(sector) || double.IsInfinity(sector) || sector <= 0.0D || sector > Utils.DOUBLE_PI)
			{
				throw new System.ArgumentException(string.Format("Argument 'sector' should be between 0.0 exclusive and 2 * PI inclusive but got {0}.", sector));
			}

			this.angle = GeometryUtil.normalizeAngle(angle);
			this.radius = radius;
			this.sector = sector;
		}

		public ArcForm(double radius, double angle, double sector) : this(radius, angle, sector, true)
		{
		}

		public virtual double Radius
		{
			get
			{
				return radius;
			}
		}

		public virtual double Angle
		{
			get
			{
				return angle;
			}
		}

		public virtual double Sector
		{
			get
			{
				return sector;
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
			return position; // TODO just a method stub, does not really return a center of mass
		}

		public override double getAngularMass(double mass)
		{
			if (double.IsInfinity(mass) && mass != double.NegativeInfinity)
			{
				return mass;
			}

			throw new System.ArgumentException("Arc form is only supported for static bodies.");
		}

        //public override string ToString()
        //{
        //    return StringUtil.ToString(this, false, "radius", "angle", "sector");
        //}
	}

}