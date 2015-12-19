namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Shape = com.codegame.codeseries.notreal2d.form.Shape;


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.06.2015
	/// </summary>
	public class ArcAndArcCollider : ColliderBase
	{
		public ArcAndArcCollider(double epsilon) : base(epsilon)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean matchesOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override bool matchesOneWay(Body bodyA, Body bodyB)
		{
			return bodyA.Form.Shape == Shape.ARC && bodyB.Form.Shape == Shape.ARC;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override protected CollisionInfo collideOneWay(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB)
		protected override CollisionInfo collideOneWay(Body bodyA, Body bodyB)
		{
			return null; // TODO
		}
	}

}