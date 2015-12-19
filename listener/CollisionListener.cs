namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using CollisionInfo = com.codegame.codeseries.notreal2d.collision.CollisionInfo;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 11.06.2015
	/// </summary>
	public interface CollisionListener
	{
		/// <summary>
		/// Physics engine iterates over all registered collision listeners in some order and invokes this method before
		/// gathering any collision information. Is is not guaranteed at this stage that bodies are really intersect. If any
		/// listener returns {@code false}, it cancels all remaining method calls and the collision itself.
		/// </summary>
		/// <param name="bodyA"> first body to collide </param>
		/// <param name="bodyB"> second body to collide </param>
		/// <returns> {@code true} iff physics engine should continue to collide bodies </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: boolean beforeStartingCollision(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		bool beforeStartingCollision(Body bodyA, Body bodyB);

		/// <summary>
		/// Physics engine iterates over all registered collision listeners in some order and invokes this method before
		/// resolving collision. If any listener returns {@code false}, it cancels all remaining method calls and the
		/// collision itself.
		/// </summary>
		/// <param name="collisionInfo"> collision information </param>
		/// <returns> {@code true} iff physics engine should continue to resolve collision </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: boolean beforeResolvingCollision(@Nonnull com.codegame.codeseries.notreal2d.collision.CollisionInfo collisionInfo);
		bool beforeResolvingCollision(CollisionInfo collisionInfo);

		/// <summary>
		/// Physics engine iterates over all registered collision listeners in some order and invokes this method after
		/// resolving collision.
		/// </summary>
		/// <param name="collisionInfo"> collision information </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: void afterResolvingCollision(@Nonnull com.codegame.codeseries.notreal2d.collision.CollisionInfo collisionInfo);
		void afterResolvingCollision(CollisionInfo collisionInfo);
	}

}