namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 01.07.2015
	/// </summary>
	public abstract class ThinForm : Form
	{
		private readonly bool endpointCollisionEnabled;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: protected ThinForm(@Nonnull Shape shape, boolean endpointCollisionEnabled)
		protected internal ThinForm(Shape shape, bool endpointCollisionEnabled) : base(shape)
		{

			this.endpointCollisionEnabled = endpointCollisionEnabled;
		}

		public virtual bool EndpointCollisionEnabled
		{
			get
			{
				return endpointCollisionEnabled;
			}
		}
	}

}