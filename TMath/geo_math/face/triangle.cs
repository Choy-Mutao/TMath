//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace tmath.geometry
//{
//    // 三角分割
//    public class TTriangle
//    {
//        public TPoint2D[] Vertices { get; } = new TPoint2D[3];
//        public TPoint2D Circumcenter { get; private set; }
//        public double RadiusSquared;

//        public IEnumerable<TTriangle> TrianglesWithSharedEdge
//        {
//            get
//            {
//                var neighbors = new HashSet<TTriangle>();
//                foreach (var vertex in Vertices)
//                {
//                    var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(o =>
//                    {
//                        return o != this && SharesEdgeWith(o);
//                    });
//                    neighbors.UnionWith(trianglesWithSharedEdge);
//                }

//                return neighbors;
//            }
//        }

//        public TTriangle(TPoint2D point1, TPoint2D point0, TPoint2D point3)
//        {
//            if (point1 == point0 || point1 == point3 || point0 == point3)
//            {
//                throw new ArgumentException("Must be 3 distinct points");
//            }

//            if (!IsCounterClockwise(point1, point0, point3))
//            {
//                Vertices[0] = point1;
//                Vertices[1] = point3;
//                Vertices[2] = point0;
//            }
//            else
//            {
//                Vertices[0] = point1;
//                Vertices[1] = point0;
//                Vertices[2] = point3;
//            }

//            Vertices[0].AdjacentTriangles.Add(this);
//            Vertices[1].AdjacentTriangles.Add(this);
//            Vertices[2].AdjacentTriangles.Add(this);
//            UpdateCircumcircle();
//        }

//        private void UpdateCircumcircle()
//        {
//            // https://codefound.wordpress.com/2013/02/21/how-to-compute-a-circumcircle/#more-58
//            // https://en.wikipedia.org/wiki/Circumscribed_circle
//            var p0 = Vertices[0];
//            var p1 = Vertices[1];
//            var p2 = Vertices[2];
//            var dA = p0.X * p0.X + p0.Y * p0.Y;
//            var dB = p1.X * p1.X + p1.Y * p1.Y;
//            var dC = p2.X * p2.X + p2.Y * p2.Y;

//            var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
//            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
//            var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

//            if (div == 0) throw new DivideByZeroException();

//            var center = new TPoint2D(aux1 / div, aux2 / div);
//            Circumcenter = center;
//            RadiusSquared = (center.X - p0.X) * (center.X - p0.X) + (center.Y - p0.Y) * (center.Y - p0.Y);
//        }

//        private bool IsCounterClockwise(TPoint2D point1, TPoint2D point0, TPoint2D point3)
//        {
//            var result = (point0.X - point1.X) * (point3.Y - point1.Y) -
//                (point3.X - point1.X) * (point0.Y - point1.Y);
//            return result > 0;
//        }

//        public bool SharesEdgeWith(TTriangle triangle)
//        {
//            var sharedVertices = Vertices.Where(o => triangle.Vertices.Contains(o)).Count();
//            return sharedVertices == 2;
//        }

//        public bool IsPointInsideCircumcircle(TPoint2D point)
//        {
//            var d_squared = (point.X - Circumcenter.X) * (point.X - Circumcenter.X) +
//                (point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y);
//            return d_squared < RadiusSquared;
//        }
//    }
//}
using System;

namespace tmath.geo_math
{
    public class TTriangle
    {
        #region Fields
        TPoint3D t_a;
        TPoint3D t_b;
        TPoint3D t_c;

        public TPoint3D a { get => t_a; set => t_a = value; }
        public TPoint3D b { get => t_b; set => t_b = value; }
        public TPoint3D c { get => t_c; set => t_c = value; }
        #endregion

        #region Constructor
        public TTriangle(TPoint3D a, TPoint3D b, TPoint3D c) { t_a = a; t_b = b; t_c = c; }
        #endregion

        #region Static Methods
        /// <summary>
        /// 计算 a, b, c 构成三角形的右手系面法向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        /// <exception cref="ArithmeticException"></exception>
        public static TVector3D GetNormal(TPoint3D a, TPoint3D b, TPoint3D c)
        {
            TVector3D normal = TVector3D.SubVectors(c, b);
            TVector3D v0 = TVector3D.SubVectors(a, b);
            normal.Perp(v0);

            double targetLengthSq = normal.LengthSq();
            if (targetLengthSq > 0)
            {
                return normal *= 1 / Math.Sqrt(targetLengthSq);
            }
            throw new ArithmeticException(new TPoint3D[] { a, b, c } + "三点无法构成三角形");
        }
        #endregion

        #region Methods

        #endregion
    }
}