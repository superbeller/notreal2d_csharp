
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Vector2D = com.codeforces.commons.geometry.Vector2D;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 04.06.2015
	/// </summary>
	public class BidirectionalMovementFrictionProvider : MovementFrictionProvider
	{
		private readonly double lengthwiseMovementFrictionFactor;
		private readonly double crosswiseMovementFrictionFactor;

		public BidirectionalMovementFrictionProvider(double lengthwiseMovementFrictionFactor, double crosswiseMovementFrictionFactor)
		{
			if (lengthwiseMovementFrictionFactor < 0.0D)
			{
				throw new System.ArgumentException("Argument 'lengthwiseMovementFrictionFactor' should be zero or positive.");
			}

			if (crosswiseMovementFrictionFactor < 0.0D)
			{
				throw new System.ArgumentException("Argument 'crosswiseMovementFrictionFactor' should be zero or positive.");
			}

			this.lengthwiseMovementFrictionFactor = lengthwiseMovementFrictionFactor;
			this.crosswiseMovementFrictionFactor = crosswiseMovementFrictionFactor;
		}

		public virtual double LengthwiseMovementFrictionFactor
		{
			get
			{
				return lengthwiseMovementFrictionFactor;
			}
		}

		public virtual double CrosswiseMovementFrictionFactor
		{
			get
			{
				return crosswiseMovementFrictionFactor;
			}
		}

		public virtual void applyFriction(Body body, double updateFactor)
		{
			double velocityLength = body.Velocity.Length;
			if (velocityLength <= 0.0D)
			{
				return;
			}

			double lengthwiseVelocityChange = lengthwiseMovementFrictionFactor * updateFactor;
			double crosswiseVelocityChange = crosswiseMovementFrictionFactor * updateFactor;

			Vector2D lengthwiseUnitVector = (new Vector2D(1.0D, 0.0D)).rotate(body.Angle);
			Vector2D crosswiseUnitVector = (new Vector2D(0.0D, 1.0D)).rotate(body.Angle);

			double lengthwiseVelocityPart = body.Velocity.dotProduct(lengthwiseUnitVector);

			if (lengthwiseVelocityPart >= 0.0D)
			{
				lengthwiseVelocityPart -= lengthwiseVelocityChange;
				if (lengthwiseVelocityPart < 0.0D)
				{
					lengthwiseVelocityPart = 0.0D;
				}
			}
			else
			{
				lengthwiseVelocityPart += lengthwiseVelocityChange;
				if (lengthwiseVelocityPart > 0.0D)
				{
					lengthwiseVelocityPart = 0.0D;
				}
			}

			double crosswiseVelocityPart = body.Velocity.dotProduct(crosswiseUnitVector);

			if (crosswiseVelocityPart >= 0.0D)
			{
				crosswiseVelocityPart -= crosswiseVelocityChange;
				if (crosswiseVelocityPart < 0.0D)
				{
					crosswiseVelocityPart = 0.0D;
				}
			}
			else
			{
				crosswiseVelocityPart += crosswiseVelocityChange;
				if (crosswiseVelocityPart > 0.0D)
				{
					crosswiseVelocityPart = 0.0D;
				}
			}

			body.Velocity = lengthwiseUnitVector.copy().multiply(lengthwiseVelocityPart).add(crosswiseUnitVector.copy().multiply(crosswiseVelocityPart));
		}
	}

}