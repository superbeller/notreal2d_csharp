using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    //using StringUtil = com.codeforces.commons.text.StringUtil;
    //using Contract = org.jetbrains.annotations.Contract;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 26.08.2015
	/// </summary>
    public class NamedEntry
	{
		public readonly string name;

        public NamedEntry(string name)
		{
			this.name = name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}

			if (o == null || this.GetType() != o.GetType())
			{
				return false;
			}

			NamedEntry namedEntry = (NamedEntry) o;

			return name.Equals(namedEntry.name);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract("null -> fail") static void validateName(@Nonnull String name)
		public static void validateName(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new System.ArgumentException("Argument 'name' is blank.");
			}

            if (String.IsNullOrWhiteSpace(name))
			{
				throw new System.ArgumentException("Argument 'name' should not contain neither leading nor trailing whitespace characters.");
			}
		}
	}

}