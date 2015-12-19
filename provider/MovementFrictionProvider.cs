namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 04.06.2015
	/// </summary>
	public interface MovementFrictionProvider
	{
		void applyFriction(Body body, double updateFactor);
	}

}