using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{


	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public interface BodyList
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: void addBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		void addBody(Body body);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: void removeBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		void removeBody(Body body);
		void removeBody(long id);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: void removeBodyQuietly(@Nullable com.codegame.codeseries.notreal2d.Body body);
		void removeBodyQuietly(Body body);
		void removeBodyQuietly(long id);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: boolean hasBody(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		bool hasBody(Body body);
		bool hasBody(long id);

		Body getBody(long id);
		IList<Body> Bodies {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: java.util.List<com.codegame.codeseries.notreal2d.Body> getPotentialIntersections(@Nonnull com.codegame.codeseries.notreal2d.Body body);
		IList<Body> getPotentialIntersections(Body body);
	}

}