using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using NumberUtil = com.codeforces.commons.math.NumberUtil;
    //using IntPair = com.codeforces.commons.pair.IntPair;
    //using PositionListenerAdapter = com.codegame.codeseries.notreal2d.PositionListenerAdapter;
    //using UnmodifiableIterator = com.google.common.collect.UnmodifiableIterator;
    //using ArrayUtils = org.apache.commons.lang3.ArrayUtils;
    //using Contract = org.jetbrains.annotations.Contract;


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.floor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.sqr;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 02.06.2015
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @NotThreadSafe public class CellSpaceBodyList extends BodyListBase
	public class CellSpaceBodyList : BodyListBase
	{
		private const int MIN_FAST_X = -1000;
		private const int MAX_FAST_X = 1000;
		private const int MIN_FAST_Y = -1000;
		private const int MAX_FAST_Y = 1000;

		private const int MAX_FAST_CELL_BODY_ID = 9999;

		private readonly IList<Body> bodies = new List<Body>();
		private readonly IDictionary<long?, Body> bodyById = new Dictionary<long?, Body>();

		private readonly int[] fastCellXByBodyId = new int[MAX_FAST_CELL_BODY_ID + 1];
		private readonly int[] fastCellYByBodyId = new int[MAX_FAST_CELL_BODY_ID + 1];
		private readonly Point2D[] fastCellLeftTopByBodyId = new Point2D[MAX_FAST_CELL_BODY_ID + 1];
		private readonly Point2D[] fastCellRightBottomByBodyId = new Point2D[MAX_FAST_CELL_BODY_ID + 1];

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: private readonly Body[][][] bodiesByCellXY = new Body[MAX_FAST_X - MIN_FAST_X + 1][MAX_FAST_Y - MIN_FAST_Y + 1][];
		private readonly Body[][][] bodiesByCellXY = RectangularArrays.ReturnRectangularBodyArray(MAX_FAST_X - MIN_FAST_X + 1, MAX_FAST_Y - MIN_FAST_Y + 1, -1);
		private readonly IDictionary<IntPair, Body[]> bodiesByCell = new Dictionary<IntPair, Body[]>();
		private readonly IList<Body> cellExceedingBodies = new List<Body>();

		private double cellSize;
		private readonly double maxCellSize;

		public CellSpaceBodyList(double initialCellSize, double maxCellSize)
		{
			this.cellSize = initialCellSize;
			this.maxCellSize = maxCellSize;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void addBody(@Nonnull final com.codegame.codeseries.notreal2d.Body body)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
		public override void addBody(Body body)
		{
			validateBody(body);

			if (bodies.Contains(body))
			{
				throw new System.InvalidOperationException(body + " is already added.");
			}

			double radius = body.Form.CircumcircleRadius;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double diameter = 2.0D * radius;
			double diameter = 2.0D * radius;

			if (diameter > cellSize && diameter <= maxCellSize)
			{
				cellSize = diameter;
				rebuildIndexes();
			}

			bodies.Add(body);
			bodyById[body.Id] = body;
			addBodyToIndexes(body);

			body.CurrentState.registerPositionListener(new PositionListenerAdapterAnonymousInnerClassHelper(this, body, diameter), this.GetType().Name + "Listener");
		}

		private class PositionListenerAdapterAnonymousInnerClassHelper : PositionListenerAdapter
		{
			private readonly CellSpaceBodyList outerInstance;

			private Body body;
			private double diameter;

			public PositionListenerAdapterAnonymousInnerClassHelper(CellSpaceBodyList outerInstance, Body body, double diameter)
			{
				this.outerInstance = outerInstance;
				this.body = body;
				this.diameter = diameter;
			}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void afterChangePosition(@Nonnull com.codeforces.commons.geometry.Point2D oldPosition, @Nonnull com.codeforces.commons.geometry.Point2D newPosition)
			public override void afterChangePosition(Point2D oldPosition, Point2D newPosition)
			{
				if (diameter > outerInstance.cellSize)
				{
					return;
				}

				int oldCellX;
				int oldCellY;

				int newCellX;
				int newCellY;

				if (body.Id >= 0 && body.Id <= MAX_FAST_CELL_BODY_ID)
				{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("NumericCastThatLosesPrecision") int bodyId = (int) body.getId();
					int bodyId = (int) body.Id;
					Point2D cellLeftTop = outerInstance.fastCellLeftTopByBodyId[bodyId];
					Point2D cellRightBottom = outerInstance.fastCellRightBottomByBodyId[bodyId];

					Point2D position = body.Position;

					if (position.X >= cellLeftTop.X && position.Y >= cellLeftTop.Y && position.X < cellRightBottom.X && position.Y < cellRightBottom.Y)
					{
						return;
					}

					oldCellX = outerInstance.getCellX(oldPosition.X);
					oldCellY = outerInstance.getCellY(oldPosition.Y);

					newCellX = outerInstance.getCellX(newPosition.X);
					newCellY = outerInstance.getCellY(newPosition.Y);
				}
				else
				{
					oldCellX = outerInstance.getCellX(oldPosition.X);
					oldCellY = outerInstance.getCellY(oldPosition.Y);

					newCellX = outerInstance.getCellX(newPosition.X);
					newCellY = outerInstance.getCellY(newPosition.Y);

					if (oldCellX == newCellX && oldCellY == newCellY)
					{
						return;
					}
				}

				outerInstance.removeBodyFromIndexes(body, oldCellX, oldCellY);
				outerInstance.addBodyToIndexes(body, newCellX, newCellY);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void removeBody(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override void removeBody(Body body)
		{
			validateBody(body);

			if (bodyById.Remove(body.Id) == null)
			{
				throw new System.InvalidOperationException("Can't find " + body + '.');
			}

			bodies.Remove(body);
			removeBodyFromIndexes(body);
		}

		public override void removeBody(long id)
		{
            if (!bodyById.Keys.Contains(id))
			{
				throw new System.InvalidOperationException("Can't find Body {id=" + id + "}.");
			}

			Body body = bodyById[id];            			
			bodies.Remove(body);
			removeBodyFromIndexes(body);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void removeBodyQuietly(@Nullable com.codegame.codeseries.notreal2d.Body body)
		public override void removeBodyQuietly(Body body)
		{
			if (body == null)
			{
				return;
			}

			if (bodyById.Remove(body.Id) == null)
			{
				return;
			}

			bodies.Remove(body);
			removeBodyFromIndexes(body);
		}

		public override void removeBodyQuietly(long id)
		{
            if (!bodyById.Keys.Contains(id))
			{
				//throw new System.InvalidOperationException("Can't find Body {id=" + id + "}.");
                return;
			}

			Body body = bodyById[id];

			bodies.Remove(body);
			removeBodyFromIndexes(body);
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
			return bodyById.ContainsKey(id);
		}

		public override Body getBody(long id)
		{
			return bodyById[id];
		}

		public override IList<Body> Bodies
		{
			get
			{
				return bodies;
			}
		}

		/// <summary>
		/// May not find all potential intersections for bodies whose size exceeds cell size.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public List<com.codegame.codeseries.notreal2d.Body> getPotentialIntersections(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		public override IList<Body> getPotentialIntersections(Body body)
		{
			validateBody(body);

			if (!bodies.Contains(body))
			{
				throw new System.InvalidOperationException("Can't find " + body + '.');
			}

			IList<Body> potentialIntersections = new List<Body>();

			if (cellExceedingBodies.Count > 0)
			{
				foreach (Body otherBody in cellExceedingBodies)
				{
					addPotentialIntersection(body, otherBody, potentialIntersections);
				}
			}

			int cellX;
			int cellY;

			if (body.Id >= 0 && body.Id <= MAX_FAST_CELL_BODY_ID)
			{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("NumericCastThatLosesPrecision") int bodyId = (int) body.getId();
				int bodyId = (int) body.Id;
				cellX = fastCellXByBodyId[bodyId];
				cellY = fastCellYByBodyId[bodyId];
			}
			else
			{
				cellX = getCellX(body.X);
				cellY = getCellY(body.Y);
			}

			for (int cellOffsetX = -1; cellOffsetX <= 1; ++cellOffsetX)
			{
				for (int cellOffsetY = -1; cellOffsetY <= 1; ++cellOffsetY)
				{
					Body[] cellBodies = getCellBodies(cellX + cellOffsetX, cellY + cellOffsetY);
					addPotentialIntersections(body, cellBodies, potentialIntersections);
				}
			}

            return potentialIntersections;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private static void addPotentialIntersections(@Nonnull com.codegame.codeseries.notreal2d.Body body, @Nullable com.codegame.codeseries.notreal2d.Body[] bodies, @Nonnull List<com.codegame.codeseries.notreal2d.Body> potentialIntersections)
		private static void addPotentialIntersections(Body body, Body[] bodies, IList<Body> potentialIntersections)
		{
			if (bodies == null)
			{
				return;
			}

			for (int bodyIndex = 0, bodyCount = bodies.Length; bodyIndex < bodyCount; ++bodyIndex)
			{
				addPotentialIntersection(body, bodies[bodyIndex], potentialIntersections);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private static void addPotentialIntersection(@Nonnull com.codegame.codeseries.notreal2d.Body body, @Nonnull com.codegame.codeseries.notreal2d.Body otherBody, @Nonnull List<com.codegame.codeseries.notreal2d.Body> potentialIntersections)
		private static void addPotentialIntersection(Body body, Body otherBody, IList<Body> potentialIntersections)
		{
			if (otherBody.Equals(body))
			{
				return;
			}

			if (body.Static && otherBody.Static)
			{
				return;
			}

			if (Math.Pow(otherBody.Form.CircumcircleRadius + body.Form.CircumcircleRadius, 2) < otherBody.getSquaredDistanceTo(body))
			{
				return;
			}

			potentialIntersections.Add(otherBody);
		}

		private void rebuildIndexes()
		{
			for (int cellX = MIN_FAST_X; cellX <= MAX_FAST_X; ++cellX)
			{
				for (int cellY = MIN_FAST_Y; cellY <= MAX_FAST_Y; ++cellY)
				{
					bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y] = null;
				}
			}

			bodiesByCell.Clear();
			cellExceedingBodies.Clear();

			foreach (Body body in bodies)
			{
				addBodyToIndexes(body);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void addBodyToIndexes(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		private void addBodyToIndexes(Body body)
		{
			double radius = body.Form.CircumcircleRadius;
			double diameter = 2.0D * radius;

			if (diameter > cellSize)
			{
                cellExceedingBodies.Add(body);

                //if (!cellExceedingBodies.Add(body))
                //{
                //    throw new System.InvalidOperationException("Can't add Body {id=" + body.Id + "} to index.");
                //}
			}
			else
			{
				addBodyToIndexes(body, getCellX(body.X), getCellY(body.Y));
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void addBodyToIndexes(@Nonnull com.codegame.codeseries.notreal2d.Body body, int cellX, int cellY)
		private void addBodyToIndexes(Body body, int cellX, int cellY)
		{
			if (cellX >= MIN_FAST_X && cellX <= MAX_FAST_X && cellY >= MIN_FAST_Y && cellY <= MAX_FAST_Y)
			{
				Body[] cellBodies = bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y];
				cellBodies = addBodyToCell(cellBodies, body);
				bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y] = cellBodies;
			}
			else
			{
				IntPair cell = new IntPair(cellX, cellY);
				Body[] cellBodies = bodiesByCell[cell];
				cellBodies = addBodyToCell(cellBodies, body);
				bodiesByCell[cell] = cellBodies;
			}

			if (body.Id >= 0 && body.Id <= MAX_FAST_CELL_BODY_ID)
			{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("NumericCastThatLosesPrecision") int bodyId = (int) body.getId();
				int bodyId = (int) body.Id;
				fastCellXByBodyId[bodyId] = cellX;
				fastCellYByBodyId[bodyId] = cellY;
				fastCellLeftTopByBodyId[bodyId] = new Point2D(cellX * cellSize, cellY * cellSize);
				fastCellRightBottomByBodyId[bodyId] = fastCellLeftTopByBodyId[bodyId].copy().add(cellSize, cellSize);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void removeBodyFromIndexes(@Nonnull com.codegame.codeseries.notreal2d.Body body)
		private void removeBodyFromIndexes(Body body)
		{
			double radius = body.Form.CircumcircleRadius;
			double diameter = 2.0D * radius;

			if (diameter > cellSize)
			{
				if (!cellExceedingBodies.Remove(body))
				{
					throw new System.InvalidOperationException("Can't remove Body {id=" + body.Id + "} from index.");
				}
			}
			else
			{
				removeBodyFromIndexes(body, getCellX(body.X), getCellY(body.Y));
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void removeBodyFromIndexes(@Nonnull com.codegame.codeseries.notreal2d.Body body, int cellX, int cellY)
		private void removeBodyFromIndexes(Body body, int cellX, int cellY)
		{
			if (cellX >= MIN_FAST_X && cellX <= MAX_FAST_X && cellY >= MIN_FAST_Y && cellY <= MAX_FAST_Y)
			{
				Body[] cellBodies = bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y];
				cellBodies = removeBodyFromCell(cellBodies, body);
				bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y] = cellBodies;
			}
			else
			{
				IntPair cell = new IntPair(cellX, cellY);
				Body[] cellBodies = bodiesByCell[cell];
				cellBodies = removeBodyFromCell(cellBodies, body);

				if (cellBodies == null)
				{
					bodiesByCell.Remove(cell);
				}
				else
				{
					bodiesByCell[cell] = cellBodies;
				}
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nonnull private static com.codegame.codeseries.notreal2d.Body[] addBodyToCell(@Nullable com.codegame.codeseries.notreal2d.Body[] cellBodies, @Nonnull com.codegame.codeseries.notreal2d.Body body)
		private static Body[] addBodyToCell(Body[] cellBodies, Body body)
		{
			if (cellBodies == null)
			{
				return new Body[]{body};
			}

            if (!cellBodies.Contains(body))
            {
                throw new System.InvalidOperationException("Can't add Body {id=" + body.Id + "} to index.");
            }

			int bodyCount = cellBodies.Length;
			Body[] newCellBodies = new Body[bodyCount + 1];
			Array.Copy(cellBodies, 0, newCellBodies, 0, bodyCount);
			newCellBodies[bodyCount] = body;
			return newCellBodies;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable private static com.codegame.codeseries.notreal2d.Body[] removeBodyFromCell(@Nonnull com.codegame.codeseries.notreal2d.Body[] cellBodies, @Nonnull com.codegame.codeseries.notreal2d.Body body)
		private static Body[] removeBodyFromCell(Body[] cellBodies, Body body)
		{            
            if (!cellBodies.Contains(body))
            {
                throw new System.InvalidOperationException("Can't add Body {id=" + body.Id + "} to index.");
            }

            int bodyIndex = Array.IndexOf(cellBodies, body);

			int bodyCount = cellBodies.Length;
			if (bodyCount == 1)
			{
				return null;
			}

			Body[] newCellBodies = new Body[bodyCount - 1];
			Array.Copy(cellBodies, 0, newCellBodies, 0, bodyIndex);
			Array.Copy(cellBodies, bodyIndex + 1, newCellBodies, bodyIndex, bodyCount - bodyIndex - 1);
			return newCellBodies;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Nullable private com.codegame.codeseries.notreal2d.Body[] getCellBodies(int cellX, int cellY)
		private Body[] getCellBodies(int cellX, int cellY)
		{
			if (cellX >= MIN_FAST_X && cellX <= MAX_FAST_X && cellY >= MIN_FAST_Y && cellY <= MAX_FAST_Y)
			{
				return bodiesByCellXY[cellX - MIN_FAST_X][cellY - MIN_FAST_Y];
			}
			else
			{
				return bodiesByCell[new IntPair(cellX, cellY)];
			}
		}

		private int getCellX(double x)
		{
			return NumberUtil.toInt(Math.Floor(x / cellSize));
		}

		private int getCellY(double y)
		{
            return NumberUtil.toInt(Math.Floor(y / cellSize));
		}

//        private sealed class UnmodifiableCollectionWrapperList<E> : IList<E>
//        {
//            internal readonly ICollection<E> collection;

//            internal UnmodifiableCollectionWrapperList(ICollection<E> collection)
//            {
//                this.collection = collection;
//            }

//            public int Count
//            {
//                get
//                {
//                    return collection.Count;
//                }
//            }

//            public override bool Empty
//            {
//                get
//                {
//                    return collection.Count == 0;
//                }
//            }

//            public bool Contains(object @object)
//            {
//                return collection.Contains(@object);
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Nonnull @Override public Iterator<E> iterator()
//            public IEnumerator<E> GetEnumerator()
//            {
////JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
////ORIGINAL LINE: final Iterator<E> iterator = collection.iterator();
//                IEnumerator<E> iterator = collection.GetEnumerator();

//                return new UnmodifiableIteratorAnonymousInnerClassHelper(this, iterator);
//            }

//            private class UnmodifiableIteratorAnonymousInnerClassHelper : UnmodifiableIterator<E>
//            {
//                private readonly UnmodifiableCollectionWrapperList<E> outerInstance;

//                private IEnumerator<E> iterator;

//                public UnmodifiableIteratorAnonymousInnerClassHelper(UnmodifiableCollectionWrapperList<E> outerInstance, IEnumerator<E> iterator)
//                {
//                    this.outerInstance = outerInstance;
//                    this.iterator = iterator;
//                }

//                public override bool hasNext()
//                {
////JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
//                    return iterator.hasNext();
//                }

//                public override E next()
//                {
////JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
//                    return iterator.next();
//                }
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Nonnull @Override public Object[] toArray()
//            public override object[] toArray()
//            {
//                return collection.ToArray();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @SuppressWarnings("SuspiciousToArrayCall") @Nonnull @Override public <T> T[] toArray(@Nonnull T[] array)
//            public override T[] toArray<T>(T[] array)
//            {
//                return collection.toArray(array);
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean add(E element)
//            public override bool add(E element)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean remove(Object object)
//            public bool Remove(object @object)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Override public boolean containsAll(@Nonnull Collection<?> collection)
//            public override bool containsAll<T1>(ICollection<T1> collection)
//            {
////JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
//                return this.collection.containsAll(collection);
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean addAll(@Nonnull Collection<? extends E> collection)
////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean addAll(@Nonnull Collection<? extends E> collection)
////JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//            public override bool addAll<T1>(ICollection<T1> collection) where ? : E
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
//            Contract("_, _ -> fail") Override public boolean addAll(int index, Nonnull Collection<? extends E> collection)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean removeAll(@Nonnull Collection<?> collection)
////JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//            public bool removeAll(@Nonnull ICollection<?> collection)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public boolean retainAll(@Nonnull Collection<?> collection)
////JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//            public bool retainAll(@Nonnull ICollection<?> collection)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract(" -> fail") @Override public void clear()
//            public void clear()
//            {
//                throw new System.NotSupportedException();
//            }

//            public E get(int index)
//            {
//                if (collection is IList)
//                {
//                    return ((IList<E>) collection)[index];
//                }

//                if (index < 0 || index >= collection.Count)
//                {
//                    throw new System.IndexOutOfRangeException("Illegal index: " + index + ", size: " + collection.Count + '.');
//                }

//                IEnumerator<E> iterator = collection.GetEnumerator();

//                for (int i = 0; i < index; ++i)
//                {
////JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
//                    iterator.next();
//                }

////JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
//                return iterator.next();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_, _ -> fail") @Override public E set(int index, E element)
//            public E set(int index, E element)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_, _ -> fail") @Override public void add(int index, E element)
//            public void add(int index, E element)
//            {
//                throw new System.NotSupportedException();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Contract("_ -> fail") @Override public E remove(int index)
//            public E remove(int index)
//            {
//                throw new System.NotSupportedException();
//            }

//            public int indexOf(object o)
//            {
//                IEnumerator<E> iterator = collection.GetEnumerator();
//                int index = 0;

//                if (o == null)
//                {
//                    while (iterator.MoveNext())
//                    {
//                        if (iterator.Current == null)
//                        {
//                            return index;
//                        }
//                        ++index;
//                    }
//                }
//                else
//                {
//                    while (iterator.MoveNext())
//                    {
//                        if (o.Equals(iterator.Current))
//                        {
//                            return index;
//                        }
//                        ++index;
//                    }
//                }

//                return -1;
//            }

//            public int lastIndexOf(object o)
//            {
//                if (collection is IList)
//                {
//                    return ((IList) collection).lastIndexOf(o);
//                }

//                IEnumerator<E> iterator = collection.GetEnumerator();
//                int index = 0;
//                int lastIndex = -1;

//                if (o == null)
//                {
//                    while (iterator.MoveNext())
//                    {
//                        if (iterator.Current == null)
//                        {
//                            lastIndex = index;
//                        }
//                        ++index;
//                    }
//                }
//                else
//                {
//                    while (iterator.MoveNext())
//                    {
//                        if (o.Equals(iterator.Current))
//                        {
//                            lastIndex = index;
//                        }
//                        ++index;
//                    }
//                }

//                return lastIndex;
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Nonnull @Override public ListIterator<E> listIterator()
//            public IEnumerator<E> listIterator()
//            {
////JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
//                return collection is IList ? Collections.unmodifiableList((IList<E>) collection).GetEnumerator() : Collections.unmodifiableList(new List<>(collection)).GetEnumerator();
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Nonnull @Override public ListIterator<E> listIterator(int index)
//            public IEnumerator<E> listIterator(int index)
//            {
//                return collection is IList ? Collections.unmodifiableList((IList<E>) collection).listIterator(index) : Collections.unmodifiableList(new List<>(collection)).listIterator(index);
//            }

////JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
////ORIGINAL LINE: @Nonnull @Override public List<E> subList(int fromIndex, int toIndex)
//            public IList<E> subList(int fromIndex, int toIndex)
//            {
//                return collection is IList ? Collections.unmodifiableList(((IList<E>) collection).subList(fromIndex, toIndex)) : Collections.unmodifiableList(new List<>(collection)).subList(fromIndex, toIndex);
//            }
//        }
	}

}