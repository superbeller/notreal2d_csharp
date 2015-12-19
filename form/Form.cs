
using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Preconditions = com.google.common.@base.Preconditions;
    //using Contract = org.jetbrains.annotations.Contract;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.abs;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public abstract class Form
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private final Shape shape;
		private readonly Shape shape;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: protected Form(@Nonnull Shape shape)
		protected internal Form(Shape shape)
		{
            if (shape == null)
            {
                throw new ArgumentNullException("Argument 'shape' is null.");
            }			

			this.shape = shape;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public Shape getShape()
		public virtual Shape Shape
		{
			get
			{
				return shape;
			}
		}

		public abstract double CircumcircleRadius {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public abstract com.codeforces.commons.geometry.Point2D getCenterOfMass(@Nonnull com.codeforces.commons.geometry.Point2D position, double angle);
		public abstract Point2D getCenterOfMass(Point2D position, double angle);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public final com.codeforces.commons.geometry.Point2D getCenterOfMass(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public Point2D getCenterOfMass(Body body)
		{
			return getCenterOfMass(body.Position, body.Angle);
		}

		public abstract double getAngularMass(double mass);

        //public override abstract string ToString();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public static String toString(@Nullable Form form)
        //public static string ToString(Form form)
        //{
        //    return form == null ? "Form {null}" : form.ToString();
        //}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(pure = true) protected static double normalizeSinCos(double value, double epsilon)
		protected internal static double normalizeSinCos(double value, double epsilon)
		{
            return Math.Abs(value) < epsilon ? 0.0D : Math.Abs(1.0D - value) < epsilon ? 1.0D : Math.Abs(-1.0D - value) < epsilon ? -1.0D : value;
		}
	}

}