namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	//using Form = com.codegame.codeseries.notreal2d.form.Form;


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.07.2015
	/// </summary>
	public abstract class ColliderBase : Collider
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("ProtectedField") protected final double epsilon;
		protected readonly double epsilon;

		protected ColliderBase(double epsilon)
		{
			this.epsilon = epsilon;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public final boolean matches(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		public bool matches(Body bodyA, Body bodyB)
		{
			return matchesOneWay(bodyA, bodyB) || matchesOneWay(bodyB, bodyA);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override public final CollisionInfo collide(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		public CollisionInfo collide(Body bodyA, Body bodyB)
		{
			if (matchesOneWay(bodyA, bodyB))
			{
				return collideOneWay(bodyA, bodyB);
			}

			if (matchesOneWay(bodyB, bodyA))
			{
				CollisionInfo collisionInfo = collideOneWay(bodyB, bodyA);
				return collisionInfo == null ? null : new CollisionInfo(bodyA, bodyB, collisionInfo.Point, collisionInfo.NormalB.negate(), collisionInfo.Depth, epsilon);
			}

            throw new System.ArgumentException(string.Format("Unsupported {0} of {1} or {2} of {3}.", bodyA.Form.ToString(), bodyA, bodyB.Form.ToString(), bodyB));
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: protected abstract boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		protected abstract bool matchesOneWay(Body bodyA, Body bodyB);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable protected abstract CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		protected abstract CollisionInfo collideOneWay(Body bodyA, Body bodyB);
	}

}