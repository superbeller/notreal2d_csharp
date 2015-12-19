using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using NumberUtil = com.codeforces.commons.math.NumberUtil;
    //using LongPair = com.codeforces.commons.pair.LongPair;
    //using BodyList = com.codegame.codeseries.notreal2d.bodylist.BodyList;
    //using SimpleBodyList = com.codegame.codeseries.notreal2d.bodylist.SimpleBodyList;
    //using com.codegame.codeseries.notreal2d.collision;
    //using CollisionListener = com.codegame.codeseries.notreal2d.CollisionListener;
    //using MomentumTransferFactorProvider = com.codegame.codeseries.notreal2d.provider.MomentumTransferFactorProvider;
    //using Vector3D = org.apache.commons.math3.geometry.euclidean.threed.Vector3D;
    //using Level = org.apache.log4j.Level;
    //using Logger = org.apache.log4j.Logger;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.*;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
	public class PhysWorld
	{
		//private static readonly Logger logger = Logger.getLogger(typeof(World));

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("ConstantConditions") private static final CollisionInfo NULL_COLLISION_INFO = new CollisionInfo(null, null, null, null, 0.0D, 0.0D);
		private static readonly CollisionInfo NULL_COLLISION_INFO = new CollisionInfo(null, null, null, null, 0.0D, 0.0D);

		private readonly int iterationCountPerStep;
		private readonly int stepCountPerTimeUnit;
		private readonly double updateFactor;

		private readonly double epsilon;
		private readonly double squaredEpsilon;

		private readonly BodyList bodyList;
		private readonly MomentumTransferFactorProvider momentumTransferFactorProvider;

		private readonly IDictionary<string, ColliderEntry> colliderEntryByName = new Dictionary<string, ColliderEntry>();
		private readonly SortedSet<ColliderEntry> colliderEntries = new SortedSet<ColliderEntry>(ColliderEntry.comparator);

		private readonly IDictionary<string, CollisionListenerEntry> collisionListenerEntryByName = new Dictionary<string, CollisionListenerEntry>();
		private readonly SortedSet<CollisionListenerEntry> collisionListenerEntries = new SortedSet<CollisionListenerEntry>(CollisionListenerEntry.comparator);

		public PhysWorld() : this(Defaults.ITERATION_COUNT_PER_STEP)
		{
		}

		public PhysWorld(int iterationCountPerStep) : this(iterationCountPerStep, Defaults.STEP_COUNT_PER_TIME_UNIT)
		{
		}

		public PhysWorld(int iterationCountPerStep, int stepCountPerTimeUnit) : this(iterationCountPerStep, stepCountPerTimeUnit, Defaults.EPSILON)
		{
		}

		public PhysWorld(int iterationCountPerStep, int stepCountPerTimeUnit, double epsilon) : this(iterationCountPerStep, stepCountPerTimeUnit, epsilon, new SimpleBodyList())
		{
		}

		public PhysWorld(int iterationCountPerStep, int stepCountPerTimeUnit, double epsilon, BodyList bodyList) : this(iterationCountPerStep, stepCountPerTimeUnit, epsilon, bodyList, null)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public World(int iterationCountPerStep, int stepCountPerTimeUnit, double epsilon, com.codegame.codeseries.notreal2d.bodylist.BodyList bodyList, @Nullable com.codegame.codeseries.notreal2d.provider.MomentumTransferFactorProvider momentumTransferFactorProvider)
		public PhysWorld(int iterationCountPerStep, int stepCountPerTimeUnit, double epsilon, BodyList bodyList, MomentumTransferFactorProvider momentumTransferFactorProvider)
		{
			if (iterationCountPerStep < 1)
			{
				throw new System.ArgumentException("Argument 'iterationCountPerStep' is zero or negative.");
			}

			if (stepCountPerTimeUnit < 1)
			{
				throw new System.ArgumentException("Argument 'stepCountPerTimeUnit' is zero or negative.");
			}

			if (double.IsNaN(epsilon) || double.IsInfinity(epsilon) || epsilon < 1.0E-100D || epsilon > 1.0D)
			{
				throw new System.ArgumentException("Argument 'epsilon' should be between 1.0E-100 and 1.0.");
			}

			if (bodyList == null)
			{
				throw new System.ArgumentException("Argument 'bodyList' is null.");
			}

			this.stepCountPerTimeUnit = stepCountPerTimeUnit;
			this.iterationCountPerStep = iterationCountPerStep;
			this.updateFactor = 1.0D / (stepCountPerTimeUnit * iterationCountPerStep);
			this.epsilon = epsilon;
			this.squaredEpsilon = epsilon * epsilon;
			this.bodyList = bodyList;
			this.momentumTransferFactorProvider = momentumTransferFactorProvider;

			registerCollider(new ArcAndArcCollider(epsilon));
			registerCollider(new ArcAndCircleCollider(epsilon));
			registerCollider(new CircleAndCircleCollider(epsilon));
			registerCollider(new LineAndArcCollider(epsilon));
			registerCollider(new LineAndCircleCollider(epsilon));
			registerCollider(new LineAndLineCollider(epsilon));
			registerCollider(new LineAndRectangleCollider(epsilon));
			registerCollider(new RectangleAndArcCollider(epsilon));
			registerCollider(new RectangleAndCircleCollider(epsilon));
			registerCollider(new RectangleAndRectangleCollider(epsilon));
		}

		public virtual int IterationCountPerStep
		{
			get
			{
				return iterationCountPerStep;
			}
		}

		public virtual int StepCountPerTimeUnit
		{
			get
			{
				return stepCountPerTimeUnit;
			}
		}

		public virtual double Epsilon
		{
			get
			{
				return epsilon;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void addBody(@Nonnull Body body)
		public virtual void addBody(Body body)
		{
			if (body.Form == null || body.Mass == 0.0D)
			{
				throw new System.ArgumentException("Specify form and mass of 'body' before adding to the world.");
			}

			bodyList.addBody(body);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void removeBody(@Nonnull Body body)
		public virtual void removeBody(Body body)
		{
			bodyList.removeBody(body);
		}

		public virtual void removeBody(long id)
		{
			bodyList.removeBody(id);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void removeBodyQuietly(@Nullable Body body)
		public virtual void removeBodyQuietly(Body body)
		{
			bodyList.removeBodyQuietly(body);
		}

		public virtual void removeBodyQuietly(long id)
		{
			bodyList.removeBodyQuietly(id);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public boolean hasBody(@Nonnull Body body)
		public virtual bool hasBody(Body body)
		{
			return bodyList.hasBody(body);
		}

		public virtual bool hasBody(long id)
		{
			return bodyList.hasBody(id);
		}

		public virtual Body getBody(long id)
		{
			return bodyList.getBody(id);
		}

		public virtual IList<Body> Bodies
		{
			get
			{
				return bodyList.Bodies;
			}
		}

		public virtual void proceed()
		{
			IList<Body> bodies = new List<Body>(Bodies);

			foreach (Body body in bodies)
			{
				if (!hasBody(body))
				{
					continue;
				}

				body.normalizeAngle();
				body.saveBeforeStepState();
			}

			for (int i = 1; i <= iterationCountPerStep; ++i)
			{
				foreach (Body body in bodies)
				{
					if (!hasBody(body))
					{
						continue;
					}

					body.saveBeforeIterationState();
					updateState(body);
					body.normalizeAngle();
				}

				IDictionary<LongPair, CollisionInfo> collisionInfoByBodyIdsPair = new Dictionary<LongPair, CollisionInfo>();

				foreach (Body body in bodies)
				{
					if (body.Static || !hasBody(body))
					{
						continue;
					}

					foreach (Body otherBody in bodyList.getPotentialIntersections(body))
					{
						if (hasBody(body) && hasBody(otherBody))
						{
							collide(body, otherBody, collisionInfoByBodyIdsPair);
						}
					}
				}
			}

			foreach (Body body in bodies)
			{
				if (!hasBody(body))
				{
					continue;
				}

				body.setForce(0.0D, 0.0D);
				body.Torque = 0.0D;
			}
		}

		private void collide(Body body, Body otherBody, IDictionary<LongPair, CollisionInfo> collisionInfoByBodyIdsPair)
		{
			Body bodyA;
			Body bodyB;

			if (body.Id > otherBody.Id)
			{
				bodyA = otherBody;
				bodyB = body;
			}
			else
			{
				bodyA = body;
				bodyB = otherBody;
			}

			LongPair bodyIdsPair = new LongPair(bodyA.Id, bodyB.Id);

			CollisionInfo collisionInfo = collisionInfoByBodyIdsPair[bodyIdsPair];
			if (collisionInfo != null)
			{
				return;
			}

			foreach (CollisionListenerEntry collisionListenerEntry in collisionListenerEntries)
			{
				if (!collisionListenerEntry.listener.beforeStartingCollision(bodyA, bodyB))
				{
					collisionInfoByBodyIdsPair[bodyIdsPair] = NULL_COLLISION_INFO;
					return;
				}

				if (!hasBody(bodyA) || !hasBody(bodyB))
				{
					return;
				}
			}

			foreach (ColliderEntry colliderEntry in colliderEntries)
			{
				if (colliderEntry.collider.matches(bodyA, bodyB))
				{
					collisionInfo = colliderEntry.collider.collide(bodyA, bodyB);
					break;
				}
			}

			if (collisionInfo == null)
			{
				collisionInfoByBodyIdsPair[bodyIdsPair] = NULL_COLLISION_INFO;
			}
			else
			{
				collisionInfoByBodyIdsPair[bodyIdsPair] = collisionInfo;
				resolveCollision(collisionInfo);
			}
		}

		private void resolveCollision(CollisionInfo collisionInfo)
		{
			Body bodyA = collisionInfo.BodyA;
			Body bodyB = collisionInfo.BodyB;

			if (bodyA.Static && bodyB.Static)
			{
				throw new System.ArgumentException("Both " + bodyA + " and " + bodyB + " are static.");
			}

			foreach (CollisionListenerEntry collisionListenerEntry in collisionListenerEntries)
			{
				if (!collisionListenerEntry.listener.beforeResolvingCollision(collisionInfo))
				{
					return;
				}

				if (!hasBody(bodyA) || !hasBody(bodyB))
				{
					return;
				}
			}

			//logCollision(collisionInfo);

			Vector3D collisionNormalB = toVector3D(collisionInfo.NormalB);

			Vector3D vectorAC = toVector3D(bodyA.CenterOfMass, collisionInfo.Point);
			Vector3D vectorBC = toVector3D(bodyB.CenterOfMass, collisionInfo.Point);

			Vector3D angularVelocityPartAC = toVector3DZ(bodyA.AngularVelocity).crossProduct(vectorAC);
			Vector3D angularVelocityPartBC = toVector3DZ(bodyB.AngularVelocity).crossProduct(vectorBC);

			Vector3D velocityAC = toVector3D(bodyA.Velocity).add(angularVelocityPartAC);
			Vector3D velocityBC = toVector3D(bodyB.Velocity).add(angularVelocityPartBC);

			Vector3D relativeVelocityC = velocityAC.subtract(velocityBC);
			double normalRelativeVelocityLengthC = -relativeVelocityC.dotProduct(collisionNormalB);

			if (normalRelativeVelocityLengthC > -epsilon)
			{
				resolveImpact(bodyA, bodyB, collisionNormalB, vectorAC, vectorBC, relativeVelocityC);
				resolveSurfaceFriction(bodyA, bodyB, collisionNormalB, vectorAC, vectorBC, relativeVelocityC);
			}

			if (collisionInfo.Depth >= epsilon)
			{
				pushBackBodies(bodyA, bodyB, collisionInfo);
			}

			bodyA.normalizeAngle();
			bodyB.normalizeAngle();

			foreach (CollisionListenerEntry collisionListenerEntry in collisionListenerEntries)
			{
				collisionListenerEntry.listener.afterResolvingCollision(collisionInfo);
			}
		}

		private void resolveImpact(Body bodyA, Body bodyB, Vector3D collisionNormalB, Vector3D vectorAC, Vector3D vectorBC, Vector3D relativeVelocityC)
		{
			double? momentumTransferFactor;

			if (momentumTransferFactorProvider == null || (momentumTransferFactor = momentumTransferFactorProvider.getFactor(bodyA, bodyB)) == null)
			{
				momentumTransferFactor = bodyA.MomentumTransferFactor * bodyB.MomentumTransferFactor;
			}

			Vector3D denominatorPartA = vectorAC.crossProduct(collisionNormalB).scalarMultiply(bodyA.InvertedAngularMass).crossProduct(vectorAC);
			Vector3D denominatorPartB = vectorBC.crossProduct(collisionNormalB).scalarMultiply(bodyB.InvertedAngularMass).crossProduct(vectorBC);

			double denominator = bodyA.InvertedMass + bodyB.InvertedMass + collisionNormalB.dotProduct(denominatorPartA.add(denominatorPartB));

			double impulseChange = -1.0D * (1.0D + momentumTransferFactor.Value) * relativeVelocityC.dotProduct(collisionNormalB) / denominator;

			if (Math.Abs(impulseChange) < epsilon)
			{
				return;
			}

			if (!bodyA.Static)
			{
				Vector3D velocityChangeA = collisionNormalB.scalarMultiply(impulseChange * bodyA.InvertedMass);
				Vector3D newVelocityA = toVector3D(bodyA.Velocity).add(velocityChangeA);
				bodyA.setVelocity(newVelocityA.X, newVelocityA.Y);

				Vector3D angularVelocityChangeA = vectorAC.crossProduct(collisionNormalB.scalarMultiply(impulseChange)).scalarMultiply(bodyA.InvertedAngularMass);
				Vector3D newAngularVelocityA = toVector3DZ(bodyA.AngularVelocity).add(angularVelocityChangeA);
				bodyA.AngularVelocity = newAngularVelocityA.Z;
			}

			if (!bodyB.Static)
			{
				Vector3D velocityChangeB = collisionNormalB.scalarMultiply(impulseChange * bodyB.InvertedMass);
				Vector3D newVelocityB = toVector3D(bodyB.Velocity).subtract(velocityChangeB);
				bodyB.setVelocity(newVelocityB.X, newVelocityB.Y);

				Vector3D angularVelocityChangeB = vectorBC.crossProduct(collisionNormalB.scalarMultiply(impulseChange)).scalarMultiply(bodyB.InvertedAngularMass);
				Vector3D newAngularVelocityB = toVector3DZ(bodyB.AngularVelocity).subtract(angularVelocityChangeB);
				bodyB.AngularVelocity = newAngularVelocityB.Z;
			}
		}

		private void resolveSurfaceFriction(Body bodyA, Body bodyB, Vector3D collisionNormalB, Vector3D vectorAC, Vector3D vectorBC, Vector3D relativeVelocityC)
		{
			Vector3D tangent = relativeVelocityC.subtract(collisionNormalB.scalarMultiply(relativeVelocityC.dotProduct(collisionNormalB)));

			if (tangent.NormSq < squaredEpsilon)
			{
				return;
			}

			tangent = tangent.normalize();

            double surfaceFriction = Math.Sqrt(bodyA.SurfaceFrictionFactor * bodyB.SurfaceFrictionFactor) * Math.Sqrt(2.0) * Math.Abs(relativeVelocityC.dotProduct(collisionNormalB)) / relativeVelocityC.Norm;

			if (surfaceFriction < epsilon)
			{
				return;
			}

			Vector3D denominatorPartA = vectorAC.crossProduct(tangent).scalarMultiply(bodyA.InvertedAngularMass).crossProduct(vectorAC);
			Vector3D denominatorPartB = vectorBC.crossProduct(tangent).scalarMultiply(bodyB.InvertedAngularMass).crossProduct(vectorBC);

			double denominator = bodyA.InvertedMass + bodyB.InvertedMass + tangent.dotProduct(denominatorPartA.add(denominatorPartB));

			double impulseChange = -1.0D * surfaceFriction * relativeVelocityC.dotProduct(tangent) / denominator;

			if (Math.Abs(impulseChange) < epsilon)
			{
				return;
			}

			if (!bodyA.Static)
			{
				Vector3D velocityChangeA = tangent.scalarMultiply(impulseChange * bodyA.InvertedMass);
				Vector3D newVelocityA = toVector3D(bodyA.Velocity).add(velocityChangeA);
				bodyA.setVelocity(newVelocityA.X, newVelocityA.Y);

				Vector3D angularVelocityChangeA = vectorAC.crossProduct(tangent.scalarMultiply(impulseChange)).scalarMultiply(bodyA.InvertedAngularMass);
				Vector3D newAngularVelocityA = toVector3DZ(bodyA.AngularVelocity).add(angularVelocityChangeA);
				bodyA.AngularVelocity = newAngularVelocityA.Z;
			}

			if (!bodyB.Static)
			{
				Vector3D velocityChangeB = tangent.scalarMultiply(impulseChange * bodyB.InvertedMass);
				Vector3D newVelocityB = toVector3D(bodyB.Velocity).subtract(velocityChangeB);
				bodyB.setVelocity(newVelocityB.X, newVelocityB.Y);

				Vector3D angularVelocityChangeB = vectorBC.crossProduct(tangent.scalarMultiply(impulseChange)).scalarMultiply(bodyB.InvertedAngularMass);
				Vector3D newAngularVelocityB = toVector3DZ(bodyB.AngularVelocity).subtract(angularVelocityChangeB);
				bodyB.AngularVelocity = newAngularVelocityB.Z;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void updateState(@Nonnull Body body)
		public void updateState(Body body)
		{
			updatePosition(body);
			updateAngle(body);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void updatePosition(@Nonnull Body body)
		private void updatePosition(Body body)
		{
			if (body.Velocity.SquaredLength > 0.0D)
			{
				body.Position.add(body.Velocity.copy().multiply(updateFactor));
			}

			if (body.Force.SquaredLength > 0.0D)
			{
				body.Velocity.add(body.Force.copy().multiply(body.InvertedMass).multiply(updateFactor));
			}

			if (body.MovementAirFrictionFactor >= 1.0D)
			{
				body.Velocity = body.MedianVelocity.copy();
			}
			else if (body.MovementAirFrictionFactor > 0.0D)
			{
				body.applyMovementAirFriction(updateFactor);

				if (body.Velocity.nearlyEquals(body.MedianVelocity, epsilon))
				{
					body.Velocity = body.MedianVelocity.copy();
				}
			}

			body.Velocity.subtract(body.MedianVelocity);
			body.applyFriction(updateFactor);
			body.Velocity.add(body.MedianVelocity);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void updateAngle(@Nonnull Body body)
		private void updateAngle(Body body)
		{
			body.Angle = body.Angle + body.AngularVelocity * updateFactor;
			body.AngularVelocity = body.AngularVelocity + body.Torque * body.InvertedAngularMass * updateFactor;

			if (body.RotationAirFrictionFactor >= 1.0D)
			{
				body.AngularVelocity = body.MedianAngularVelocity;
			}
			else if (body.RotationAirFrictionFactor > 0.0D)
			{
				body.applyRotationAirFriction(updateFactor);

				if (NumberUtil.nearlyEquals(body.AngularVelocity, body.MedianAngularVelocity, epsilon))
				{
					body.AngularVelocity = body.MedianAngularVelocity;
				}
			}

			double angularVelocity = body.AngularVelocity - body.MedianAngularVelocity;

			if (Math.Abs(angularVelocity) > 0.0D)
			{
				double rotationFrictionFactor = body.RotationFrictionFactor * updateFactor;

				if (rotationFrictionFactor >= Math.Abs(angularVelocity))
				{
					body.AngularVelocity = body.MedianAngularVelocity;
				}
				else if (rotationFrictionFactor > 0.0D)
				{
					if (angularVelocity > 0.0D)
					{
						body.AngularVelocity = angularVelocity - rotationFrictionFactor + body.MedianAngularVelocity;
					}
					else
					{
						body.AngularVelocity = angularVelocity + rotationFrictionFactor + body.MedianAngularVelocity;
					}
				}
			}
		}

		private void pushBackBodies(Body bodyA, Body bodyB, CollisionInfo collisionInfo)
		{
			if (bodyA.Static)
			{
				bodyB.Position.subtract(collisionInfo.NormalB.multiply(collisionInfo.Depth + epsilon));
			}
			else if (bodyB.Static)
			{
				bodyA.Position.add(collisionInfo.NormalB.multiply(collisionInfo.Depth + epsilon));
			}
			else
			{
				Vector2D normalOffset = collisionInfo.NormalB.multiply(0.5D * (collisionInfo.Depth + epsilon));
				bodyA.Position.add(normalOffset);
				bodyB.Position.subtract(normalOffset);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerCollider(@Nonnull Collider collider, @Nonnull String name, double priority)
		public virtual void registerCollider(Collider collider, string name, double priority)
		{
			NamedEntry.validateName(name);

			if (colliderEntryByName.ContainsKey(name))
			{
				throw new System.ArgumentException("Collider '" + name + "' is already registered.");
			}

			ColliderEntry colliderEntry = new ColliderEntry(name, priority, collider);
			colliderEntryByName[name] = colliderEntry;
			colliderEntries.Add(colliderEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerCollider(@Nonnull Collider collider, @Nonnull String name)
		public virtual void registerCollider(Collider collider, string name)
		{
			registerCollider(collider, name, 0.0D);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void registerCollider(@Nonnull Collider collider)
		private void registerCollider(Collider collider)
		{
			registerCollider(collider, collider.GetType().Name);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void unregisterCollider(@Nonnull String name)
		public virtual void unregisterCollider(string name)
		{
			NamedEntry.validateName(name);
			
			if (!colliderEntryByName.Keys.Contains(name))
			{
				throw new System.ArgumentException("Collider '" + name + "' is not registered.");
			}

            ColliderEntry colliderEntry = colliderEntryByName[name];
			colliderEntries.Remove(colliderEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public boolean hasCollider(@Nonnull String name)
		public virtual bool hasCollider(string name)
		{
			NamedEntry.validateName(name);
			return colliderEntryByName.ContainsKey(name);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerCollisionListener(@Nonnull com.codegame.codeseries.notreal2d.CollisionListener listener, @Nonnull String name, double priority)
		public virtual void registerCollisionListener(CollisionListener listener, string name, double priority)
		{
			NamedEntry.validateName(name);

			if (collisionListenerEntryByName.ContainsKey(name))
			{
				throw new System.ArgumentException("Listener '" + name + "' is already registered.");
			}

			CollisionListenerEntry collisionListenerEntry = new CollisionListenerEntry(name, priority, listener);
			collisionListenerEntryByName[name] = collisionListenerEntry;
			collisionListenerEntries.Add(collisionListenerEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerCollisionListener(@Nonnull com.codegame.codeseries.notreal2d.CollisionListener listener, @Nonnull String name)
		public virtual void registerCollisionListener(CollisionListener listener, string name)
		{
			registerCollisionListener(listener, name, 0.0D);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void registerCollisionListener(@Nonnull com.codegame.codeseries.notreal2d.CollisionListener listener)
		private void registerCollisionListener(CollisionListener listener)
		{
			registerCollisionListener(listener, listener.GetType().Name);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void unregisterCollisionListener(@Nonnull String name)
		public virtual void unregisterCollisionListener(string name)
		{
			NamedEntry.validateName(name);
			
			if (!collisionListenerEntryByName.Keys.Contains(name))
			{
				throw new System.ArgumentException("Listener '" + name + "' is not registered.");
			}

            CollisionListenerEntry collisionListenerEntry = collisionListenerEntryByName[name];
			collisionListenerEntries.Remove(collisionListenerEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public boolean hasCollisionListener(@Nonnull String name)
		public virtual bool hasCollisionListener(string name)
		{
			NamedEntry.validateName(name);
			return collisionListenerEntryByName.ContainsKey(name);
		}

        //private static void logCollision(CollisionInfo collisionInfo)
        //{
        //    if (collisionInfo.Depth >= collisionInfo.BodyA.Form.CircumcircleRadius * 0.25D || collisionInfo.Depth >= collisionInfo.BodyB.Form.CircumcircleRadius * 0.25D)
        //    {
        //        if (logger.isEnabledFor(Level.WARN))
        //        {
        //            logger.warn("Resolving collision (big depth) " + collisionInfo + '.');
        //        }
        //    }
        //    else
        //    {
        //        if (logger.DebugEnabled)
        //        {
        //            logger.debug("Resolving collision " + collisionInfo + '.');
        //        }
        //    }
        //}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private static org.apache.commons.math3.geometry.euclidean.threed.Vector3D toVector3DZ(double z)
		private static Vector3D toVector3DZ(double z)
		{
			return new Vector3D(0.0D, 0.0D, z);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private static org.apache.commons.math3.geometry.euclidean.threed.Vector3D toVector3D(@Nonnull com.codeforces.commons.geometry.Vector2D vector)
		private static Vector3D toVector3D(Vector2D vector)
		{
			return new Vector3D(vector.X, vector.Y, 0.0D);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private static org.apache.commons.math3.geometry.euclidean.threed.Vector3D toVector3D(@Nonnull com.codeforces.commons.geometry.Point2D point1, @Nonnull com.codeforces.commons.geometry.Point2D point2)
		private static Vector3D toVector3D(Point2D point1, Point2D point2)
		{
			return toVector3D(new Vector2D(point1, point2));
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("PublicField") private static final class ColliderEntry extends NamedEntry
		private sealed class ColliderEntry : NamedEntry
		{
			internal static readonly IComparer<ColliderEntry> comparator = new ComparatorAnonymousInnerClassHelper();

			private class ComparatorAnonymousInnerClassHelper : IComparer<ColliderEntry>
			{
				public ComparatorAnonymousInnerClassHelper()
				{
				}

				public virtual int Compare(ColliderEntry colliderEntryA, ColliderEntry colliderEntryB)
				{
					int comparisonResult = colliderEntryB.priority.CompareTo(colliderEntryA.priority);
					if (comparisonResult != 0)
					{
						return comparisonResult;
					}

					return colliderEntryA.name.CompareTo(colliderEntryB.name);
				}
			}

			public readonly double priority;
			public readonly Collider collider;

			internal ColliderEntry(string name, double priority, Collider collider) : base(name)
			{

				this.priority = priority;
				this.collider = collider;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("PublicField") private static final class CollisionListenerEntry extends NamedEntry
		private sealed class CollisionListenerEntry : NamedEntry
		{
			internal static readonly IComparer<CollisionListenerEntry> comparator = new ComparatorAnonymousInnerClassHelper();

			private class ComparatorAnonymousInnerClassHelper : IComparer<CollisionListenerEntry>
			{
				public ComparatorAnonymousInnerClassHelper()
				{
				}

				public virtual int Compare(CollisionListenerEntry listenerEntryA, CollisionListenerEntry listenerEntryB)
				{
					int comparisonResult = listenerEntryB.priority.CompareTo(listenerEntryA.priority);
					if (comparisonResult != 0)
					{
						return comparisonResult;
					}

					return listenerEntryA.name.CompareTo(listenerEntryB.name);
				}
			}

			public readonly double priority;
			public readonly CollisionListener listener;

			internal CollisionListenerEntry(string name, double priority, CollisionListener listener) : base(name)
			{

				this.priority = priority;
				this.listener = listener;
			}
		}
	}

}