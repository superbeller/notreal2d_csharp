using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.sqr;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @NotThreadSafe public class SimpleBodyList extends BodyListBase
	public class SimpleBodyList : BodyListBase
	{
        private List<Body> bodies = new List<Body>();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void addBody(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override void addBody(Body body)
		{
			validateBody(body);

			if (bodies.Contains(body))
			{
				throw new System.InvalidOperationException(body + " is already added.");
			}

			bodies.Add(body);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void removeBody(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override void removeBody(Body body)
		{
			validateBody(body);

            foreach (Body curBody in bodies)
            {
                if (curBody.Equals(body))
                {
                    bodies.Remove(curBody);

                    return;
                }
            }

			throw new System.InvalidOperationException("Can't find " + body + '.');
		}

		public override void removeBody(long id)
		{
            foreach (Body body in bodies)
            {
                if (body.Id == id)
                {
                    bodies.Remove(body);

                    return;
                }
            }

			throw new System.InvalidOperationException("Can't find Body {id=" + id + "}.");
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void removeBodyQuietly(@Nullable com.codegame.codeseries.notreal2d.Body body)
		public override void removeBodyQuietly(Body body)
		{
			if (body == null)
			{
				return;
			}

            foreach (Body curBody in bodies)
            {
                if (curBody.Equals(body))
                {
                    bodies.Remove(curBody);

                    return;
                }
            }
		}

		public override void removeBodyQuietly(long id)
		{
			foreach (Body body in bodies)
			{
				if (body.Id == id)
				{
                    bodies.Remove(body);
					
					return;
				}
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public boolean hasBody(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override bool hasBody(Body body)
		{
			validateBody(body);

			return bodies.Contains(body);
		}

		public override bool hasBody(long id)
		{
			foreach (Body body in bodies)
			{
				if (body.Id == id)
				{
					return true;
				}
			}

			return false;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable @Override public com.codegame.codeseries.notreal2d.Body getBody(long id)
		public override Body getBody(long id)
		{
			foreach (Body body in bodies)
			{
				if (body.Id == id)
				{
					return body;
				}
			}

			return null;
		}

		public override IList<Body> Bodies
		{
			get
			{
				return bodies;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public List<com.codegame.codeseries.notreal2d.Body> getPotentialIntersections(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override IList<Body> getPotentialIntersections(Body body)
		{
			validateBody(body);

			IList<Body> potentialIntersections = new List<Body>();
			bool exists = false;

			foreach (Body otherBody in bodies)
			{
				if (otherBody.Equals(body))
				{
					exists = true;
					continue;
				}

				if (body.Static && otherBody.Static)
				{
					continue;
				}

				if (Math.Pow(otherBody.Form.CircumcircleRadius + body.Form.CircumcircleRadius, 2) < otherBody.getSquaredDistanceTo(body))
				{
					continue;
				}

				potentialIntersections.Add(otherBody);
			}

			if (!exists)
			{
				throw new System.InvalidOperationException("Can't find " + body + '.');
			}

            return potentialIntersections;
		}
	}

}