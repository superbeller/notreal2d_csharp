namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using Form = com.codegame.codeseries.notreal2d.form.Form;
    //using RectangularForm = com.codegame.codeseries.notreal2d.form.RectangularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 19.06.2015
	/// </summary>
	public class RectangleAndRectangleCollider : ColliderBase
	{
		public RectangleAndRectangleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.RECTANGLE && bodyB.Form.Shape == Shape.RECTANGLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			RectangularForm rectangularFormA = (RectangularForm) bodyA.Form;
			RectangularForm rectangularFormB = (RectangularForm) bodyB.Form;

			Point2D[] pointsA = rectangularFormA.getPoints(bodyA.Position, bodyA.Angle, epsilon);
			Point2D[] pointsB = rectangularFormB.getPoints(bodyB.Position, bodyB.Angle, epsilon);

			CollisionInfo collisionInfoA = collideOneWay(bodyA, bodyB, pointsA, pointsB);
			if (collisionInfoA == null)
			{
				return null;
			}

			CollisionInfo collisionInfoB = collideOneWay(bodyB, bodyA, pointsB, pointsA);
			if (collisionInfoB == null)
			{
				return null;
			}

			if (collisionInfoB.Depth < collisionInfoA.Depth)
			{
				return new CollisionInfo(bodyA, bodyB, collisionInfoB.Point, collisionInfoB.NormalB.negate(), collisionInfoB.Depth, epsilon);
			}
			else
			{
				return collisionInfoA;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("OverlyLongMethod") @Nullable private CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB, @Nonnull com.codeforces.commons.geometry.Point2D[] pointsA, @Nonnull com.codeforces.commons.geometry.Point2D[] pointsB)
		private CollisionInfo collideOneWay(Body bodyA, Body bodyB, Point2D[] pointsA, Point2D[] pointsB)
		{
			int pointACount = pointsA.Length;
			int pointBCount = pointsB.Length;

			double minDepth = double.PositiveInfinity;
			Point2D bestIntersectionPoint = null;
			Vector2D bestCollisionNormalB = null;

			for (int pointAIndex = 0; pointAIndex < pointACount; ++pointAIndex)
			{
				Point2D point1A = pointsA[pointAIndex];
				Point2D point2A = pointsA[pointAIndex == pointACount - 1 ? 0 : pointAIndex + 1];

				Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);

				if (lineA.getSignedDistanceFrom(bodyA.Position) > -epsilon)
				{
                    throw new System.InvalidOperationException(string.Format("{0} of {1} is too small, " + "does not represent a convex polygon, or its points are going in wrong order.", bodyA.Form.ToString(), bodyA));
				}

				double minDistanceFromB = double.PositiveInfinity;
				Point2D intersectionPoint = null;
				Vector2D collisionNormalB = null;

				for (int pointBIndex = 0; pointBIndex < pointBCount; ++pointBIndex)
				{
					Point2D pointB = pointsB[pointBIndex];
					double distanceFromPointB = lineA.getSignedDistanceFrom(pointB);

					if (distanceFromPointB < minDistanceFromB)
					{
						minDistanceFromB = distanceFromPointB;
						intersectionPoint = pointB;
						collisionNormalB = lineA.getUnitNormalFrom(bodyA.Position, epsilon).negate();
					}
				}

				if (minDistanceFromB > 0.0D)
				{
					return null;
				}

				double depth = -minDistanceFromB;
				if (depth < minDepth)
				{
					minDepth = depth;
					bestIntersectionPoint = intersectionPoint;
					bestCollisionNormalB = collisionNormalB;
				}
			}

			if (bestIntersectionPoint == null || bestCollisionNormalB == null)
			{
				return null;
			}

			return new CollisionInfo(bodyA, bodyB, bestIntersectionPoint, bestCollisionNormalB, minDepth, epsilon);
		}
	}

}