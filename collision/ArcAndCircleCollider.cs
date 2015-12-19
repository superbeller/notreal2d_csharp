using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using ArcForm = com.codegame.codeseries.notreal2d.form.ArcForm;
    //using CircularForm = com.codegame.codeseries.notreal2d.form.CircularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;
    //using GeometryUtil = com.codegame.codeseries.notreal2d.util.GeometryUtil;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.*;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.06.2015
	/// </summary>
	public class ArcAndCircleCollider : ColliderBase
	{
		public ArcAndCircleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected  override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.ARC && bodyB.Form.Shape == Shape.CIRCLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"OverlyComplexMethod", "OverlyLongMethod"}) @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			ArcForm arcFormA = (ArcForm) bodyA.Form;
			CircularForm circularFormB = (CircularForm) bodyB.Form;

			double radiusA = arcFormA.Radius;
			double radiusB = circularFormB.Radius;
			double distance = bodyA.Position.getDistanceTo(bodyB.Position);

			if (distance > radiusA + radiusB)
			{
				return null;
			}

			if (distance < Math.Abs(radiusA - radiusB))
			{
				return null;
			}

			bodyA.normalizeAngle();
			bodyB.normalizeAngle();

			double startAngleA = bodyA.Angle + arcFormA.Angle;
			double finishAngleA = startAngleA + arcFormA.Sector;

			CollisionInfo endpointCollisionInfo = collideWithEndpoints(bodyA, bodyB, radiusA, radiusB, distance, startAngleA, finishAngleA);

			if (endpointCollisionInfo != null)
			{
				return endpointCollisionInfo;
			}

			if (distance >= epsilon)
			{
				double d = Math.Sqrt((distance + radiusA + radiusB) * (distance + radiusA - radiusB) * (distance - radiusA + radiusB) * (-distance + radiusA + radiusB)) / 4.0D;

				double squaredDistance = bodyA.Position.getSquaredDistanceTo(bodyB.Position);
				double coordinateNumeratorFactor = Math.Pow(radiusA,2) - Math.Pow(radiusB,2);

				double xBase = (bodyA.X + bodyB.X) / 2.0D + (bodyB.X - bodyA.X) * coordinateNumeratorFactor / (2.0D * squaredDistance);
				double yBase = (bodyA.Y + bodyB.Y) / 2.0D + (bodyB.Y - bodyA.Y) * coordinateNumeratorFactor / (2.0D * squaredDistance);

				double xOffset = 2.0D * (bodyA.Y - bodyB.Y) * d / squaredDistance;
				double yOffset = 2.0D * (bodyA.X - bodyB.X) * d / squaredDistance;

				Point2D collisionPoint = new Point2D(xBase, yBase);

                if (Math.Abs(xOffset) < epsilon && Math.Abs(yOffset) < epsilon)
				{
					double intersectionPointAngleA = (new Vector2D(bodyA.Position, collisionPoint)).Angle;
					if (intersectionPointAngleA < startAngleA)
					{
						intersectionPointAngleA += Utils.DOUBLE_PI;
					}

					if (intersectionPointAngleA >= startAngleA && intersectionPointAngleA <= finishAngleA)
					{
						return new CollisionInfo(bodyA, bodyB, collisionPoint, (new Vector2D(bodyB.Position, collisionPoint)).normalize(), radiusB - bodyB.getDistanceTo(collisionPoint), epsilon);
					}
				}
				else
				{
					Point2D intersectionPoint1 = collisionPoint.copy().add(xOffset, -yOffset);
					Point2D intersectionPoint2 = collisionPoint.copy().add(-xOffset, yOffset);

					double intersectionPoint1AngleA = (new Vector2D(bodyA.Position, intersectionPoint1)).Angle;
					if (intersectionPoint1AngleA < startAngleA)
					{
                        intersectionPoint1AngleA += Utils.DOUBLE_PI;
					}

					double intersectionPoint2AngleA = (new Vector2D(bodyA.Position, intersectionPoint2)).Angle;
					if (intersectionPoint2AngleA < startAngleA)
					{
                        intersectionPoint2AngleA += Utils.DOUBLE_PI;
					}

					if (intersectionPoint1AngleA >= startAngleA && intersectionPoint1AngleA <= finishAngleA && intersectionPoint2AngleA >= startAngleA && intersectionPoint2AngleA <= finishAngleA)
					{
						if (distance > radiusA - epsilon)
						{
							return new CollisionInfo(bodyA, bodyB, collisionPoint, (new Vector2D(bodyB.Position, bodyA.Position)).normalize(), radiusA + radiusB - distance, epsilon);
						}
						else
						{
							return new CollisionInfo(bodyA, bodyB, collisionPoint, (new Vector2D(bodyA.Position, bodyB.Position)).normalize(), distance + radiusB - radiusA, epsilon);
						}
					}
				}

				return null;
			}
			else
			{
				return collideSameCenter(bodyB, bodyA, arcFormA, radiusA, startAngleA, finishAngleA, radiusB);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable private CollisionInfo collideWithEndpoints(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB, double radiusA, double radiusB, double distance, double startAngleA, double finishAngleA)
		private CollisionInfo collideWithEndpoints(Body bodyA, Body bodyB, double radiusA, double radiusB, double distance, double startAngleA, double finishAngleA)
		{
			Point2D point1A = bodyA.Position.copy().add((new Vector2D(radiusA, 0.0D)).setAngle(startAngleA));
			Point2D point2A = bodyA.Position.copy().add((new Vector2D(radiusA, 0.0D)).setAngle(finishAngleA));

			double distanceToPoint1A = bodyB.getDistanceTo(point1A);
			double distanceToPoint2A = bodyB.getDistanceTo(point2A);

			if (distanceToPoint1A <= radiusB && distanceToPoint2A <= radiusB)
			{
				Point2D collisionPoint = new Point2D((point1A.X + point2A.X) / 2.0D, (point1A.Y + point2A.Y) / 2.0D);

				Vector2D collisionNormalB;
				Line2D normalLineB;

				if (bodyB.getDistanceTo(collisionPoint) >= epsilon)
				{
					collisionNormalB = (new Vector2D(bodyB.Position, collisionPoint)).normalize();
					normalLineB = Line2D.getLineByTwoPoints(bodyB.Position, collisionPoint);
				}
				else
				{
					collisionNormalB = (new Vector2D(bodyB.Position, bodyA.Position)).normalize();
					normalLineB = Line2D.getLineByTwoPoints(bodyB.Position, bodyA.Position);
				}

				Point2D projectionOfPoint1A = normalLineB.getProjectionOf(point1A, epsilon);
				double distanceFromPoint1A = normalLineB.getDistanceFrom(point1A);
                double depth1 = Math.Sqrt(Math.Pow(radiusB, 2) - Math.Pow(distanceFromPoint1A, 2)) - bodyB.getDistanceTo(projectionOfPoint1A);

				Point2D projectionOfPoint2A = normalLineB.getProjectionOf(point2A, epsilon);
				double distanceFromPoint2A = normalLineB.getDistanceFrom(point2A);
                double depth2 = Math.Sqrt(Math.Pow(radiusB, 2) - Math.Pow(distanceFromPoint2A, 2)) - bodyB.getDistanceTo(projectionOfPoint2A);

				return new CollisionInfo(bodyA, bodyB, collisionPoint, collisionNormalB, Math.Max(depth1, depth2), epsilon);
			}

			if (distanceToPoint1A <= radiusB)
			{
				if (distanceToPoint1A >= epsilon)
				{
					return new CollisionInfo(bodyA, bodyB, point1A, (new Vector2D(bodyB.Position, point1A)).normalize(), radiusB - distanceToPoint1A, epsilon);
				}
				else
				{
					return new CollisionInfo(bodyA, bodyB, point1A, (new Vector2D(bodyB.Position, bodyA.Position)).normalize(), radiusA + radiusB - distance, epsilon);
				}
			}

			if (distanceToPoint2A <= radiusB)
			{
				if (distanceToPoint2A >= epsilon)
				{
					return new CollisionInfo(bodyA, bodyB, point2A, (new Vector2D(bodyB.Position, point2A)).normalize(), radiusB - distanceToPoint2A, epsilon);
				}
				else
				{
					return new CollisionInfo(bodyA, bodyB, point2A, (new Vector2D(bodyB.Position, bodyA.Position)).normalize(), radiusA + radiusB - distance, epsilon);
				}
			}

			return null;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable private CollisionInfo collideSameCenter(@Nonnull com.codegame.codeseries.notreal2d.Body bodyB, @Nonnull com.codegame.codeseries.notreal2d.Body bodyA, com.codegame.codeseries.notreal2d.form.ArcForm arcFormA, double radiusA, double startAngleA, double finishAngleA, double radiusB)
		private CollisionInfo collideSameCenter(Body bodyB, Body bodyA, ArcForm arcFormA, double radiusA, double startAngleA, double finishAngleA, double radiusB)
		{
			if (radiusB >= radiusA)
			{
				Vector2D relativeVelocityB = bodyB.Velocity.copy().subtract(bodyA.Velocity);
				Vector2D collisionNormalB;

				if (relativeVelocityB.Length >= epsilon && GeometryUtil.isAngleBetween(relativeVelocityB.Angle, startAngleA, finishAngleA))
				{
					collisionNormalB = relativeVelocityB.normalize();
				}
				else if (bodyB.Velocity.Length >= epsilon && GeometryUtil.isAngleBetween(bodyB.Velocity.Angle, startAngleA, finishAngleA))
				{
					collisionNormalB = bodyB.Velocity.copy().normalize();
				}
				else
				{
					collisionNormalB = (new Vector2D(1.0D, 0.0D)).setAngle(bodyA.Angle + arcFormA.Angle + arcFormA.Sector / 2.0D);
				}

				return new CollisionInfo(bodyA, bodyB, bodyB.Position.copy(), collisionNormalB, radiusB - radiusA, epsilon);
			}
			else
			{
				return null;
			}
		}
	}

}