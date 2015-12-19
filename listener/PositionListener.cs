namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.08.2015
	/// </summary>
	public interface PositionListener
	{
		/// <summary>
		/// Physics engine iterates over all registered position listeners in some order and invokes this method before
		/// changing position. If any listener returns {@code false}, it cancels all remaining method calls and the change
		/// itself.
		/// <para>
		/// Any {@code oldPosition} changes in the method will be ignored.
		/// Any {@code newPosition} changes in the method will be saved and used to update associated object after last
		/// iteration (all listeners should return {@code true}).
		/// 
		/// </para>
		/// </summary>
		/// <param name="oldPosition"> current position </param>
		/// <param name="newPosition"> next position </param>
		/// <returns> {@code true} iff physics engine should continue to change position </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: boolean beforeChangePosition(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition);
		bool beforeChangePosition(Point2D oldPosition, Point2D newPosition);

		/// <summary>
		/// Physics engine iterates over all registered position listeners in some order and invokes this method after
		/// changing position.
		/// <para>
		/// Any {@code oldPosition} changes in the method will be ignored.
		/// Any {@code newPosition} changes in the method will be ignored.
		/// 
		/// </para>
		/// </summary>
		/// <param name="oldPosition"> previous position </param>
		/// <param name="newPosition"> current position </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: void afterChangePosition(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition);
		void afterChangePosition(Point2D oldPosition, Point2D newPosition);
	}

}