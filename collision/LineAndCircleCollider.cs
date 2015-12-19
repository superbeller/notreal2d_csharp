using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using CircularForm = com.codegame.codeseries.notreal2d.form.CircularForm;
    //using LinearForm = com.codegame.codeseries.notreal2d.form.LinearForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.max;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.min;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 08.06.2015
	/// </summary>
	public class LineAndCircleCollider : ColliderBase
	{
		public LineAndCircleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.LINE && bodyB.Form.Shape == Shape.CIRCLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			LinearForm linearFormA = (LinearForm) bodyA.Form;
			CircularForm circularFormB = (CircularForm) bodyB.Form;

			Point2D point1A = linearFormA.getPoint1(bodyA.Position, bodyA.Angle, epsilon);
			Point2D point2A = linearFormA.getPoint2(bodyA.Position, bodyA.Angle, epsilon);

			return collideOneWay(bodyA, bodyB, point1A, point2A, circularFormB, epsilon);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("OverlyLongMethod") @Nullable static CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB, @Nonnull com.codeforces.commons.geometry.Point2D point1A, @Nonnull com.codeforces.commons.geometry.Point2D point2A, @Nonnull com.codegame.codeseries.notreal2d.form.CircularForm circularFormB, double epsilon)
		internal static CollisionInfo collideOneWay(Body bodyA, Body bodyB, Point2D point1A, Point2D point2A, CircularForm circularFormB, double epsilon)
		{
			Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);

			double distanceFromB = lineA.getDistanceFrom(bodyB.Position);
			double radiusB = circularFormB.Radius;

			if (distanceFromB > radiusB)
			{
				return null;
			}

			double leftA = Math.Min(point1A.X, point2A.X);
            double topA = Math.Min(point1A.Y, point2A.Y);
            double rightA = Math.Max(point1A.X, point2A.X);
            double bottomA = Math.Max(point1A.Y, point2A.Y);

			Point2D projectionOfB = lineA.getProjectionOf(bodyB.Position);

			bool projectionOfBBelongsToA = (projectionOfB.X > leftA - epsilon) && (projectionOfB.X < rightA + epsilon) && (projectionOfB.Y > topA - epsilon) && (projectionOfB.Y < bottomA + epsilon);

			if (projectionOfBBelongsToA)
			{
				Vector2D collisionNormalB;

				if (distanceFromB >= epsilon)
				{
					collisionNormalB = (new Vector2D(bodyB.Position, projectionOfB)).normalize();
				}
				else
				{
					Vector2D unitNormalA = lineA.UnitNormal;
					Vector2D relativeVelocityB = bodyB.Velocity.copy().subtract(bodyA.Velocity);

					if (relativeVelocityB.Length >= epsilon)
					{
						collisionNormalB = relativeVelocityB.dotProduct(unitNormalA) >= epsilon ? unitNormalA : unitNormalA.negate();
					}
					else if (bodyB.Velocity.Length >= epsilon)
					{
						collisionNormalB = bodyB.Velocity.dotProduct(unitNormalA) >= epsilon ? unitNormalA : unitNormalA.negate();
					}
					else
					{
						collisionNormalB = unitNormalA;
					}
				}

				return new CollisionInfo(bodyA, bodyB, projectionOfB, collisionNormalB, radiusB - distanceFromB, epsilon);
			}

			double distanceToPoint1A = bodyB.getDistanceTo(point1A);
			double distanceToPoint2A = bodyB.getDistanceTo(point2A);

			Point2D nearestPointA;
			double distanceToNearestPointA;

			if (distanceToPoint1A < distanceToPoint2A)
			{
				nearestPointA = point1A;
				distanceToNearestPointA = distanceToPoint1A;
			}
			else
			{
				nearestPointA = point2A;
				distanceToNearestPointA = distanceToPoint2A;
			}

			if (distanceToNearestPointA > radiusB)
			{
				return null;
			}

			return new CollisionInfo(bodyA, bodyB, nearestPointA, (new Vector2D(bodyB.Position, nearestPointA)).normalize(), radiusB - distanceToNearestPointA, epsilon);
		}
	}

}