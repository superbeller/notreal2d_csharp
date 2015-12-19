
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public class DynamicState : StaticState
	{
		private Vector2D velocity;
		private Vector2D medianVelocity;
		private Vector2D force;

		private double angularVelocity;
		private double medianAngularVelocity;
		private double torque;

		public DynamicState()
		{
			this.velocity = new Vector2D(0.0D, 0.0D);
			this.medianVelocity = new Vector2D(0.0D, 0.0D);
			this.force = new Vector2D(0.0D, 0.0D);
		}

		public DynamicState(Point2D position, Vector2D velocity, Vector2D force, double angle, double angularVelocity, double torque) : base(position, angle)
		{

			this.velocity = velocity.copy();
			this.force = force.copy();
			this.angularVelocity = angularVelocity;
			this.torque = torque;
		}

		public DynamicState(DynamicState state) : base(state)
		{

			this.velocity = state.velocity.copy();
			this.force = state.force.copy();
			this.angularVelocity = state.angularVelocity;
			this.torque = state.torque;
		}

		public virtual Vector2D Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				this.velocity = value.copy();
			}
		}


		public virtual Vector2D MedianVelocity
		{
			get
			{
				return medianVelocity;
			}
			set
			{
				this.medianVelocity = value;
			}
		}


		public virtual Vector2D Force
		{
			get
			{
				return force;
			}
			set
			{
				this.force = value.copy();
			}
		}


		public virtual double AngularVelocity
		{
			get
			{
				return angularVelocity;
			}
			set
			{
				this.angularVelocity = value;
			}
		}


		public virtual double MedianAngularVelocity
		{
			get
			{
				return medianAngularVelocity;
			}
			set
			{
				this.medianAngularVelocity = value;
			}
		}


		public virtual double Torque
		{
			get
			{
				return torque;
			}
			set
			{
				this.torque = value;
			}
		}

	}

}