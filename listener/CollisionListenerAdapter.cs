namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using CollisionInfo = com.codegame.codeseries.notreal2d.collision.CollisionInfo;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 11.06.2015
	/// </summary>
	public class CollisionListenerAdapter : CollisionListener
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public boolean beforeStartingCollision(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		public virtual bool beforeStartingCollision(Body bodyA, Body bodyB)
		{
			return true;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public boolean beforeResolvingCollision(@Nonnull com.codegame.codeseries.notreal2d.collision.CollisionInfo collisionInfo)
		public virtual bool beforeResolvingCollision(CollisionInfo collisionInfo)
		{
			return true;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void afterResolvingCollision(@Nonnull com.codegame.codeseries.notreal2d.collision.CollisionInfo collisionInfo)
		public virtual void afterResolvingCollision(CollisionInfo collisionInfo)
		{
			// No operations.
		}
	}

}