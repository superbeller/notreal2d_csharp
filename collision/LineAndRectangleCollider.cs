
using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using LinearForm = com.codegame.codeseries.notreal2d.form.LinearForm;
    //using RectangularForm = com.codegame.codeseries.notreal2d.form.RectangularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;
    //using GeometryUtil = com.codegame.codeseries.notreal2d.util.GeometryUtil;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.*;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 19.06.2015
	/// </summary>
	public class LineAndRectangleCollider : ColliderBase
	{
		public LineAndRectangleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.LINE && bodyB.Form.Shape == Shape.RECTANGLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"OverlyComplexMethod", "OverlyLongMethod"}) @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			LinearForm linearFormA = (LinearForm) bodyA.Form;
			RectangularForm rectangularFormB = (RectangularForm) bodyB.Form;

			Point2D point1A = linearFormA.getPoint1(bodyA.Position, bodyA.Angle, epsilon);
			Point2D point2A = linearFormA.getPoint2(bodyA.Position, bodyA.Angle, epsilon);

			Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);

			if (lineA.getDistanceFrom(bodyB.Position) > rectangularFormB.CircumcircleRadius)
			{
				return null;
			}

			Point2D[] pointsB = rectangularFormB.getPoints(bodyB.Position, bodyB.Angle, epsilon);
			int pointBCount = pointsB.Length;

			Line2D intersectionLineB = null;
			IList<Point2D> intersectionPoints = new List<Point2D>(pointBCount);
			int intersectionCount = 0;

			for (int pointBIndex = 0; pointBIndex < pointBCount; ++pointBIndex)
			{
				Point2D point1B = pointsB[pointBIndex];
				Point2D point2B = pointsB[pointBIndex == pointBCount - 1 ? 0 : pointBIndex + 1];

				Line2D lineB = Line2D.getLineByTwoPoints(point1B, point2B);

				Point2D potentialIntersectionPoint = lineA.getIntersectionPoint(lineB, epsilon);
				if (potentialIntersectionPoint == null)
				{
					continue;
				}

                double left = Math.Max(Math.Min(point1A.X, point2A.X), Math.Min(point1B.X, point2B.X));
                double top = Math.Max(Math.Min(point1A.Y, point2A.Y), Math.Min(point1B.Y, point2B.Y));
                double right = Math.Min(Math.Max(point1A.X, point2A.X), Math.Max(point1B.X, point2B.X));
                double bottom = Math.Min(Math.Max(point1A.Y, point2A.Y), Math.Max(point1B.Y, point2B.Y));

				if (potentialIntersectionPoint.X <= left - epsilon || potentialIntersectionPoint.X >= right + epsilon || potentialIntersectionPoint.Y <= top - epsilon || potentialIntersectionPoint.Y >= bottom + epsilon)
				{
					continue;
				}

				intersectionLineB = lineB;

				bool alreadyAdded = false;

				foreach (Point2D intersectionPoint in intersectionPoints)
				{
					if (intersectionPoint.nearlyEquals(potentialIntersectionPoint, epsilon))
					{
						alreadyAdded = true;
						break;
					}
				}

				if (!alreadyAdded)
				{
					intersectionPoints.Add(potentialIntersectionPoint);
				}

				++intersectionCount;
			}

			if (intersectionCount == 1 && linearFormA.EndpointCollisionEnabled && 
                (!GeometryUtil.isPointOutsideConvexPolygon(point1A, pointsB, epsilon) || !GeometryUtil.isPointOutsideConvexPolygon(point2A, pointsB, epsilon)))
			{
				Vector2D collisionNormalB = (new Vector2D(bodyB.Position, intersectionLineB.getProjectionOf(bodyB.Position))).normalize();

				Line2D parallelLine1A = intersectionLineB.getParallelLine(point1A);
				double distance1AFromB = parallelLine1A.getDistanceFrom(bodyB.Position);

				Line2D parallelLine2A = intersectionLineB.getParallelLine(point2A);
				double distance2AFromB = parallelLine2A.getDistanceFrom(bodyB.Position);

				double depth = (distance1AFromB < distance2AFromB ? parallelLine1A : parallelLine2A).getDistanceFrom(intersectionLineB, epsilon);

				return new CollisionInfo(bodyA, bodyB, intersectionPoints[0], collisionNormalB, depth, epsilon);
			}
			else
			{
				Point2D pointBWithMinDistanceFromA = pointsB[0];
				double minDistanceBFromA = lineA.getSignedDistanceFrom(pointBWithMinDistanceFromA);

				Point2D pointBWithMaxDistanceFromA = pointBWithMinDistanceFromA;
				double maxDistanceBFromA = minDistanceBFromA;

				for (int pointBIndex = 1; pointBIndex < pointBCount; ++pointBIndex)
				{
					Point2D pointB = pointsB[pointBIndex];
					double distanceBFromA = lineA.getSignedDistanceFrom(pointB);

					if (distanceBFromA < minDistanceBFromA)
					{
						minDistanceBFromA = distanceBFromA;
						pointBWithMinDistanceFromA = pointB;
					}

					if (distanceBFromA > maxDistanceBFromA)
					{
						maxDistanceBFromA = distanceBFromA;
						pointBWithMaxDistanceFromA = pointB;
					}
				}

				if (minDistanceBFromA < 0.0D && maxDistanceBFromA < 0.0D || minDistanceBFromA > 0.0D && maxDistanceBFromA > 0.0D)
				{
					return null;
				}

				if (intersectionPoints.Count == 0)
				{
					return null; // TODO check line inside rectangle
				}

				Vector2D collisionNormalB;
				double depth;

				if (lineA.getSignedDistanceFrom(bodyB.Position) > 0.0D)
				{
					collisionNormalB = lineA.getParallelLine(pointBWithMinDistanceFromA).getUnitNormalFrom(pointBWithMaxDistanceFromA);
					depth = Math.Abs(minDistanceBFromA);
				}
				else
				{
					collisionNormalB = lineA.getParallelLine(pointBWithMaxDistanceFromA).getUnitNormalFrom(pointBWithMinDistanceFromA);
					depth = maxDistanceBFromA;
				}

				double averageIntersectionX = 0.0D;
				double averageIntersectionY = 0.0D;

				foreach (Point2D intersectionPoint in intersectionPoints)
				{
					averageIntersectionX += intersectionPoint.X / intersectionPoints.Count;
					averageIntersectionY += intersectionPoint.Y / intersectionPoints.Count;
				}

				return new CollisionInfo(bodyA, bodyB, new Point2D(averageIntersectionX, averageIntersectionY), collisionNormalB, depth, epsilon);
			}
		}
	}

}