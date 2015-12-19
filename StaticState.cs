using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;
    //using PositionListener = com.codegame.codeseries.notreal2d.PositionListener;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.DOUBLE_PI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.PI;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 31.08.2015
	/// </summary>
	public class StaticState
	{
		private ListeningPoint2D position;
		private double angle;

		private IDictionary<string, PositionListenerEntry> positionListenerEntryByName;
        private SortedSet<PositionListenerEntry> positionListenerEntries;

		public StaticState()
		{
			this.position = new ListeningPoint2D(this, 0.0D, 0.0D);
		}

		public StaticState(Point2D position, double angle)
		{
			this.position = new ListeningPoint2D(this, position);
			this.angle = angle;
		}

		public StaticState(StaticState state)
		{
			this.position = new ListeningPoint2D(this, state.position);
			this.angle = state.angle;
		}

		public virtual Point2D Position
		{
			get
			{
				return position;
			}
			set
			{
				Point2D oldPosition = this.position.copy();
				Point2D newPosition = value.copy();
    
				if (positionListenerEntries != null)
				{
					foreach (PositionListenerEntry positionListenerEntry in positionListenerEntries)
					{
						if (!positionListenerEntry.listener.beforeChangePosition(oldPosition.copy(), newPosition))
						{
							return;
						}
					}
				}
    
				this.position = new ListeningPoint2D(this, newPosition);
    
				if (positionListenerEntries != null)
				{
					foreach (PositionListenerEntry positionListenerEntry in positionListenerEntries)
					{
						positionListenerEntry.listener.afterChangePosition(oldPosition.copy(), newPosition.copy());
					}
				}
			}
		}


		public virtual double Angle
		{
			get
			{
				return angle;
			}
			set
			{
				this.angle = value;
			}
		}


		public virtual void normalizeAngle()
		{
			while (angle > Utils.PI)
			{
                angle -= Utils.DOUBLE_PI;
			}

            while (angle < -Utils.PI)
			{
                angle += Utils.DOUBLE_PI;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerPositionListener(@Nonnull com.codegame.codeseries.notreal2d.PositionListener listener, @Nonnull String name, double priority)
		public virtual void registerPositionListener(PositionListener listener, string name, double priority)
		{
			NamedEntry.validateName(name);

			if (positionListenerEntryByName == null)
			{
				positionListenerEntryByName = new Dictionary<string, PositionListenerEntry>();
                positionListenerEntries = new SortedSet<PositionListenerEntry>(PositionListenerEntry.comparator); // maybe need PositionListenerEntry.comparator
			}
			else if (positionListenerEntryByName.ContainsKey(name))
			{
				throw new System.ArgumentException("Listener '" + name + "' is already registered.");
			}

			PositionListenerEntry positionListenerEntry = new PositionListenerEntry(name, priority, listener);
			positionListenerEntryByName[name] = positionListenerEntry;
			positionListenerEntries.Add(positionListenerEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void registerPositionListener(@Nonnull com.codegame.codeseries.notreal2d.PositionListener listener, @Nonnull String name)
		public virtual void registerPositionListener(PositionListener listener, string name)
		{
			registerPositionListener(listener, name, 0.0D);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void registerPositionListener(@Nonnull com.codegame.codeseries.notreal2d.PositionListener listener)
		private void registerPositionListener(PositionListener listener)
		{
			registerPositionListener(listener, listener.GetType().Name);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public void unregisterPositionListener(@Nonnull String name)
		public virtual void unregisterPositionListener(string name)
		{
			NamedEntry.validateName(name);			

            if (positionListenerEntryByName == null || !positionListenerEntryByName.Keys.Contains(name))
			{
				throw new System.ArgumentException("Listener '" + name + "' is not registered.");
			}

            PositionListenerEntry positionListenerEntry = positionListenerEntryByName[name];

			positionListenerEntries.Remove(positionListenerEntry);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public boolean hasPositionListener(@Nonnull String name)
		public virtual bool hasPositionListener(string name)
		{
			NamedEntry.validateName(name);
			return positionListenerEntryByName != null && positionListenerEntryByName.ContainsKey(name);
		}

        //public override string ToString()
        //{
        //    return StringUtil.ToString(this, false);
        //}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"RefusedBequest", "NonStaticInnerClassInSecureContext"}) private final class ListeningPoint2D extends com.codeforces.commons.geometry.Point2D
		private sealed class ListeningPoint2D : Point2D
		{
			private readonly StaticState outerInstance;

			internal ListeningPoint2D(StaticState outerInstance, double x, double y) : base(x, y)
			{
				this.outerInstance = outerInstance;
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private ListeningPoint2D(@Nonnull com.codeforces.commons.geometry.Point2D point)
			internal ListeningPoint2D(StaticState outerInstance, Point2D point) : base(point)
			{
				this.outerInstance = outerInstance;
			}

			public override double X
			{
				set
				{
					First = value;
				}
			}

			public override double Y
			{
				set
				{
					Second = value;
				}
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public com.codeforces.commons.geometry.Point2D add(com.codeforces.commons.geometry.Vector2D vector)
			public override Point2D add(Vector2D vector)
			{
				Point2D oldPosition = base.copy();
				Point2D newPosition = base.copy().add(vector);

				return onChange(oldPosition, newPosition);
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public com.codeforces.commons.geometry.Point2D add(double x, double y)
			public override Point2D add(double x, double y)
			{
				Point2D oldPosition = base.copy();
				Point2D newPosition = base.copy().add(x, y);

				return onChange(oldPosition, newPosition);
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public com.codeforces.commons.geometry.Point2D subtract(com.codeforces.commons.geometry.Vector2D vector)
			public override Point2D subtract(Vector2D vector)
			{
				Point2D oldPosition = base.copy();
				Point2D newPosition = base.copy().subtract(vector);

				return onChange(oldPosition, newPosition);
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull @Override public com.codeforces.commons.geometry.Point2D subtract(double x, double y)
			public override Point2D subtract(double x, double y)
			{
				Point2D oldPosition = base.copy();
				Point2D newPosition = base.copy().subtract(x, y);

				return onChange(oldPosition, newPosition);
			}

			public double? First
			{
				set
				{
					Point2D oldPosition = base.copy();
					Point2D newPosition = base.copy();
					newPosition.First = value;
    
					onChange(oldPosition, newPosition);
				}
			}

			public double? Second
			{
				set
				{
					Point2D oldPosition = base.copy();
					Point2D newPosition = base.copy();
					newPosition.Second = value;
    
					onChange(oldPosition, newPosition);
				}
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private com.codeforces.commons.geometry.Point2D onChange(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition)
			public Point2D onChange(Point2D oldPosition, Point2D newPosition)
			{
				if (outerInstance.positionListenerEntries != null)
				{
					foreach (PositionListenerEntry positionListenerEntry in outerInstance.positionListenerEntries)
					{
						if (!positionListenerEntry.listener.beforeChangePosition(oldPosition.copy(), newPosition))
						{
							return this;
						}
					}
				}

				base.First = newPosition.First;
				base.Second = newPosition.Second;

				if (outerInstance.positionListenerEntries != null)
				{
					foreach (PositionListenerEntry positionListenerEntry in outerInstance.positionListenerEntries)
					{
						positionListenerEntry.listener.afterChangePosition(oldPosition.copy(), newPosition.copy());
					}
				}

				return this;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("PublicField") private static final class PositionListenerEntry extends NamedEntry
		private sealed class PositionListenerEntry : NamedEntry
		{
			internal static readonly IComparer<PositionListenerEntry> comparator = new ComparatorAnonymousInnerClassHelper();

			private class ComparatorAnonymousInnerClassHelper : IComparer<PositionListenerEntry>
			{
				public ComparatorAnonymousInnerClassHelper()
				{
				}

				public virtual int Compare(PositionListenerEntry listenerEntryA, PositionListenerEntry listenerEntryB)
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
			public readonly PositionListener listener;

			internal PositionListenerEntry(string name, double priority, PositionListener listener) : base(name)
			{

				this.priority = priority;
				this.listener = listener;
			}
		}
	}

}