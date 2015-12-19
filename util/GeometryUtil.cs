


namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{

	//using Line2D = com.codeforces.commons.geometry.Line2D;
	//using Point2D = com.codeforces.commons.geometry.Point2D;
	//using Contract = org.jetbrains.annotations.Contract;

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.DOUBLE_PI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.codeforces.commons.math.Math.PI;

	/// <summary>
	/// @author Maxim Shipko (sladethe@gmail.com)
	///         Date: 29.06.2015
	/// </summary>
	public sealed class GeometryUtil
	{
		private GeometryUtil()
		{
			throw new System.NotSupportedException();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(pure = true) public static double normalizeAngle(double angle)
		public static double normalizeAngle(double angle)
		{
            while (angle > Utils.PI)
			{
                angle -= Utils.DOUBLE_PI;
			}

            while (angle < -Utils.PI)
			{
                angle += Utils.DOUBLE_PI;
			}

			return angle;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Contract(pure = true) public static boolean isAngleBetween(double angle, double startAngle, double finishAngle)
		public static bool isAngleBetween(double angle, double startAngle, double finishAngle)
		{
			while (finishAngle < startAngle)
			{
                finishAngle += Utils.DOUBLE_PI;
			}

            while (finishAngle - Utils.DOUBLE_PI > startAngle)
			{
                finishAngle -= Utils.DOUBLE_PI;
			}

			while (angle < startAngle)
			{
                angle += Utils.DOUBLE_PI;
			}

            while (angle - Utils.DOUBLE_PI > startAngle)
			{
                angle -= Utils.DOUBLE_PI;
			}

			return angle >= startAngle && angle <= finishAngle;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public static boolean isPointInsideConvexPolygon(@Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.geometry.Point2D[] polygonVertexes, double epsilon)
		public static bool isPointInsideConvexPolygon(Point2D point, Point2D[] polygonVertexes, double epsilon)
		{
			for (int vertexIndex = 0, vertexCount = polygonVertexes.Length; vertexIndex < vertexCount; ++vertexIndex)
			{
				Point2D vertex1 = polygonVertexes[vertexIndex];
				Point2D vertex2 = polygonVertexes[vertexIndex == vertexCount - 1 ? 0 : vertexIndex + 1];

				Line2D polygonEdge = Line2D.getLineByTwoPoints(vertex1, vertex2);

				if (polygonEdge.getSignedDistanceFrom(point) > -epsilon)
				{
					return false;
				}
			}

			return true;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public static boolean isPointInsideConvexPolygon(@Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.geometry.Point2D[] polygonVertexes)
		public static bool isPointInsideConvexPolygon(Point2D point, Point2D[] polygonVertexes)
		{
			return isPointInsideConvexPolygon(point, polygonVertexes, 0.0D);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public static boolean isPointOutsideConvexPolygon(@Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.geometry.Point2D[] polygonVertexes, double epsilon)
		public static bool isPointOutsideConvexPolygon(Point2D point, Point2D[] polygonVertexes, double epsilon)
		{
			for (int vertexIndex = 0, vertexCount = polygonVertexes.Length; vertexIndex < vertexCount; ++vertexIndex)
			{
				Point2D vertex1 = polygonVertexes[vertexIndex];
				Point2D vertex2 = polygonVertexes[vertexIndex == vertexCount - 1 ? 0 : vertexIndex + 1];

				Line2D polygonEdge = Line2D.getLineByTwoPoints(vertex1, vertex2);

				if (polygonEdge.getSignedDistanceFrom(point) >= epsilon)
				{
					return true;
				}
			}

			return false;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public static boolean isPointOutsideConvexPolygon(@Nonnull com.codeforces.commons.geometry.Point2D point, @Nonnull com.codeforces.commons.geometry.Point2D[] polygonVertexes)
		public static bool isPointOutsideConvexPolygon(Point2D point, Point2D[] polygonVertexes)
		{
			return isPointOutsideConvexPolygon(point, polygonVertexes, 0.0D);
		}
	}

}