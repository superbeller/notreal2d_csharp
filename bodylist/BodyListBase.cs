using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	//using Contract = org.jetbrains.annotations.Contract;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.08.2015
	/// </summary>
	public abstract class BodyListBase : BodyList
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract java.util.List<com.codegame.codeseries.notreal2d.Body> getPotentialIntersections(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		public abstract IList<Body> getPotentialIntersections(Body body);
		public abstract IList<Body> Bodies {get;}
		public abstract Body getBody(long id);
		public abstract bool hasBody(long id);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract boolean hasBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		public abstract bool hasBody(Body body);
		public abstract void removeBodyQuietly(long id);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void removeBodyQuietly(@Nullable com.codegame.codeseries.notreal2d.Body body);
		public abstract void removeBodyQuietly(Body body);
		public abstract void removeBody(long id);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void removeBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		public abstract void removeBody(Body body);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void addBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		public abstract void addBody(Body body);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract("null -> fail") protected static void validateBody(com.codegame.codeseries.notreal2d.Body body)
		protected internal static void validateBody(Body body)
		{
			if (body == null)
			{
				throw new System.ArgumentException("Argument 'body' is null.");
			}
		}
	}

}