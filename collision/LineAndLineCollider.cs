using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using LinearForm = com.codegame.codeseries.notreal2d.form.LinearForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;
    //using NotImplementedException = org.apache.commons.lang3.NotImplementedException;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.max;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.min;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 08.06.2015
	/// </summary>
	public class LineAndLineCollider : ColliderBase
	{
		public LineAndLineCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.LINE && bodyB.Form.Shape == Shape.LINE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"OverlyLongMethod", "OverlyComplexMethod"}) @Nullable @Override public CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
        protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			if (2 + 2 == 2 * 2)
			{
				throw new NotImplementedException("Soon, very soon. Maybe...");
			}

			LinearForm linearFormA = (LinearForm) bodyA.Form;
			LinearForm linearFormB = (LinearForm) bodyB.Form;

			Point2D point1A = linearFormA.getPoint1(bodyA.Position, bodyA.Angle, epsilon);
			Point2D point2A = linearFormA.getPoint2(bodyA.Position, bodyA.Angle, epsilon);

			Point2D point1B = linearFormB.getPoint1(bodyB.Position, bodyB.Angle, epsilon);
			Point2D point2B = linearFormB.getPoint2(bodyB.Position, bodyB.Angle, epsilon);

			Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);
			Line2D lineB = Line2D.getLineByTwoPoints(point1B, point2B);

			Point2D intersectionPoint = lineA.getIntersectionPoint(lineB, epsilon);
			if (intersectionPoint == null)
			{
				return null;
			}

            double leftA = Math.Min(point1A.X, point2A.X);
            double topA = Math.Min(point1A.Y, point2A.Y);
            double rightA = Math.Max(point1A.X, point2A.X);
            double bottomA = Math.Max(point1A.Y, point2A.Y);

			if (intersectionPoint.X <= leftA - epsilon || intersectionPoint.X >= rightA + epsilon || intersectionPoint.Y <= topA - epsilon || intersectionPoint.Y >= bottomA + epsilon)
			{
				return null;
			}

            double leftB = Math.Min(point1B.X, point2B.X);
            double topB = Math.Min(point1B.Y, point2B.Y);
            double rightB = Math.Max(point1B.X, point2B.X);
            double bottomB = Math.Max(point1B.Y, point2B.Y);

			if (intersectionPoint.X <= leftB - epsilon || intersectionPoint.X >= rightB + epsilon || intersectionPoint.Y <= topB - epsilon || intersectionPoint.Y >= bottomB + epsilon)
			{
				return null;
			}

			Vector2D collisionNormalB = lineA.getUnitNormalFrom(bodyB.Position).multiply(-1.0D); // TODO wrong?
            double depth = Math.Min(lineA.getDistanceFrom(point1B), lineA.getDistanceFrom(point2B));

			return new CollisionInfo(bodyA, bodyB, intersectionPoint, collisionNormalB, depth, epsilon); // TODO negate normal?
		}
	}

}