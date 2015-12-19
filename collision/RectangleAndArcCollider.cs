using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using Mutable = com.codeforces.commons.holder.Mutable;
    //using SimpleMutable = com.codeforces.commons.holder.SimpleMutable;
    //using SimplePair = com.codeforces.commons.pair.SimplePair;
    //using ArcForm = com.codegame.codeseries.notreal2d.form.ArcForm;
    //using Form = com.codegame.codeseries.notreal2d.form.Form;
    //using RectangularForm = com.codegame.codeseries.notreal2d.form.RectangularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;
    //using GeometryUtil = com.codegame.codeseries.notreal2d.util.GeometryUtil;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.*;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.06.2015
	/// </summary>
	public class RectangleAndArcCollider : ColliderBase
	{
		public RectangleAndArcCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.RECTANGLE && bodyB.Form.Shape == Shape.ARC;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"OverlyComplexMethod", "OverlyLongMethod", "ConstantConditions"}) @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			RectangularForm rectangularFormA = (RectangularForm) bodyA.Form;
			ArcForm arcFormB = (ArcForm) bodyB.Form;

			double radiusA = rectangularFormA.CircumcircleRadius;
			double radiusB = arcFormB.Radius;
			double distance = bodyA.Position.getDistanceTo(bodyB.Position);

			if (distance > radiusA + radiusB)
			{
				return null;
			}

			if (distance < Math.Abs(radiusA - radiusB))
			{
				return null;
			}

			Point2D[] pointsA = rectangularFormA.getPoints(bodyA.Position, bodyA.Angle, epsilon);
			int pointACount = pointsA.Length;

			double squaredRadiusB = radiusB * radiusB;

			double startAngleB = bodyB.Angle + arcFormB.Angle;
			double finishAngleB = startAngleB + arcFormB.Sector;

			Point2D point1B = bodyB.Position.copy().add((new Vector2D(radiusB, 0.0D)).setAngle(startAngleB));
			Point2D point2B = bodyB.Position.copy().add((new Vector2D(radiusB, 0.0D)).setAngle(finishAngleB));

			IList<IntersectionInfo> intersectionInfos = new List<IntersectionInfo>();

			for (int pointAIndex = 0; pointAIndex < pointACount; ++pointAIndex)
			{
				Point2D point1A = pointsA[pointAIndex];
				Point2D point2A = pointsA[pointAIndex == pointACount - 1 ? 0 : pointAIndex + 1];

				Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);

				if (lineA.getSignedDistanceFrom(bodyA.Position) > -epsilon)
				{
                    throw new System.InvalidOperationException(string.Format("{0} of {1} is too small, " + "does not represent a convex polygon, or its points are going in wrong order.", bodyA.Form.ToString(), bodyA));
				}

				double distanceFromB = lineA.getSignedDistanceFrom(bodyB.Position);

				if (distanceFromB > radiusB)
				{
					continue;
				}

				double leftA = Math.Min(point1A.X, point2A.X);
                double topA = Math.Min(point1A.Y, point2A.Y);
                double rightA = Math.Max(point1A.X, point2A.X);
                double bottomA = Math.Max(point1A.Y, point2A.Y);

				Point2D projectionOfB = lineA.getProjectionOf(bodyB.Position);

				double offset = Math.Sqrt(squaredRadiusB - distanceFromB * distanceFromB);
				Vector2D offsetVector = (new Vector2D(point1A, point2A)).copy().setLength(offset);

				Point2D intersectionPoint1 = projectionOfB.copy().add(offsetVector);

				if (doesPointBelongToAAndB(intersectionPoint1, leftA, topA, rightA, bottomA, bodyB, startAngleB, finishAngleB))
				{
					addIntersectionInfo(intersectionPoint1, point1A, point2A, lineA, intersectionInfos);
				}

				Point2D intersectionPoint2 = projectionOfB.copy().add(offsetVector.copy().negate());

				if (doesPointBelongToAAndB(intersectionPoint2, leftA, topA, rightA, bottomA, bodyB, startAngleB, finishAngleB))
				{
					addIntersectionInfo(intersectionPoint2, point1A, point2A, lineA, intersectionInfos);
				}
			}

			int intersectionCount = intersectionInfos.Count;

			if (intersectionCount == 0)
			{
				// TODO check arc inside rectangle
				return null;
			}
			else if (intersectionCount == 1 && arcFormB.EndpointCollisionEnabled && (!GeometryUtil.isPointOutsideConvexPolygon(point1B, pointsA, epsilon) || !GeometryUtil.isPointOutsideConvexPolygon(point2B, pointsA, epsilon)))
			{
				IntersectionInfo intersectionInfo = intersectionInfos[0];
				int intersectionLineCount = intersectionInfo.intersectionLines.Count;

				if (intersectionLineCount == 1 || intersectionLineCount == 2)
				{ // TODO separate 1 and 2 ??
					Line2D intersectionLine = intersectionInfo.intersectionLines[0];

					double distanceFromPoint1B = intersectionLine.getSignedDistanceFrom(point1B);
					double distanceFromPoint2B = intersectionLine.getSignedDistanceFrom(point2B);

					for (int pointAIndex = 0; pointAIndex < pointACount; ++pointAIndex)
					{
						Point2D point1A = pointsA[pointAIndex];
						Point2D point2A = pointsA[pointAIndex == pointACount - 1 ? 0 : pointAIndex + 1];

						Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);

						if (lineA.getSignedDistanceFrom(point1B) >= epsilon)
						{
							return new CollisionInfo(bodyA, bodyB, point2B, intersectionLine.UnitNormal.negate(), -distanceFromPoint2B, epsilon);
						}

						if (lineA.getSignedDistanceFrom(point2B) >= epsilon)
						{
							return new CollisionInfo(bodyA, bodyB, point1B, intersectionLine.UnitNormal.negate(), -distanceFromPoint1B, epsilon);
						}
					}

					if (distanceFromPoint1B < distanceFromPoint2B)
					{
						return new CollisionInfo(bodyA, bodyB, point1B, intersectionLine.UnitNormal.negate(), -distanceFromPoint1B, epsilon);
					}
					else
					{
						return new CollisionInfo(bodyA, bodyB, point2B, intersectionLine.UnitNormal.negate(), -distanceFromPoint2B, epsilon);
					}
				}
				else
				{
                    throw new System.InvalidOperationException(string.Format("{0} of {1} is too small, " + "does not represent a convex polygon, or its points are going in wrong order.", bodyA.Form.ToString(), bodyA));
				}
			}
			else
			{
				Vector2D vectorCB = new Vector2D(intersectionInfos[0].intersectionPoint, bodyB.Position);
				Vector2D vectorCA = new Vector2D(intersectionInfos[0].intersectionPoint, bodyA.Position);

				if (distance > radiusB - epsilon && vectorCB.dotProduct(vectorCA) < 0.0D)
				{
					Point2D nearestPoint = new Point2D();
					double? distanceToNearestPoint = new double?();

					foreach (IntersectionInfo intersectionInfo in intersectionInfos)
					{
						updateNearestPoint(bodyB, intersectionInfo.intersectionPoint, nearestPoint, distanceToNearestPoint.Value);

						foreach (SimplePair pointAndPoint in intersectionInfo.intersectionLinePointPairs)
						{
                            updateNearestPoint(bodyB, (Point2D)pointAndPoint.First, nearestPoint, distanceToNearestPoint);
                            updateNearestPoint(bodyB, (Point2D)pointAndPoint.Second, nearestPoint, distanceToNearestPoint);
						}
					}

					return nearestPoint == null ? null : new CollisionInfo(bodyA, bodyB, nearestPoint, (new Vector2D(bodyB.Position, nearestPoint)).normalize(), radiusB - distanceToNearestPoint.Value, epsilon);
				}
				else
				{
					Point2D farthestPoint = new Point2D();
					double? distanceToFarthestPoint = new double?();

					foreach (IntersectionInfo intersectionInfo in intersectionInfos)
					{
						updateFarthestPoint(bodyB, intersectionInfo.intersectionPoint, farthestPoint, distanceToFarthestPoint, startAngleB, finishAngleB);

						foreach (SimplePair pointAndPoint in intersectionInfo.intersectionLinePointPairs)
						{
                            updateFarthestPoint(bodyB, (Point2D)pointAndPoint.First, farthestPoint, distanceToFarthestPoint, startAngleB, finishAngleB);
                            updateFarthestPoint(bodyB, (Point2D)pointAndPoint.Second, farthestPoint, distanceToFarthestPoint, startAngleB, finishAngleB);
						}
					}

					return farthestPoint == null ? null : new CollisionInfo(bodyA, bodyB, farthestPoint, (new Vector2D(farthestPoint, bodyB.Position)).normalize(), distanceToFarthestPoint.Value - radiusB, epsilon);
				}
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void updateNearestPoint(@Nonnull com.codegame.codeseries.notreal2d.Body body, @Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.holder.Mutable<com.codeforces.commons.geometry.Point2D> nearestPoint, @Nonnull com.codeforces.commons.holder.Mutable<Double> distanceToNearestPoint)
		private void updateNearestPoint(Body body, Point2D point, Point2D nearestPoint, double? distanceToNearestPoint)
		{
			double distanceToPoint = body.getDistanceTo(point);

			if (distanceToPoint >= epsilon && (nearestPoint == null || distanceToPoint < distanceToNearestPoint))
			{
				nearestPoint = (point);
				distanceToNearestPoint = distanceToPoint;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private static void updateFarthestPoint(@Nonnull com.codegame.codeseries.notreal2d.Body body, @Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.holder.Mutable<com.codeforces.commons.geometry.Point2D> farthestPoint, @Nonnull com.codeforces.commons.holder.Mutable<Double> distanceToFarthestPoint, double startAngle, double finishAngle)
		private static void updateFarthestPoint(Body body, Point2D point, Point2D farthestPoint, double? distanceToFarthestPoint, double startAngle, double finishAngle)
		{
			double distanceToPoint = body.getDistanceTo(point);

			if (GeometryUtil.isAngleBetween((new Vector2D(body.Position, point)).Angle, startAngle, finishAngle) && (farthestPoint == null || distanceToPoint > distanceToFarthestPoint))
			{
				farthestPoint = point;
				distanceToFarthestPoint = distanceToPoint;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private boolean doesPointBelongToAAndB(@Nonnull com.codeforces.commons.geometry.Point2D point, double leftA, double topA, double rightA, double bottomA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB, double startAngleB, double finishAngleB)
		private bool doesPointBelongToAAndB(Point2D point, double leftA, double topA, double rightA, double bottomA, Body bodyB, double startAngleB, double finishAngleB)
		{
			bool belongsToA = (point.X > leftA - epsilon) && (point.X < rightA + epsilon) && (point.Y > topA - epsilon) && (point.Y < bottomA + epsilon);

			double pointAngleB = (new Vector2D(bodyB.Position, point)).Angle;
			if (pointAngleB < startAngleB)
			{
				pointAngleB += Utils.DOUBLE_PI;
			}

			bool belongsToB = pointAngleB >= startAngleB && pointAngleB <= finishAngleB;

			return belongsToA && belongsToB;
		}

		private void addIntersectionInfo(Point2D point, Point2D point1A, Point2D point2A, Line2D lineA, IList<IntersectionInfo> intersectionInfos)
		{
			bool alreadyAdded = false;

			foreach (IntersectionInfo intersectionInfo in intersectionInfos)
			{
				if (intersectionInfo.intersectionPoint.nearlyEquals(point, epsilon))
				{
					intersectionInfo.intersectionLines.Add(lineA);
					intersectionInfo.intersectionLinePointPairs.Add(new SimplePair(point1A, point2A));
					alreadyAdded = true;
					break;
				}
			}

			if (!alreadyAdded)
			{
				IntersectionInfo intersectionInfo = new IntersectionInfo(point);
				intersectionInfo.intersectionLines.Add(lineA);
				intersectionInfo.intersectionLinePointPairs.Add(new SimplePair(point1A, point2A));
				intersectionInfos.Add(intersectionInfo);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("PublicField") private static final class IntersectionInfo
		private sealed class IntersectionInfo
		{
			public readonly Point2D intersectionPoint;
			public readonly IList<Line2D> intersectionLines = new List<Line2D>();
			public readonly IList<SimplePair> intersectionLinePointPairs = new List<SimplePair>();

			internal IntersectionInfo(Point2D intersectionPoint)
			{
				this.intersectionPoint = intersectionPoint;
			}
		}
	}

}