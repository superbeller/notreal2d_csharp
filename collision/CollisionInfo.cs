
using System;
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

    //using Point2D = com.codeforces.commons.geometry.Point2D;
    //using Vector2D = com.codeforces.commons.geometry.Vector2D;
    //using StringUtil = com.codeforces.commons.text.StringUtil;
    //using Logger = org.apache.log4j.Logger;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 08.06.2015
	/// </summary>
	public class CollisionInfo
	{
		//private static readonly Logger logger = Logger.getLogger(typeof(CollisionInfo));

		private readonly Body bodyA;
		private readonly Body bodyB;
		private readonly Point2D point;
		private readonly Vector2D normalB;
		private readonly double depth;

		public CollisionInfo(Body bodyA, Body bodyB, Point2D point, Vector2D normalB, double depth, double epsilon)
		{
			this.bodyA = bodyA;
			this.bodyB = bodyB;
			this.point = point;
			this.normalB = normalB;

			if (depth < 0.0D && depth > -epsilon)
			{
				this.depth = 0.0D;
			}
			else
			{
				this.depth = depth;
			}

			if (double.IsNaN(this.depth) || double.IsInfinity(this.depth) || this.depth < 0.0D)
			{
                throw new ArgumentException(string.Format("Argument 'depth' should be non-negative number but got {0} ({1} and {2}).", this.depth, bodyA, bodyB));				
			}
		}

		public virtual Body BodyA
		{
			get
			{
				return bodyA;
			}
		}

		public virtual Body BodyB
		{
			get
			{
				return bodyB;
			}
		}

		public virtual Point2D Point
		{
			get
			{
				return point.copy();
			}
		}

		public virtual Vector2D NormalB
		{
			get
			{
				return normalB.copy();
			}
		}

		public virtual double Depth
		{
			get
			{
				return depth;
			}
		}

        //public override string ToString()
        //{
        //    return StringUtil.ToString(this, false);
        //}
	}

}