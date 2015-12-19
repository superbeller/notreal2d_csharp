namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 08.06.2015
	/// </summary>
	public interface Collider
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: boolean matches(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		bool matches(Body bodyA, Body bodyB);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable CollisionInfo collide(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		CollisionInfo collide(Body bodyA, Body bodyB);
	}

}