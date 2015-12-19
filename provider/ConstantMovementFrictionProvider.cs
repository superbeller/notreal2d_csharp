namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 04.06.2015
	/// </summary>
	public class ConstantMovementFrictionProvider : MovementFrictionProvider
	{
		private readonly double movementFrictionFactor;

		public ConstantMovementFrictionProvider(double movementFrictionFactor)
		{
			if (movementFrictionFactor < 0.0D)
			{
				throw new System.ArgumentException("Argument 'movementFrictionFactor' should be zero or positive.");
			}

			this.movementFrictionFactor = movementFrictionFactor;
		}

		public virtual double MovementFrictionFactor
		{
			get
			{
				return movementFrictionFactor;
			}
		}

		public virtual void applyFriction(Body body, double updateFactor)
		{
			if (movementFrictionFactor <= 0.0D)
			{
				return;
			}

			double velocityLength = body.Velocity.Length;
			if (velocityLength <= 0.0D)
			{
				return;
			}

			double velocityChange = movementFrictionFactor * updateFactor;

			if (velocityChange >= velocityLength)
			{
				body.setVelocity(0.0D, 0.0D);
			}
			else if (velocityChange > 0.0D)
			{
				body.Velocity.multiply(1.0D - velocityChange / velocityLength);
			}
		}
	}

}