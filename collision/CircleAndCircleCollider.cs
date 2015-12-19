namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using CircularForm = com.codegame.codeseries.notreal2d.form.CircularForm;
    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 08.06.2015
	/// </summary>
	public class CircleAndCircleCollider : ColliderBase
	{
		public CircleAndCircleCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.CIRCLE && bodyB.Form.Shape == Shape.CIRCLE;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			CircularForm circularFormA = (CircularForm) bodyA.Form;
			CircularForm circularFormB = (CircularForm) bodyB.Form;

			double radiusA = circularFormA.Radius;
			double radiusB = circularFormB.Radius;
			double distance = bodyA.Position.getDistanceTo(bodyB.Position);

			if (distance > radiusA + radiusB)
			{
				return null;
			}

			Vector2D collisionNormalB;
			Point2D collisionPoint;

			if (distance >= epsilon)
			{
				Vector2D vectorBA = new Vector2D(bodyB.Position, bodyA.Position);
				collisionNormalB = vectorBA.copy().normalize();
				collisionPoint = bodyB.Position.copy().add(vectorBA.copy().multiply(radiusB / (radiusA + radiusB)));
			}
			else
			{
				Vector2D relativeVelocityB = bodyB.Velocity.copy().subtract(bodyA.Velocity);

				if (relativeVelocityB.Length >= epsilon)
				{
					collisionNormalB = relativeVelocityB.normalize();
				}
				else if (bodyB.Velocity.Length >= epsilon)
				{
					collisionNormalB = bodyB.Velocity.copy().normalize();
				}
				else
				{
					collisionNormalB = new Vector2D(1.0D, 0.0D);
				}

				collisionPoint = bodyB.Position.copy();
			}

			return new CollisionInfo(bodyA, bodyB, collisionPoint, collisionNormalB, radiusA + radiusB - distance, epsilon);
		}
	}

}