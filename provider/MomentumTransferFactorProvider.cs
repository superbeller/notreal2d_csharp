namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 28.08.2015
	/// </summary>
	public interface MomentumTransferFactorProvider
	{
		/// <summary>
		/// Calculates and returns momentum transfer factor used to resolve collision of two bodies. {@code null} result
		/// means that physics engine should use a product of
		/// <seealso cref="Body#getMomentumTransferFactor() momentum transfer factors"/> of two bodies.
		/// <para>
		/// A momentum transfer factor should be between 0.0 and 1.0 both inclusive.
		/// 
		/// </para>
		/// </summary>
		/// <param name="bodyA"> first body to get factor </param>
		/// <param name="bodyB"> second body to get factor </param>
		/// <returns> momentum transfer factor of two bodies or {@code null} to use default strategy </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable Double getFactor(@Nonnull com.codegame.codeseries.notreal2d.Body bodyA, @Nonnull com.codegame.codeseries.notreal2d.Body bodyB);
		double? getFactor(Body bodyA, Body bodyB);
	}

}