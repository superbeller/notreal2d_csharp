namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.08.2015
	/// </summary>
	public class PositionListenerAdapter : PositionListener
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public boolean beforeChangePosition(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition)
		public virtual bool beforeChangePosition(Point2D oldPosition, Point2D newPosition)
		{
			return true;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void afterChangePosition(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition)
		public virtual void afterChangePosition(Point2D oldPosition, Point2D newPosition)
		{
			// No operations.
		}
	}

}