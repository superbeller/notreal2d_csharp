
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Line2D = com.codeforces.commons.geometry.Line2D;
    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using CircularForm = com.codegame.codeseries.notreal2d.form.CircularForm;
    //using RectangularForm = com.codegame.codeseries.notreal2d.form.RectangularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;
    //using GeometryUtil = com.codegame.codeseries.notreal2d.util.GeometryUtil;


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 19.06.2015
	/// </summary>
	public class RectangleAndCircleCollider : ColliderBase
	{
		public RectangleAndCircleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.RECTANGLE && bodyB.Form.Shape == Shape.CIRCLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("OverlyLongMethod") @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			RectangularForm rectangularFormA = (RectangularForm) bodyA.Form;
			CircularForm circularFormB = (CircularForm) bodyB.Form;

			Point2D[] pointsA = rectangularFormA.getPoints(bodyA.Position, bodyA.Angle, epsilon);
			int pointACount = pointsA.Length;

			if (!GeometryUtil.isPointOutsideConvexPolygon(bodyB.Position, pointsA, epsilon))
			{
				double minDistanceFromB = double.PositiveInfinity;
				Line2D nearestLineA = null;

				for (int pointAIndex = 0; pointAIndex < pointACount; ++pointAIndex)
				{
					Point2D point1A = pointsA[pointAIndex];
					Point2D point2A = pointsA[pointAIndex == pointACount - 1 ? 0 : pointAIndex + 1];

					Line2D lineA = Line2D.getLineByTwoPoints(point1A, point2A);
					double distanceFromB = lineA.getDistanceFrom(bodyB.Position);

					if (distanceFromB < minDistanceFromB)
					{
						minDistanceFromB = distanceFromB;
						nearestLineA = lineA;
					}
				}

				if (nearestLineA != null)
				{
					return new CollisionInfo(bodyA, bodyB, bodyB.Position, nearestLineA.UnitNormal.negate(), circularFormB.Radius - nearestLineA.getSignedDistanceFrom(bodyB.Position), epsilon);
				}
			}

			CollisionInfo collisionInfo = null;

			for (int pointAIndex = 0; pointAIndex < pointACount; ++pointAIndex)
			{
				Point2D point1A = pointsA[pointAIndex];
				Point2D point2A = pointsA[pointAIndex == pointACount - 1 ? 0 : pointAIndex + 1];

				CollisionInfo lineCollisionInfo = LineAndCircleCollider.collideOneWay(bodyA, bodyB, point1A, point2A, circularFormB, epsilon);

				if (lineCollisionInfo != null && (collisionInfo == null || lineCollisionInfo.Depth > collisionInfo.Depth))
				{
					collisionInfo = lineCollisionInfo;
				}
			}

			return collisionInfo;
		}
	}

}