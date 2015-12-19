using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;
    //using Form = com.codegame.codeseries.notreal2d.form.Form;
    //using ConstantMovementFrictionProvider = com.codegame.codeseries.notreal2d.provider.ConstantMovementFrictionProvider;
    //using MovementFrictionProvider = com.codegame.codeseries.notreal2d.provider.MovementFrictionProvider;
    //using Contract = org.jetbrains.annotations.Contract;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.pow;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public class Body
	{
		//private static readonly AtomicLong idGenerator = new AtomicLong();

		/// <summary>
		/// Unique ID.
		/// </summary>
        private readonly long id = Utils.IncrementAndGet();

		/// <summary>
		/// The name of this body.
		/// </summary>
		private string name;

		/// <summary>
		/// The form (shape and size) of this body.
		/// </summary>
		private Form form;

		/// <summary>
		/// The mass of this body.
		/// </summary>
		private double mass;

		/// <summary>
		/// The inverted mass of this body. Used to speed up some calculations.
		/// </summary>
		private double invertedMass;

		/// <summary>
		/// The relative loss of the speed per time unit. Should be in range [0, 1].
		/// </summary>
		private double movementAirFrictionFactor;

		/// <summary>
		/// The relative loss of the angular speed per time unit. Should be in range [0, 1].
		/// </summary>
		private double rotationAirFrictionFactor;

		/// <summary>
		/// The provider of the absolute loss of the speed.
		/// </summary>
		private MovementFrictionProvider movementFrictionProvider = new ConstantMovementFrictionProvider(0.0D);

		/// <summary>
		/// The absolute loss of the angular speed per time unit.
		/// </summary>
		private double rotationFrictionFactor;

		/// <summary>
		/// The momentum transfer factor of this body. Should be in range [0, 1].
		/// <p/>
		/// If two bodies collide, the resulting momentum transfer can be calculated as the product of their momentum
		/// transfer factors. This is a default behaviour.
		/// <p/>
		/// However it can be <seealso cref="World#momentumTransferFactorProvider overridden"/>.
		/// </summary>
		private double momentumTransferFactor = 1.0D;

		/// <summary>
		/// The surface friction factor of this body. Should be in range [0, 1].
		/// <p/>
		/// If two bodies collide, the resulting surface friction is proportional to the square root of the product of their
		/// surface friction factors.
		/// </summary>
		private double surfaceFrictionFactor;

		private readonly DynamicState currentState = new DynamicState();
		private DynamicState beforeStepState;
		private DynamicState beforeIterationState;

		private double lastMovementAirFrictionFactor;
		private double lastMovementUpdateFactor;
		private double? lastMovementTransferFactor;

		private double lastRotationAirFrictionFactor;
		private double lastRotationUpdateFactor;
		private double? lastRotationTransferFactor;

		private IDictionary<string, object> attributeByName;
        
        public virtual long Id
        {
            get
            {
                return id;
            }
        }

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		public virtual Form Form
		{
			get
			{
				return form;
			}
			set
			{
				this.form = value;
			}
		}


		public virtual double Mass
		{
			get
			{
				return mass;
			}
			set
			{
				if (double.IsNaN(value) || value == double.NegativeInfinity || value <= 0.0D)
				{
					throw new System.ArgumentException(this + ": argument 'mass' should be positive.");
				}
    
				this.mass = value;
    
				if (double.IsInfinity(value))
				{
					this.invertedMass = 0.0D;
				}
				else
				{
					this.invertedMass = 1.0D / value;
				}
			}
		}


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(pure = true) public boolean isStatic()
		public virtual bool Static
		{
			get
			{
				return double.IsInfinity(mass);
			}
		}

		public virtual double InvertedMass
		{
			get
			{
				return invertedMass;
			}
		}

		public virtual double AngularMass
		{
			get
			{
				if (double.IsNaN(mass) || mass == double.NegativeInfinity || mass <= 0.0D)
				{
					throw new System.InvalidOperationException(this + ": field 'mass' should be positive.");
				}
    
				if (double.IsInfinity(mass))
				{
					return double.PositiveInfinity;
				}
				else
				{
					return form.getAngularMass(mass);
				}
			}
		}

		public virtual double InvertedAngularMass
		{
			get
			{
				double angularMass = AngularMass;
    
				if (double.IsInfinity(angularMass))
				{
					return 0.0D;
				}
				else
				{
					return 1.0D / angularMass;
				}
			}
		}

		public virtual double MovementAirFrictionFactor
		{
			get
			{
				return movementAirFrictionFactor;
			}
			set
			{
				if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0D || value > 1.0D)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'movementAirFrictionFactor' should be between 0.0 and 1.0 both inclusive but got {1}.", this, value));
				}
    
				this.movementAirFrictionFactor = value;
			}
		}


		public virtual double RotationAirFrictionFactor
		{
			get
			{
				return rotationAirFrictionFactor;
			}
			set
			{
				if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0D || value > 1.0D)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'rotationAirFrictionFactor' should be between 0.0 and 1.0 both inclusive but got {1}.", this, value));
				}
    
				this.rotationAirFrictionFactor = value;
			}
		}


		public virtual MovementFrictionProvider MovementFrictionProvider
		{
			get
			{
				return movementFrictionProvider;
			}
			set
			{
				if (value == null)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'movementFrictionProvider' is null.", this));
				}
    
				this.movementFrictionProvider = value;
			}
		}


		public virtual double MovementFrictionFactor
		{
			set
			{
				MovementFrictionProvider = new ConstantMovementFrictionProvider(value);
			}
		}

		public virtual void applyFriction(double updateFactor)
		{
			movementFrictionProvider.applyFriction(this, updateFactor);
		}

		public virtual double RotationFrictionFactor
		{
			get
			{
				return rotationFrictionFactor;
			}
			set
			{
				if (double.IsNaN(value) || value < 0.0D)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'rotationFrictionFactor' should be zero or positive but got {1}.", this, value));
				}
    
				this.rotationFrictionFactor = value;
			}
		}


		public virtual double MomentumTransferFactor
		{
			get
			{
				return momentumTransferFactor;
			}
			set
			{
				if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0D || value > 1.0D)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'momentumTransferFactor' should be between 0.0 and 1.0 both inclusive but got {1}.", this, value));
				}
    
				this.momentumTransferFactor = value;
			}
		}


		public virtual double SurfaceFrictionFactor
		{
			get
			{
				return surfaceFrictionFactor;
			}
			set
			{
				if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0D || value > 1.0D)
				{
					throw new System.ArgumentException(string.Format("{0}: argument 'surfaceFrictionFactor' should be between 0.0 and 1.0 both inclusive but got {1}.", this, value));
				}
    
				this.surfaceFrictionFactor = value;
			}
		}


		public virtual DynamicState CurrentState
		{
			get
			{
				return currentState;
			}
		}

		public virtual DynamicState BeforeStepState
		{
			get
			{
				return beforeStepState;
			}
		}

		public virtual void saveBeforeStepState()
		{
			this.beforeStepState = new DynamicState(currentState);
		}

		public virtual DynamicState BeforeIterationState
		{
			get
			{
				return beforeIterationState;
			}
		}

		public virtual void saveBeforeIterationState()
		{
			this.beforeIterationState = new DynamicState(currentState);
		}

		public virtual Point2D Position
		{
			get
			{
				return currentState.Position;
			}
			set
			{
				currentState.Position = value;
			}
		}


		public virtual void setPosition(double x, double y)
		{
			Point2D position = currentState.Position;
			if (position == null)
			{
				currentState.Position = new Point2D(x, y);
			}
			else
			{
				position.X = x;
				position.Y = y;
			}
		}

		public virtual double X
		{
			get
			{
				Point2D position = currentState.Position;
				return position == null ? 0.0D : position.X;
			}
			set
			{
				Point2D position = currentState.Position;
				if (position == null)
				{
					currentState.Position = new Point2D(value, 0.0D);
				}
				else
				{
					position.X = value;
				}
			}
		}


		public virtual double Y
		{
			get
			{
				Point2D position = currentState.Position;
				return position == null ? 0.0D : position.Y;
			}
			set
			{
				Point2D position = currentState.Position;
				if (position == null)
				{
					currentState.Position = new Point2D(0.0D, value);
				}
				else
				{
					position.Y = value;
				}
			}
		}


		public virtual Vector2D Velocity
		{
			get
			{
				return currentState.Velocity;
			}
			set
			{
				currentState.Velocity = value;
			}
		}


		public virtual void setVelocity(double x, double y)
		{
			Vector2D velocity = currentState.Velocity;
			if (velocity == null)
			{
				currentState.Velocity = new Vector2D(x, y);
			}
			else
			{
				velocity.X = x;
				velocity.Y = y;
			}
		}

		public virtual double VelocityX
		{
			get
			{
				Vector2D velocity = currentState.Velocity;
				return velocity == null ? 0.0D : velocity.X;
			}
			set
			{
				Vector2D velocity = currentState.Velocity;
				if (velocity == null)
				{
					currentState.Velocity = new Vector2D(value, 0.0D);
				}
				else
				{
					velocity.X = value;
				}
			}
		}


		public virtual double VelocityY
		{
			get
			{
				Vector2D velocity = currentState.Velocity;
				return velocity == null ? 0.0D : velocity.Y;
			}
			set
			{
				Vector2D velocity = currentState.Velocity;
				if (velocity == null)
				{
					currentState.Velocity = new Vector2D(0.0D, value);
				}
				else
				{
					velocity.Y = value;
				}
			}
		}


		public virtual Vector2D MedianVelocity
		{
			get
			{
				return currentState.MedianVelocity;
			}
			set
			{
				currentState.MedianVelocity = value;
			}
		}


		public virtual void setMedianVelocity(double x, double y)
		{
			Vector2D medianVelocity = currentState.MedianVelocity;
			if (medianVelocity == null)
			{
				currentState.MedianVelocity = new Vector2D(x, y);
			}
			else
			{
				medianVelocity.X = x;
				medianVelocity.Y = y;
			}
		}

		public virtual double MedianVelocityX
		{
			get
			{
				Vector2D medianVelocity = currentState.MedianVelocity;
				return medianVelocity == null ? 0.0D : medianVelocity.X;
			}
			set
			{
				Vector2D medianVelocity = currentState.MedianVelocity;
				if (medianVelocity == null)
				{
					currentState.MedianVelocity = new Vector2D(value, 0.0D);
				}
				else
				{
					medianVelocity.X = value;
				}
			}
		}


		public virtual double MedianVelocityY
		{
			get
			{
				Vector2D medianVelocity = currentState.MedianVelocity;
				return medianVelocity == null ? 0.0D : medianVelocity.Y;
			}
			set
			{
				Vector2D medianVelocity = currentState.MedianVelocity;
				if (medianVelocity == null)
				{
					currentState.MedianVelocity = new Vector2D(0.0D, value);
				}
				else
				{
					medianVelocity.Y = value;
				}
			}
		}


		public virtual Vector2D Force
		{
			get
			{
				return currentState.Force;
			}
			set
			{
				currentState.Force = value;
			}
		}


		public virtual void setForce(double x, double y)
		{
			Vector2D force = currentState.Force;
			if (force == null)
			{
				currentState.Force = new Vector2D(x, y);
			}
			else
			{
				force.X = x;
				force.Y = y;
			}
		}

		public virtual double ForceX
		{
			get
			{
				Vector2D force = currentState.Force;
				return force == null ? 0.0D : force.X;
			}
			set
			{
				Vector2D force = currentState.Force;
				if (force == null)
				{
					currentState.Force = new Vector2D(value, 0.0D);
				}
				else
				{
					force.X = value;
				}
			}
		}


		public virtual double ForceY
		{
			get
			{
				Vector2D force = currentState.Force;
				return force == null ? 0.0D : force.Y;
			}
			set
			{
				Vector2D force = currentState.Force;
				if (force == null)
				{
					currentState.Force = new Vector2D(0.0D, value);
				}
				else
				{
					force.Y = value;
				}
			}
		}


		public virtual double Angle
		{
			get
			{
				return currentState.Angle;
			}
			set
			{
				currentState.Angle = value;
			}
		}


		public virtual double AngularVelocity
		{
			get
			{
				return currentState.AngularVelocity;
			}
			set
			{
				currentState.AngularVelocity = value;
			}
		}


		public virtual double MedianAngularVelocity
		{
			get
			{
				return currentState.MedianAngularVelocity;
			}
			set
			{
				currentState.MedianAngularVelocity = value;
			}
		}


		public virtual double Torque
		{
			get
			{
				return currentState.Torque;
			}
			set
			{
				currentState.Torque = value;
			}
		}


		public virtual double getDistanceTo(Body body)
		{
			return currentState.Position.getDistanceTo(body.currentState.Position);
		}

		public virtual double getDistanceTo(Point2D point)
		{
			return currentState.Position.getDistanceTo(point);
		}

		public virtual double getDistanceTo(double x, double y)
		{
			return currentState.Position.getDistanceTo(x, y);
		}

		public virtual double getSquaredDistanceTo(Body body)
		{
			return currentState.Position.getSquaredDistanceTo(body.currentState.Position);
		}

		public virtual double getSquaredDistanceTo(Point2D point)
		{
			return currentState.Position.getSquaredDistanceTo(point);
		}

		public virtual double getSquaredDistanceTo(double x, double y)
		{
			return currentState.Position.getSquaredDistanceTo(x, y);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public com.codeforces.commons.geometry.Point2D getCenterOfMass()
		public virtual Point2D CenterOfMass
		{
			get
			{
				Point2D position = currentState.Position;
				if (position == null)
				{
					throw new System.InvalidOperationException("Can't calculate center of mass for body with no position.");
				}
    
				if (form == null)
				{
					throw new System.InvalidOperationException("Can't calculate center of mass for body with no form.");
				}
    
				return form.getCenterOfMass(this);
			}
		}

		public virtual void normalizeAngle()
		{
			currentState.normalizeAngle();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public java.util.Map<String, Object> getAttributeByName()
		public virtual IDictionary<string, object> AttributeByName
		{
			get
			{
                if (attributeByName == null)
                {
                    attributeByName = new Dictionary<string, object>();
                }

                return attributeByName;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable public Object getAttribute(@Nonnull String name)
		public virtual object getAttribute(string name)
		{
			return attributeByName == null ? null : attributeByName[name];
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void setAttribute(@Nonnull String name, @Nullable Object value)
		public virtual void setAttribute(string name, object value)
		{
			if (value == null)
			{
				if (attributeByName != null)
				{
					attributeByName.Remove(name);
				}
			}
			else
			{
				if (attributeByName == null)
				{
                    attributeByName = new Dictionary<string, object>();
				}

				attributeByName[name] = value;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("FloatingPointEquality") void applyMovementAirFriction(double updateFactor)
		internal virtual void applyMovementAirFriction(double updateFactor)
		{
			if (lastMovementTransferFactor == null || movementAirFrictionFactor != lastMovementAirFrictionFactor || updateFactor != lastMovementUpdateFactor)
			{
				lastMovementAirFrictionFactor = movementAirFrictionFactor;
				lastMovementUpdateFactor = updateFactor;
				lastMovementTransferFactor = Math.Pow(1.0D - movementAirFrictionFactor, updateFactor);
			}

			Velocity.subtract(MedianVelocity).multiply(lastMovementTransferFactor.Value).add(MedianVelocity);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("FloatingPointEquality") void applyRotationAirFriction(double updateFactor)
		internal virtual void applyRotationAirFriction(double updateFactor)
		{
			if (lastRotationTransferFactor == null || rotationAirFrictionFactor != lastRotationAirFrictionFactor || updateFactor != lastRotationUpdateFactor)
			{
				lastRotationAirFrictionFactor = rotationAirFrictionFactor;
				lastRotationUpdateFactor = updateFactor;
                lastRotationTransferFactor = Math.Pow(1.0D - rotationAirFrictionFactor, updateFactor);
			}

			AngularVelocity = (AngularVelocity - MedianAngularVelocity) * lastRotationTransferFactor.Value + MedianAngularVelocity;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(value = "null -> false", pure = true) public boolean equals(@Nullable Body body)
		public virtual bool Equals(Body body)
		{
			return body != null && id == body.id;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(pure = true) @Override public int hashCode()
		public override int GetHashCode()
		{
			return (int)(id ^ ((long)((ulong)id >> 32)));
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract("null -> false") @Override public boolean equals(@Nullable Object o)
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

			Body body = (Body) o;

			return id == body.id;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public String toString()
        //public override string ToString()
        //{
        //    return ToString(this);
        //}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull public static String toString(Body body)
        //public static string ToString(Body body)
        //{
        //    return StringUtil.ToString(typeof(Body), body, true, "id", "name", "position", "angle", "velocity", "angularVelocity");
        //}
	}

}