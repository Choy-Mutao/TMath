using System;
using System.Collections.Generic;
using System.Linq;
using tmath.GeometryUtil;

namespace tmath.geometryutils
{
    /// <summary>
    /// NFP 中的所有计算数的单位换算为单位为mm的整数型数据
    /// </summary>
    public class NFPPoint
    {
        #region Fields
        public double X;
        public double Y;
        public bool marked { get; set; }
        public int id { get; set; }
        public double rotation { get; set; }
        #endregion

        #region Constructor
        public NFPPoint(double _x, double _y)
        {
            X = _x; Y = _y;
            marked = false;
            id = -1;
            rotation = 0;
        }

        public NFPPoint(NFPPoint src)
        {
            X = src.X; Y = src.Y;
            marked = src.marked;
            id = src.id;
            rotation = src.rotation;
        }

        public NFPPoint(TPoint2D src)
        {
            X = src.X; Y = src.Y;
            marked = false;
            id = -1;
            rotation = 0;
        }

        #endregion

        #region Operators
        public static bool operator ==(NFPPoint p0, NFPPoint p1) => (p0.X == p1.X && p0.Y == p1.Y);

        public static bool operator !=(NFPPoint p0, NFPPoint p1) => (p0.X != p1.X || p0.Y != p1.Y);
        #endregion

        #region Methods
        public double DistanceTo(NFPPoint p) => Math.Sqrt(X * p.X + Y * p.Y);

        public TPoint2D ToPoint2d() => new TPoint2D(X, Y);

        public double Dot(NFPPoint P) => X * P.X + Y * P.Y;
        public double Dot(NFPVector V) => X * V.X + Y * V.Y;
        #endregion
    }

    public class NFPVector
    {
        #region Fields
        public double X;
        public double Y;
        public NFPPoint start { get; set; }

        public NFPPoint end { get; set; }
        #endregion

        #region Constructor
        public NFPVector(double x, double y, NFPPoint s, NFPPoint e)
        {
            X = x; Y = y;
            start = s; end = e;
        }

        public NFPVector(NFPPoint s, NFPPoint e)
        {
            X = e.X - s.X;
            Y = e.Y - s.Y;
            start = s; end = e;
        }

        public NFPVector(double x, double y)
        {
            X = x; Y = y;
            start = new NFPPoint(0, 0);
            end = new NFPPoint(x, y);
        }

        public NFPVector(NFPVector normal)
        {
            X = normal.X; Y = normal.Y;
            start = new NFPPoint(0, 0);
            end = new NFPPoint(normal.X, normal.Y);
        }
        #endregion

        #region Methods
        public double Length() => Math.Sqrt(X * X + Y * Y);

        public void Normalize()
        {
            var l = Length(); X /= l; Y /= l;
        }

        public double Dot(NFPVector prevector) => X * prevector.X + Y * prevector.Y;

        #endregion
    }

    public class TreeNode<T> : List<T>
    {
        public TreeNode() : base()
        { }

        public TreeNode(TreeNode<T> values) : base(values)
        {
            offsetx = values.offsetx;
            offsety = values.offsety;
        }

        public double offsetx { get; set; } = 0;
        public double offsety { get; set; } = 0;

    }

    public class NFP_Key
    {
        public int A { get; set; } // store the id to minum memory
        public int B { get; set; } // store the id to minum memory
        public bool Inside { get; set; }
        public double Arotation { get; set; }
        public double Brotation { get; set; }
        public NFP_Key()
        {
            A = -1; B = -1; Inside = false; Arotation = 0; Brotation = 0;
        }
        public NFP_Key(int a, int b, bool inside, double arotation, double brotation)
        {
            A = a;
            B = b;
            Inside = inside;
            Arotation = arotation;
            Brotation = brotation;
        }
    }


    public class NFP_Value
    {
        public NFP_Key Key { get; set; }
        public List<TreeNode<NFPPoint>> Value { get; set; }
        public NFP_Value(NFP_Key key, List<TreeNode<NFPPoint>> value)
        {
            Key = key;
            Value = value;
        }
    }

    public class NFP_Pair
    {
        public NFP_Pair(TreeNode<NFPPoint> bin, TreeNode<NFPPoint> part, NFP_Key key)
        {
            A = bin;
            B = part;
            Key = key;
        }
        public TreeNode<NFPPoint> A { get; set; }
        public TreeNode<NFPPoint> B { get; set; }
        public NFP_Key Key { get; set; }
    }
    /// <summary>
    /// The util kit for no fit polygons
    /// </summary>
    public static class NFPUtil
    {
        static double TOL = 1e-4;

        struct TouchType
        {
            public int type { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public TouchType(int t, int a, int b)
            {
                type = t;
                A = a;
                B = b;
            }
        }

        public static bool GetPolygonBounds(List<NFPPoint> polygon, out Bounds bounds)
        {
            bounds = new Bounds();
            if (polygon.Count < 3)
                return false;

            var xmin = polygon[0].X;
            var xmax = polygon[0].X;
            var ymin = polygon[0].Y;
            var ymax = polygon[0].Y;

            for (var i = 1; i < polygon.Count; i++)
            {
                if (polygon[i].X > xmax)
                {
                    xmax = polygon[i].X;
                }
                else if (polygon[i].X < xmin)
                {
                    xmin = polygon[i].X;
                }

                if (polygon[i].Y > ymax)
                {
                    ymax = polygon[i].Y;
                }
                else if (polygon[i].Y < ymin)
                {
                    ymin = polygon[i].Y;
                }
            }
            bounds.X = xmin;
            bounds.Y = ymin;
            bounds.Width = xmax - xmin;
            bounds.Height = ymax - ymin;

            return true;
        }
        /// <summary>
        /// return true if point is in the polygon, false if outside, and null if exactly on a point or edge
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool? PointInPolygon(TPoint2D point, TreeNode<NFPPoint> polygon)
        {
            if (polygon.Count < 3)
            {
                return null;
            }

            var inside = false;
            var offsetx = polygon.offsetx;
            var offsety = polygon.offsety;

            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                var xi = polygon[i].X + offsetx;
                var yi = polygon[i].Y + offsety;
                var xj = polygon[j].X + offsetx;
                var yj = polygon[j].Y + offsety;

                if (CommonUtil.AlmostEqual(xi, point.X, TOL) && CommonUtil.AlmostEqual(yi, point.Y, TOL))
                {
                    return null; // no result
                }

                if (CommonUtil.OnSegment(new TPoint2D(xi, yi), new TPoint2D(xj, yj), point))
                {
                    return false; // exactly on the segment
                }

                if (CommonUtil.AlmostEqual(xi, xj, TOL) && CommonUtil.AlmostEqual(yi, yj, TOL))
                { // ignore very small lines
                    continue;
                }

                var intersect = ((yi > point.Y) != (yj > point.Y)) && (point.X < (xj - xi) * (point.Y - yi) / (yj - yi) + xi);
                if (intersect) inside = !inside;
            }

            return inside;
        }

        /// <summary>
        /// clockwise is positive;
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static double PolygonArea(List<NFPPoint> polygon)
        {
            double area = 0;

            int i, j;
            for (i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                area += (polygon[j].X + polygon[i].X) * (polygon[j].Y - polygon[i].Y);
            }
            return 0.5 * area;
        }

        /// <summary>
        /// TODO: swap this for a more efficient sweep-line implementation
        /// return Edges: if SetByRowElements, return all edges on A that have intersections
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool Intersect(in TreeNode<NFPPoint> A, in TreeNode<NFPPoint> B)
        {

            var Aoffsetx = A.offsetx;
            var Aoffsety = A.offsety;

            var Boffsetx = B.offsetx;
            var Boffsety = B.offsety;

            //A = A.slice(0);
            //B = B.slice(0);

            for (int i = 0; i < A.Count - 1; i++)
            {
                for (int j = 0; j < B.Count - 1; j++)
                {
                    NFPPoint a1 = new NFPPoint(A[i].X + Aoffsetx, A[i].Y + Aoffsety),
                        a2 = new NFPPoint(A[i + 1].X + Aoffsetx, A[i + 1].Y + Aoffsety),
                        b1 = new NFPPoint(B[j].X + Boffsetx, B[j].Y + Boffsety),
                        b2 = new NFPPoint(B[j + 1].X + Boffsetx, B[j + 1].Y + Boffsety);

                    var prevbindex = (j == 0) ? B.Count - 1 : j - 1;
                    var prevaindex = (i == 0) ? A.Count - 1 : i - 1;
                    var nextbindex = (j + 1 == B.Count - 1) ? 0 : j + 2;
                    var nextaindex = (i + 1 == A.Count - 1) ? 0 : i + 2;

                    // go even further back if we happen to hit on a loop end point
                    if (B[prevbindex] == B[j] || (CommonUtil.AlmostEqual(B[prevbindex].X, B[j].X, TOL) && CommonUtil.AlmostEqual(B[prevbindex].Y, B[j].Y, TOL)))
                    {
                        prevbindex = (prevbindex == 0) ? B.Count - 1 : prevbindex - 1;
                    }

                    if (A[prevaindex] == A[i] || (CommonUtil.AlmostEqual(A[prevaindex].X, A[i].X, TOL) && CommonUtil.AlmostEqual(A[prevaindex].Y, A[i].Y, TOL)))
                    {
                        prevaindex = (prevaindex == 0) ? A.Count - 1 : prevaindex - 1;
                    }

                    // go even further forward if we happen to hit on a loop end point
                    if (B[nextbindex] == B[j + 1] || (CommonUtil.AlmostEqual(B[nextbindex].X, B[j + 1].X, TOL) && CommonUtil.AlmostEqual(B[nextbindex].Y, B[j + 1].Y, TOL)))
                    {
                        nextbindex = (nextbindex == B.Count - 1) ? 0 : nextbindex + 1;
                    }

                    if (A[nextaindex] == A[i + 1] || (CommonUtil.AlmostEqual(A[nextaindex].X, A[i + 1].X, TOL) && CommonUtil.AlmostEqual(A[nextaindex].Y, A[i + 1].Y, TOL)))
                    {
                        nextaindex = (nextaindex == A.Count - 1) ? 0 : nextaindex + 1;
                    }

                    NFPPoint a0 = new NFPPoint(A[prevaindex].X + Aoffsetx, A[prevaindex].Y + Aoffsety),
                        b0 = new NFPPoint(B[prevbindex].X + Boffsetx, B[prevbindex].Y + Boffsety),
                        a3 = new NFPPoint(A[nextaindex].X + Aoffsetx, A[nextaindex].Y + Aoffsety),
                        b3 = new NFPPoint(B[nextbindex].X + Boffsetx, B[nextbindex].Y + Boffsety);


                    if (CommonUtil.OnSegment(a1.ToPoint2d(), a2.ToPoint2d(), b1.ToPoint2d()) || (CommonUtil.AlmostEqual(a1.X, b1.X, TOL) && CommonUtil.AlmostEqual(a1.Y, b1.Y, TOL)))
                    {
                        // if a point is on a segment, it could intersect or it could not. Check via the neighboring points
                        var b0in = PointInPolygon(b0.ToPoint2d(), A);
                        var b2in = PointInPolygon(b2.ToPoint2d(), A);

                        // todo: b0in, b2in maybe null
                        if ((b0in == true && b2in == false) || (b0in == false && b2in == true))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (CommonUtil.OnSegment(a1.ToPoint2d(), a2.ToPoint2d(), b2.ToPoint2d()) || (CommonUtil.AlmostEqual(a2.X, b2.X, TOL) && CommonUtil.AlmostEqual(a2.Y, b2.Y, TOL)))
                    {
                        // if a point is on a segment, it could intersect or it could not. Check via the neighboring points
                        var b1in = PointInPolygon(b1.ToPoint2d(), A);
                        var b3in = PointInPolygon(b3.ToPoint2d(), A);

                        // todo: b0in, b2in maybe null
                        if ((b1in == true && b3in == false) || (b1in == false && b3in == true))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (CommonUtil.OnSegment(b1.ToPoint2d(), b2.ToPoint2d(), a1.ToPoint2d()) || (CommonUtil.AlmostEqual(a1.X, b2.X, TOL) && CommonUtil.AlmostEqual(a1.Y, b2.Y, TOL)))
                    {
                        // if a point is on a segment, it could intersect or it could not. Check via the neighboring points
                        var a0in = PointInPolygon(a0.ToPoint2d(), B);
                        var a2in = PointInPolygon(a2.ToPoint2d(), B);

                        // todo: b0in, b2in maybe null
                        if ((a0in == true && a2in == false) || (a0in == false && a2in == true))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (CommonUtil.OnSegment(b1.ToPoint2d(), b2.ToPoint2d(), a2.ToPoint2d()) || (CommonUtil.AlmostEqual(a2.X, b1.X, TOL) && CommonUtil.AlmostEqual(a2.Y, b1.Y, TOL)))
                    {
                        // if a point is on a segment, it could intersect or it could not. Check via the neighboring points
                        var a1in = PointInPolygon(a1.ToPoint2d(), B);
                        var a3in = PointInPolygon(a3.ToPoint2d(), B);

                        if ((a1in == true && a3in == false) || (a1in == false && a3in == true))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    var p = LineIntersect(b1, b2, a1, a2);

                    if (!(p is null))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// returns a continuous polyline representing the normal-most edge of the given polygon
        /// eg. a normal vector of [-1,0] will return the left-most edge of the polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="normal"></param>
        public static TreeNode<NFPPoint> PolygonEdge(in TreeNode<NFPPoint> polygon, in NFPVector _normal)
        {
            if (polygon is null || polygon.Count < 3) return null;

            NFPVector normal = new NFPVector(_normal);
            normal.Normalize();

            // ccw tangent direciton
            NFPVector direction = new NFPVector(-normal.Y, normal.X);

            // find the max and min points, they will be the endpoints of our edge
            double min = double.MaxValue;
            double max = double.MinValue;

            List<double> dotproduct = new List<double>();

            for (int i = 0; i < polygon.Count; i++)
            {
                double dot = polygon[i].X * direction.X + polygon[i].Y * direction.Y;
                dotproduct.Add(dot);
                if (dot < min) min = dot;
                if (dot > max) max = dot;
            }

            // there may be multiple vertices with min/max values. In which case we choose the one that is normal-most
            int indexmin = 0, indexmax = 0;
            double normalmin = double.MinValue;
            double normalmax = double.MinValue;

            for (int i = 0; i < polygon.Count; i++)
            {
                if (CommonUtil.AlmostEqual(dotproduct[i], min, TOL))
                {
                    double dot = polygon[i].X * normal.X + polygon[i].Y * normal.Y;
                    //TODO: disputed
                    if (dot > normalmin)
                    {
                        normalmin = dot;
                        indexmin = i;
                    }
                }
                else if (CommonUtil.AlmostEqual(dotproduct[i], max, TOL))
                {
                    double dot = polygon[i].X * normal.X + polygon[i].Y * normal.Y;
                    if (dot > normalmax)
                    {
                        normalmax = dot;
                        indexmax = i;
                    }
                }
            }

            // now we have two edges bound by min and max points, figure out which edge faces our direction vector
            int indexleft = indexmin - 1;
            int indexright = indexmin + 1;

            // tips: polygon[0] != polygon[last]
            if (indexleft < 0)
            {
                indexleft = polygon.Count - 1;
            }
            if (indexright >= polygon.Count)
            {
                indexright = 0;
            }

            var minvertex = polygon[indexmin];
            var left = polygon[indexleft];
            var right = polygon[indexright];

            NFPVector leftvector = new NFPVector(left.X - minvertex.X, left.Y - minvertex.Y);
            NFPVector rightvector = new NFPVector(right.X - minvertex.X, right.Y - minvertex.Y);

            double dotleft = leftvector.X * direction.X + leftvector.Y * direction.Y;
            double dotright = rightvector.X * direction.X + rightvector.Y * direction.Y;

            // -1 = left, 1 = right
            int scandirection = -1;

            if (CommonUtil.AlmostEqual(dotleft, 0, TOL))
            {
                scandirection = 1;
            }
            else if (CommonUtil.AlmostEqual(dotright, 0, TOL))
            {
                scandirection = -1;
            }
            else
            {
                double normaldotleft = 0;
                double normaldotright = 0;

                if (CommonUtil.AlmostEqual(dotleft, dotright, TOL))
                {
                    // the points line up exactly along the normal vector
                    normaldotleft = leftvector.X * normal.X + leftvector.Y * normal.Y;
                    normaldotright = rightvector.X * normal.X + rightvector.Y * normal.Y;
                }
                else if (dotleft < dotright)
                {
                    // normalize right vertex so normal projection can be directly compared
                    normaldotleft = leftvector.X * normal.X + leftvector.Y * normal.Y;
                    normaldotright = (rightvector.X * normal.X + rightvector.Y * normal.Y) * (dotleft / dotright);
                }
                else
                {
                    // normalize left vertex so normal projection can be directly compared
                    normaldotleft = leftvector.X * normal.X + leftvector.Y * normal.Y * (dotright / dotleft);
                    normaldotright = rightvector.X * normal.X + rightvector.Y * normal.Y;
                }

                if (normaldotleft > normaldotright)
                {
                    scandirection = -1;
                }
                else
                {
                    // technically they could be equal, (ie. the segments bound by left and right points are incident) in which case we'll have to climb up the chain until lines are no longer incident for now we'll just not handle it and assume people aren't giving us garbage input..
                    scandirection = 1;
                }
            }


            // connect all points between indexmin and indexmax along the scan direction
            TreeNode<NFPPoint> edge = new TreeNode<NFPPoint>();
            int count = 0;
            int ei = indexmin;
            while (count < polygon.Count)
            {
                if (ei >= polygon.Count)
                {
                    ei = 0;
                }
                else if (ei < 0)
                {
                    ei = polygon.Count - 1;
                }

                edge.Add(polygon[ei]);

                if (ei == indexmax)
                {
                    break;
                }
                ei += scandirection;
                count++;
            }

            return edge;
        }

        /// <summary>
        /// returns the normal distance from p to a line segment defined by [s1, s2], this is bisically algorithm generalized for any vector direction
        /// eg. normal of [-1,0] returns the horizontal distance between the point and the line segment
        /// sx_inclusive: if true, include endpoints instead of excluding them
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="_normal"></param>
        /// <param name="s1inclusive"></param>
        /// <param name="s2inclusive"></param>
        public static double PointLineDistance(NFPPoint p, NFPPoint s1, NFPPoint s2, NFPVector _normal, bool s1inclusive, bool s2inclusive)
        {
            NFPVector normal = new NFPVector(_normal); normal.Normalize();

            // cw tangent direction
            NFPVector dir = new NFPVector(normal.Y, -normal.X);

            double pdot = p.X * dir.X + p.Y * dir.Y;
            double s1dot = s1.X * dir.X + s1.Y * dir.Y;
            double s2dot = s2.X * dir.X + s2.Y * dir.Y;

            double pdotnorm = p.X * normal.X + p.Y * normal.Y;
            double s1dotnorm = s1.X * normal.X + s1.Y * normal.Y;
            double s2dotnorm = s2.X * normal.X + s2.Y * normal.Y;

            // point is exactly along the edge in the normal direction
            if (CommonUtil.AlmostEqual(pdot, s1dot, TOL) && CommonUtil.AlmostEqual(pdot, s2dot, TOL))
            {

                // point lies on an endpoint
                if (CommonUtil.AlmostEqual(pdotnorm, s1dotnorm, TOL))
                {
                    return double.NaN;
                }

                if (CommonUtil.AlmostEqual(pdotnorm, s2dotnorm, TOL))
                {
                    return double.NaN;
                }

                // point is outside both endpoints
                if (pdotnorm > s1dotnorm && pdotnorm > s2dotnorm)
                {
                    return Math.Min(pdotnorm - s1dotnorm, pdotnorm - s2dotnorm);
                }
                if (pdotnorm < s1dotnorm && pdotnorm < s2dotnorm)
                {
                    return -Math.Min(s1dotnorm - pdotnorm, s2dotnorm - pdotnorm);
                }

                // point lies between endpoints
                var diff1 = pdotnorm - s1dotnorm;
                var diff2 = pdotnorm - s2dotnorm;
                if (diff1 > 0)
                {
                    return diff1;
                }
                else
                {
                    return diff2;
                }
            }
            // point
            else if (CommonUtil.AlmostEqual(pdot, s1dot, TOL))
            {
                if (s1inclusive) return pdotnorm - s1dotnorm;
                else return double.NaN;
            }
            else if (CommonUtil.AlmostEqual(pdot, s2dot, TOL))
            {
                if (s2inclusive) return pdotnorm - s2dotnorm;
                else return double.NaN;
            }
            else if ((pdot < s1dot && pdot < s2dot) || (pdot > s1dot && pdot > s2dot)) return double.NaN;

            return (pdotnorm - s1dotnorm + (s1dotnorm - s2dotnorm) * (s1dot - pdot) / (s1dot - s2dot));
        }

        /// <summary>
        /// return the distance defined by norma of point p to the setment [s1,s2]
        /// infinite: SetByRowElements true if the segment is infinite defined by two endpoints
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="normal"></param>
        /// <param name="infinite"></param>
        /// <returns></returns>
        public static double PointDistance(NFPPoint p, NFPPoint s1, NFPPoint s2, in NFPVector _normal, bool infinite = false)
        {
            NFPVector normal = new NFPVector(_normal); normal.Normalize();

            NFPVector dir = new NFPVector(normal.Y, -normal.X);

            double pdot = p.X * dir.X + p.Y * dir.Y;
            double s1dot = s1.X * dir.X + s1.Y * dir.Y;
            double s2dot = s2.X * dir.X + s2.Y * dir.Y;

            double pdotnorm = p.X * normal.X + p.Y * normal.Y;
            double s1dotnorm = s1.X * normal.X + s1.Y * normal.Y;
            double s2dotnorm = s2.X * normal.X + s2.Y * normal.Y;

            if (!infinite)
            {
                if (((pdot < s1dot || CommonUtil.AlmostEqual(pdot, s1dot, TOL)) && (pdot < s2dot || CommonUtil.AlmostEqual(pdot, s2dot, TOL))) || ((pdot > s1dot || CommonUtil.AlmostEqual(pdot, s1dot, TOL)) && (pdot > s2dot || CommonUtil.AlmostEqual(pdot, s2dot, TOL))))
                {
                    return double.NaN; // dot doesn't collide with segment, or lies directly on the vertex
                }
                if ((CommonUtil.AlmostEqual(pdot, s1dot, TOL) && CommonUtil.AlmostEqual(pdot, s2dot, TOL)) && (pdotnorm > s1dotnorm && pdotnorm > s2dotnorm))
                {
                    return Math.Min(pdotnorm - s1dotnorm, pdotnorm - s2dotnorm);
                }
                if ((CommonUtil.AlmostEqual(pdot, s1dot, TOL) && CommonUtil.AlmostEqual(pdot, s2dot, TOL)) && (pdotnorm < s1dotnorm && pdotnorm < s2dotnorm))
                {
                    return -Math.Min(s1dotnorm - pdotnorm, s2dotnorm - pdotnorm);
                }
            }

            return -(pdotnorm - s1dotnorm + (s1dotnorm - s2dotnorm) * (s1dot - pdot) / (s1dot - s2dot));
        }

        public static double SegmentDistance(NFPPoint A, NFPPoint B, NFPPoint E, NFPPoint F, NFPVector direction)
        {
            NFPVector normal = new NFPVector(direction.Y, -direction.X); // ccw - 外法向
            NFPVector reverse = new NFPVector(-direction.X, -direction.Y);

            double dotA = A.Dot(normal); // A.X * normal.X + A.Y * normal.Y;
            double dotB = B.Dot(normal); // B.X * normal.X + B.Y * normal.Y;
            double dotE = E.Dot(normal); // E.X * normal.X + E.Y * normal.Y;
            double dotF = F.Dot(normal); // F.X * normal.X + F.Y * normal.Y;

            double crossA = A.Dot(direction); // A.X * direction.X + A.Y * direction.Y;
            double crossB = B.Dot(direction); // B.X * direction.X + B.Y * direction.Y;
            double crossE = E.Dot(direction); // E.X * direction.X + E.Y * direction.Y;
            double crossF = F.Dot(direction); // F.X * direction.X + F.Y * direction.Y;

            //double crossABmin = Math.Min(crossA, crossB);
            //double crossABmax = Math.Max(crossA, crossB);

            //double crossEFmax = Math.Max(crossE, crossF);
            //double crossEFmin = Math.Min(crossE, crossF);

            double ABmin = Math.Min(dotA, dotB);
            double ABmax = Math.Max(dotA, dotB);

            double EFmax = Math.Max(dotE, dotF);
            double EFmin = Math.Min(dotE, dotF);

            // segments that will merely touch at one point
            if (CommonUtil.AlmostEqual(ABmax, EFmin, TOL) || CommonUtil.AlmostEqual(ABmin, EFmax, TOL))
            {
                return double.NaN;
            }
            // segments miss eachother completely
            if (ABmax < EFmin || ABmin > EFmax)
            {
                return double.NaN;
            }

            double overlap = -1;

            if ((ABmax > EFmax && ABmin < EFmin) || (EFmax > ABmax && EFmin < ABmin))
            {
                overlap = 1;
            }
            else
            {
                var minMax = Math.Min(ABmax, EFmax);
                var maxMin = Math.Max(ABmin, EFmin);

                var maxMax = Math.Max(ABmax, EFmax);
                var minMin = Math.Min(ABmin, EFmin);

                overlap = (minMax - maxMin) / (maxMax - minMin);
            }

            var crossABE = (E.Y - A.Y) * (B.X - A.X) - (E.X - A.X) * (B.Y - A.Y);
            var crossABF = (F.Y - A.Y) * (B.X - A.X) - (F.X - A.X) * (B.Y - A.Y);

            // lines are colinear
            if (CommonUtil.AlmostEqual(crossABE, 0, TOL) && CommonUtil.AlmostEqual(crossABF, 0, TOL))
            {

                NFPVector ABnorm = new NFPVector(B.Y - A.Y, A.X - B.X);
                NFPVector EFnorm = new NFPVector(F.Y - E.Y, E.X - F.X);

                ABnorm.Normalize();
                EFnorm.Normalize();

                // segment normals must point in opposite directions
                if (Math.Abs(ABnorm.Y * EFnorm.X - ABnorm.X * EFnorm.Y) < TOL && ABnorm.Y * EFnorm.Y + ABnorm.X * EFnorm.X < 0)
                {
                    // normal of AB segment must point in same direction as given direction vector
                    var normdot = ABnorm.Y * direction.Y + ABnorm.X * direction.X;
                    // the segments merely slide along eachother
                    if (CommonUtil.AlmostEqual(normdot, 0, TOL))
                    {
                        return double.NaN;
                    }
                    if (normdot < 0)
                    {
                        return double.NaN;
                    }
                }
                return double.NaN;
            }

            List<double> distances = new List<double>();

            // coincident points
            if (CommonUtil.AlmostEqual(dotA, dotE, TOL))
            {
                distances.Add(crossA - crossE);
            }
            else if (CommonUtil.AlmostEqual(dotA, dotF, TOL))
            {
                distances.Add(crossA - crossF);
            }
            else if (dotA > EFmin && dotA < EFmax)
            {
                double d = PointDistance(A, E, F, reverse);
                if (!double.IsNaN(d) && CommonUtil.AlmostEqual((double)d, 0, TOL))
                {
                    //  A currently touches EF, but AB is moving away from EF
                    var dB = PointDistance(B, E, F, reverse, true);
                    if (dB < 0 || CommonUtil.AlmostEqual((double)dB * overlap, 0, TOL))
                    {
                        d = double.NaN;
                    }
                }
                if (!double.IsNaN(d))
                {
                    distances.Add((double)d);
                }
            }


            if (CommonUtil.AlmostEqual(dotB, dotE, TOL))
            {
                distances.Add(crossB - crossE);
            }
            else if (CommonUtil.AlmostEqual(dotB, dotF, TOL))
            {
                distances.Add(crossB - crossF);
            }
            else if (dotB > EFmin && dotB < EFmax)
            {
                var d = PointDistance(B, E, F, reverse);

                if (!double.IsNaN(d) && CommonUtil.AlmostEqual((double)d, 0, TOL))
                {
                    // crossA>crossB A currently touches EF, but AB is moving away from EF
                    var dA = PointDistance(A, E, F, reverse, true);
                    if (dA < 0 || CommonUtil.AlmostEqual((double)dA * overlap, 0, TOL))
                        d = double.NaN;
                }
                if (!double.IsNaN(d))
                {
                    distances.Add((double)d);
                }
            }

            if (dotE > ABmin && dotE < ABmax)
            {
                var d = PointDistance(E, A, B, direction);
                if (!double.IsNaN(d) && CommonUtil.AlmostEqual((double)d, 0, TOL))
                {
                    // crossF < crossE A currently touches EF, but AB is moving away from EF
                    var dF = PointDistance(F, A, B, direction, true);
                    if (dF < 0 || CommonUtil.AlmostEqual((double)dF * overlap, 0, TOL))
                    {
                        d = double.NaN;
                    }
                }
                if (!double.IsNaN(d))
                {
                    distances.Add((double)d);
                }
            }

            if (dotF > ABmin && dotF < ABmax)
            {
                var d = PointDistance(F, A, B, direction);
                if (!double.IsNaN(d) && CommonUtil.AlmostEqual((double)d, 0, TOL))
                { // && crossE<crossF A currently touches EF, but AB is moving away from EF
                    var dE = PointDistance(E, A, B, direction, true);
                    if (dE < 0 || CommonUtil.AlmostEqual((double)dE * overlap, 0, TOL))
                    {
                        d = double.NaN;
                    }
                }
                if (!double.IsNaN(d))
                {
                    distances.Add((double)d);
                }
            }

            if (distances.Count == 0)
            {
                return double.NaN;
            }


            return distances.Min();
        }

        /// <summary>
        /// return the distance of two polygon A, B in direction
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="direction"></param>
        /// <param name="ignoreNegative"></param>
        /// <returns></returns>
        public static double PolygonSlideDistance(in TreeNode<NFPPoint> _A, in TreeNode<NFPPoint> _B, NFPVector direction, bool ignoreNegative = false)
        {
            TreeNode<NFPPoint> A = new TreeNode<NFPPoint>(_A);
            TreeNode<NFPPoint> B = new TreeNode<NFPPoint>(_B);

            double Aoffsetx = A.offsetx;
            double Aoffsety = A.offsety;

            double Boffsetx = B.offsetx;
            double Boffsety = B.offsety;

            // close the loop for polygons
            if (A[0] != A[A.Count - 1])
            {
                A.Add(A[0]);
            }

            if (B[0] != B[B.Count - 1])
            {
                B.Add(B[0]);
            }

            TreeNode<NFPPoint> edgeA = A;
            TreeNode<NFPPoint> edgeB = B;

            double distance = double.NaN;

            NFPVector dir = new NFPVector(direction.X, direction.Y);
            dir.Normalize();

            //NFPVector normal = new NFPVector(dir.Y, -dir.X); // right normal
            //NFPVector reverse = new NFPVector(-dir.X, -dir.Y); // negative vector


            for (var i = 0; i < edgeB.Count - 1; i++) // i for B
            {
                for (var j = 0; j < edgeA.Count - 1; j++) // j for A
                {
                    NFPPoint A1 = new NFPPoint(edgeA[j].X + Aoffsetx, edgeA[j].Y + Aoffsety);
                    NFPPoint A2 = new NFPPoint(edgeA[j + 1].X + Aoffsetx, edgeA[j + 1].Y + Aoffsety);

                    NFPPoint B1 = new NFPPoint(edgeB[i].X + Boffsetx, edgeB[i].Y + Boffsety);
                    NFPPoint B2 = new NFPPoint(edgeB[i + 1].X + Boffsetx, edgeB[i + 1].Y + Boffsety);

                    if ((CommonUtil.AlmostEqual(A1.X, A2.X, TOL) && CommonUtil.AlmostEqual(A1.Y, A2.Y, TOL)) || (CommonUtil.AlmostEqual(B1.X, B2.X, TOL) && CommonUtil.AlmostEqual(B1.Y, B2.Y, TOL)))
                    {
                        continue; // ignore extremely small lines
                    }

                    double d = SegmentDistance(A1, A2, B1, B2, dir);
                    if (!double.IsNaN(d) && (double.IsNaN(distance) || d < distance))
                    {
                        if (!ignoreNegative || d > 0 || CommonUtil.AlmostEqual((double)d, 0, TOL))
                        {
                            distance = d;
                        }
                    }
                }
            }
            return distance;
        }
        /// <summary>
        /// project each point of B onto A in the given direction, and return the shortest/most negative projection of B onto A
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="direction"></param>
        public static double PolygonProjectionDistance(TreeNode<NFPPoint> A, TreeNode<NFPPoint> B, NFPVector direction)
        {
            double Boffsetx = B.offsetx;
            double Boffsety = B.offsety;

            double Aoffsetx = A.offsetx;
            double Aoffsety = A.offsety;

            //A = A.slice(0);
            //B = B.slice(0);

            // close the loop for polygons
            if (A[0] != A[A.Count - 1])
            {
                A.Add(A[0]);
            }

            if (B[0] != B[B.Count - 1])
            {
                B.Add(B[0]);
            }

            var edgeA = A;
            var edgeB = B;

            double distance = double.NaN;


            for (var i = 0; i < edgeB.Count; i++)
            {
                // the shortest/most negative projection of B onto A
                double minprojection = double.NaN;
                NFPPoint minp = null;
                for (var j = 0; j < edgeA.Count - 1; j++)
                {
                    NFPPoint p = new NFPPoint(edgeB[i].X + Boffsetx, edgeB[i].Y + Boffsety);
                    NFPPoint s1 = new NFPPoint(edgeA[j].X + Aoffsetx, edgeA[j].Y + Aoffsety);
                    NFPPoint s2 = new NFPPoint(edgeA[j + 1].X + Aoffsetx, edgeA[j + 1].Y + Aoffsety);

                    if (Math.Abs((s2.Y - s1.Y) * direction.X - (s2.X - s1.X) * direction.Y) < TOL)
                    {
                        continue;
                    }

                    // project point, ignore edge boundaries
                    double d = PointDistance(p, s1, s2, direction);
                    if (!double.IsNaN(d) && (double.IsNaN(minprojection) || d < minprojection))
                    {
                        minprojection = d;
                        minp = p;
                    }
                }
                if (!double.IsNaN(minprojection) && (double.IsNaN(distance) || minprojection > distance))
                {
                    distance = minprojection;
                }
            }

            return distance;
        }
        /// <summary>
        /// searches for an arrangement of A and B such that they do not overlap if an NFP is given, only search for start points that have not already been traversed in the given NFP
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="inside"></param>
        /// <param name="nFPList"></param>
        /// <returns></returns>
        public static TPoint2D? SearchStartPoint(in TreeNode<NFPPoint> a, in TreeNode<NFPPoint> b, bool inside, List<TreeNode<NFPPoint>> NFPList = null)
        {
            // clone arrays;
            TreeNode<NFPPoint> A = new TreeNode<NFPPoint>();
            a.ForEach(_a => A.Add(new NFPPoint(_a)));
            TreeNode<NFPPoint> B = new TreeNode<NFPPoint>();
            b.ForEach(_b => B.Add(new NFPPoint(_b)));

            // close the loop for polygons
            if (A[0] != A[A.Count - 1]) A.Add(A[0]);
            if (B[0] != B[B.Count - 1]) B.Add(B[0]);


            for (int i = 0; i < A.Count - 1; i++)
            {
                if (!A[i].marked)
                {
                    A[i].marked = true;
                    for (var j = 0; j < B.Count; j++)
                    {
                        B.offsetx = A[i].X - B[j].X;
                        B.offsety = A[i].Y - B[j].Y;

                        object Binside = null;
                        for (var k = 0; k < B.Count; k++)
                        {
                            var inpoly = PointInPolygon(new TPoint2D(B[k].X + B.offsetx, B[k].Y + B.offsety), A);
                            if (inpoly != null)
                            {
                                Binside = inpoly;
                                break;
                            }
                        }

                        if (Binside == null)
                        { // A and B are the same
                            return null;
                        }

                        var startPoint = new NFPPoint(B.offsetx, B.offsety);
                        if ((((bool)Binside && inside) || (!(bool)Binside && !inside)) && !Intersect(A, B) && !InNFP(startPoint.ToPoint2d(), NFPList))
                        {
                            return startPoint.ToPoint2d();
                        }

                        // slide B along vector
                        var vx = A[i + 1].X - A[i].X;
                        var vy = A[i + 1].Y - A[i].Y;

                        var d1 = PolygonProjectionDistance(A, B, new NFPVector(vx, vy));
                        var d2 = PolygonProjectionDistance(B, A, new NFPVector(-vx, -vy));

                        double d = double.NaN;

                        // TODO: clean this up
                        if (double.IsNaN(d1) && double.IsNaN(d2))
                        {
                            // nothin
                        }
                        else if (double.IsNaN(d1))
                        {
                            d = d2;
                        }
                        else if (double.IsNaN(d2))
                        {
                            d = d1;
                        }
                        else
                        {
                            d = Math.Min((double)d1, (double)d2);
                        }

                        // only slide until no longer negative
                        // TODO: clean this up
                        if (!double.IsNaN(d) && !CommonUtil.AlmostEqual((double)d, 0, TOL) && d > 0)
                        {

                        }
                        else
                        {
                            continue;
                        }

                        var vd2 = vx * vx + vy * vy;

                        if (d * d < vd2 && !CommonUtil.AlmostEqual((double)d * (double)d, vd2, TOL))
                        {
                            var vd = Math.Sqrt(vx * vx + vy * vy);
                            vx *= (double)d / vd;
                            vy *= (double)d / vd;
                        }

                        B.offsetx += vx;
                        B.offsety += vy;

                        for (int k = 0; k < B.Count; k++)
                        {
                            var inpoly = PointInPolygon(new TPoint2D(B[k].X + B.offsetx, B[k].Y + B.offsety), A);
                            if (inpoly != null)
                            {
                                Binside = inpoly;
                                break;
                            }
                        }
                        startPoint = new NFPPoint(B.offsetx, B.offsety);
                        if ((((bool)Binside && inside) || (!(bool)Binside && !inside)) && !Intersect(A, B) && !InNFP(startPoint.ToPoint2d(), NFPList))
                        {
                            return startPoint.ToPoint2d();
                        }
                    }
                }
            }

            return null;
        }

        public static bool IsRectangle(List<NFPPoint> poly, double tolerance)
        {
            bool isb = GetPolygonBounds(poly, out Bounds bb);
            if (!isb || poly.Count < 4) return false;
            for (int i = 0; i < poly.Count; i++)
            {
                if (!CommonUtil.AlmostEqual(poly[i].X, bb.X, tolerance) && !CommonUtil.AlmostEqual(poly[i].X, bb.X + bb.Width, tolerance))
                {
                    return false;
                }
                if (!CommonUtil.AlmostEqual(poly[i].Y, bb.Y, TOL) && !CommonUtil.AlmostEqual(poly[i].Y, bb.Y + bb.Height, TOL))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// returns an interior NFP for the special case where A is a rectangle
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static List<TreeNode<NFPPoint>> NoFitPolygonRectangle(List<NFPPoint> A, List<NFPPoint> B)
        {
            var minAx = A[0].X;
            var minAy = A[0].Y;
            var maxAx = A[0].X;
            var maxAy = A[0].Y;

            for (var i = 1; i < A.Count; i++)
            {
                if (A[i].X < minAx)
                {
                    minAx = A[i].X;
                }
                if (A[i].Y < minAy)
                {
                    minAy = A[i].Y;
                }
                if (A[i].X > maxAx)
                {
                    maxAx = A[i].X;
                }
                if (A[i].Y > maxAy)
                {
                    maxAy = A[i].Y;
                }
            }

            var minBx = B[0].X;
            var minBy = B[0].Y;
            var maxBx = B[0].X;
            var maxBy = B[0].Y;
            for (int i = 1; i < B.Count; i++)
            {
                if (B[i].X < minBx)
                {
                    minBx = B[i].X;
                }
                if (B[i].Y < minBy)
                {
                    minBy = B[i].Y;
                }
                if (B[i].X > maxBx)
                {
                    maxBx = B[i].X;
                }
                if (B[i].Y > maxBy)
                {
                    maxBy = B[i].Y;
                }
            }

            if (maxBx - minBx > maxAx - minAx)
            {
                return null;
            }
            if (maxBy - minBy > maxAy - minAy)
            {
                return null;
            }


            return new List<TreeNode<NFPPoint>>() { new TreeNode<NFPPoint>()
            {
                new NFPPoint(minAx - minBx + B[0].X, minAy - minBy + B[0].Y),
                new NFPPoint(maxAx - maxBx + B[0].X, minAy - minBy + B[0].Y),
                new NFPPoint(maxAx - maxBx + B[0].X, maxAy - maxBy + B[0].Y),
                new NFPPoint(minAx - minBx + B[0].X, maxAy - maxBy + B[0].Y),
            } };

        }

        /// <summary>
        /// given a static polygon A and a movable polygon B, compute a no fit polygon by orbiting B about A
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="inside">if the inside flag is true, B is orbited inside of A rather than outside</param>
        /// <param name="searchEdges">if the searchEdges flag is true, all edges of A are expolred for NFPs - multiple</param>
        /// <returns></returns>
        public static List<TreeNode<NFPPoint>> NoFitPolygon(in TreeNode<NFPPoint> A, in TreeNode<NFPPoint> B, bool inside = false, bool searchEdges = false, double tolerance = 1e-4)
        {
            TOL = tolerance;

            if (A.Count < 3 || B.Count < 3) return null;

            A.offsetx = 0;
            A.offsety = 0;

            //int i = 0, j = 0; 
            double minA = A[0].Y;
            int minAindex = 0;

            double maxB = B[0].Y;
            int maxBindex = 0;

            for (int i = 1; i < A.Count; i++)
            {
                A[i].marked = false;
                if (A[i].Y < minA)
                {
                    minA = A[i].Y;
                    minAindex = i;
                }
            }

            for (int i = 1; i < B.Count; i++)
            {
                B[i].marked = false;
                if (B[i].Y > maxB)
                {
                    maxB = B[i].Y;
                    maxBindex = i;
                }
            }

            TPoint2D? startpoint = null;
            if (!inside)
            {
                // shift B such that the bottom-most point of B is at the top-most point of A. This guarantees an initial placement with no intersections
                startpoint = new TPoint2D(A[minAindex].X - B[maxBindex].X, A[minAindex].Y - B[maxBindex].Y);
            }
            else
            {
                // no reliable heuristic for inside
                startpoint = SearchStartPoint(A, B, true);
            }

            List<TreeNode<NFPPoint>> NFPList = new List<TreeNode<NFPPoint>>();

            while (!(startpoint is null))
            {
                B.offsetx = ((TPoint2D)startpoint).X;
                B.offsety = ((TPoint2D)startpoint).Y;

                // maintain a list of touching points/edges
                List<TouchType> touching;

                NFPVector prevector = null; // keep track of previous vector
                TreeNode<NFPPoint> NFP = new TreeNode<NFPPoint>() { new NFPPoint(B[0].X + B.offsetx, B[0].Y + B.offsety) };

                double referencex = B[0].X + B.offsetx;
                double referencey = B[0].Y + B.offsety;
                double startx = referencex;
                double starty = referencey;
                int counter = 0;

                while (counter < 10 * (A.Count + B.Count)) // sanity check prevent infinite loop
                {
                    touching = new List<TouchType>();
                    #region find touching vertices/edges
                    for (int i = 0; i < A.Count; i++)
                    {
                        int nexti = (i == A.Count - 1) ? 0 : i + 1;
                        for (int j = 0; j < B.Count; j++)
                        {
                            var nextj = (j == B.Count - 1) ? 0 : j + 1;
                            if (CommonUtil.AlmostEqual(A[i].X, B[j].X + B.offsetx, TOL) && CommonUtil.AlmostEqual(A[i].Y, B[j].Y + B.offsety))
                                touching.Add(new TouchType(0, i, j));
                            else if (CommonUtil.OnSegment(A[i].ToPoint2d(), A[nexti].ToPoint2d(), new TPoint2D(B[j].X + B.offsetx, B[j].Y + B.offsety)))
                            {
                                touching.Add(new TouchType(1, nexti, j));
                            }
                            else if (CommonUtil.OnSegment(new TPoint2D(B[j].X + B.offsetx, B[j].Y + B.offsety), new TPoint2D(B[nextj].X + B.offsetx, B[nextj].Y + B.offsety), A[i].ToPoint2d()))
                            {
                                touching.Add(new TouchType(2, i, nextj));
                            }

                        }
                    }
                    #endregion

                    #region generate translation vectors from touching vertices/edges
                    List<NFPVector> vectors = new List<NFPVector>();
                    for (int i = 0; i < touching.Count; i++)
                    {
                        #region adjacent A vertices
                        NFPPoint vertexA = A[touching[i].A];
                        vertexA.marked = true;

                        int prevAindex = touching[i].A - 1;
                        int nextAindex = touching[i].A + 1;

                        prevAindex = (prevAindex < 0) ? A.Count - 1 : prevAindex; // loop
                        nextAindex = (nextAindex >= A.Count) ? 0 : nextAindex; // loop

                        NFPPoint prevA = A[prevAindex];
                        NFPPoint nextA = A[nextAindex];
                        #endregion

                        #region adjacent B vertices

                        NFPPoint vertexB = B[touching[i].B];
                        int prevBindex = touching[i].B - 1;
                        int nextBindex = touching[i].B + 1;

                        prevBindex = (prevBindex < 0) ? B.Count - 1 : prevBindex; // loop
                        nextBindex = (nextBindex >= B.Count) ? 0 : nextBindex; // loop
                        NFPPoint prevB = B[prevBindex];
                        NFPPoint nextB = B[nextBindex];
                        #endregion

                        if (touching[i].type == 0)
                        {
                            NFPVector vA1 = new NFPVector(vertexA, prevA), vA2 = new NFPVector(vertexA, nextA);
                            // B vectors need to be inverted
                            NFPVector vB1 = new NFPVector(prevB, vertexB), vB2 = new NFPVector(nextB, vertexB);


                            vectors.Add(vA1);
                            vectors.Add(vA2);
                            vectors.Add(vB1);
                            vectors.Add(vB2);
                        }
                        else if (touching[i].type == 1)
                        {
                            //NFPVector vA1 = new NFPVector(prevA, vertexA), vA2 = new NFPVector(vertexA, prevA);
                            //vectors.Add(vA1);
                            //vectors.Add(vA2);
                            vectors.Add(new NFPVector(
                                vertexA.X - (vertexB.X + B.offsetx),
                                vertexA.Y - (vertexB.Y + B.offsety),
                                prevA,
                                vertexA
                                ));

                            vectors.Add(new NFPVector(
                                prevA.X - (vertexB.X + B.offsetx),
                                prevA.Y - (vertexB.Y + B.offsety),
                                vertexA,
                                prevA
                                ));
                        }
                        else if (touching[i].type == 2)
                        {
                            //NFPVector vB1 = new NFPVector(prevB, vertexB), vB2 = new NFPVector(vertexB, prevB);
                            //vectors.Add(vB1);
                            //vectors.Add(vB2);
                            vectors.Add(new NFPVector(
                                vertexA.X - (vertexB.X + B.offsetx),
                                vertexA.Y - (vertexB.Y + B.offsety),
                                prevB,
                                vertexB
                                ));

                            vectors.Add(new NFPVector(
                                vertexA.X - (prevB.X + B.offsetx),
                                vertexA.Y - (prevB.Y + B.offsety),
                                vertexB,
                                prevB
                                ));
                        }
                    }
                    #endregion

                    #region TODO: there should be a faster way to reject vectors that will cause immediate intersection. For now just check them all
                    NFPVector translate = null;
                    double maxd = 0;

                    for (int i = 0; i < vectors.Count; i++)
                    {
                        if (CommonUtil.AlmostEqual(vectors[i].Length(), 0, TOL)) continue;

                        // if this vector points us back to where we came from, ignore it 
                        // ie cross product = 0, dot product < 0
                        if (!(prevector is null) && vectors[i].Dot(prevector) < 0)
                        {
                            // compare magnitude with unit vectors
                            double vectorlength = vectors[i].Length();
                            TVector2D unitv = new TVector2D(vectors[i].X / vectorlength, vectors[i].Y / vectorlength);

                            double prevlength = prevector.Length();
                            TVector2D prevunit = new TVector2D(prevector.X / prevlength, prevector.Y / prevlength);

                            // we need to Scale down to unit vectors to normalize vector Length. Could also just do a tan here
                            if (Math.Abs(unitv.Y * prevunit.X - unitv.X * prevunit.Y) < 0.0001) continue;
                        }

                        double d = PolygonSlideDistance(A, B, vectors[i], true); //NOTE: test
                        double vecd2 = vectors[i].X * vectors[i].X + vectors[i].Y * vectors[i].Y;

                        if (double.IsNaN(d) || d * d > vecd2)
                            d = vectors[i].Length();

                        if (!(double.IsNaN(d)) && d > maxd)
                        {
                            maxd = d;
                            translate = vectors[i];
                        }

                    }

                    if (translate is null || CommonUtil.AlmostEqual(maxd, 0, TOL))
                    {
                        //NFP.Clear();
                        // throw new DataMisalignedException("didn't close the loop, something went wrong here");
                        break;
                    }

                    translate.start.marked = true;
                    translate.end.marked = true;

                    prevector = translate;

                    #region trim
                    double vlength2 = translate.X * translate.X + translate.Y * translate.Y;
                    if (maxd * maxd < vlength2 && !CommonUtil.AlmostEqual(maxd * maxd, vlength2, TOL))
                    {
                        double scale = Math.Sqrt((maxd * maxd) / vlength2);
                        translate.X *= scale;
                        translate.Y *= scale;
                    }
                    referencex += translate.X;
                    referencey += translate.Y;

                    if (CommonUtil.AlmostEqual(referencex, startx, TOL) && CommonUtil.AlmostEqual(referencey, starty, TOL))
                    {
                        // we've made a full loop
                        break;
                    }
                    #endregion

                    #region if A and B start on a touching horizontal line, the end point may not be the start point
                    bool looped = false;
                    if (NFP.Count > 0)
                    {
                        for (int j = 0; j < NFP.Count - 1; j++)
                        {
                            if (CommonUtil.AlmostEqual(referencex, NFP[j].X, TOL) && CommonUtil.AlmostEqual(referencey, NFP[j].Y, TOL)) looped = true;
                        }
                    }
                    #endregion

                    if (looped)
                    {
                        // we've made a full loop
                        break;
                    }

                    NFP.Add(new NFPPoint(referencex, referencey));
                    B.offsetx += translate.X;
                    B.offsety += translate.Y;

                    counter++;
                    #endregion
                }
                if (NFP != null && NFP.Count > 0)
                {
                    NFPList.Add(NFP);
                }
                if (!searchEdges)
                {
                    // only get outer NFP or first inner NFP
                    break;
                }

                startpoint = SearchStartPoint(A, B, inside, NFPList);
            }
            return NFPList;
        }

        /// <summary>
        ///  given two polygons that touch at least one point, but do not intersect. Return the outer perimeter of both polygons as a single continuous polygon
        ///  A and B mast have the same winding direction
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static TreeNode<NFPPoint> PolygonHull(in TreeNode<NFPPoint> _A, in TreeNode<NFPPoint> _B)
        {
            TreeNode<NFPPoint> A = _A;
            TreeNode<NFPPoint> B = _B;
            if (A is null || A.Count < 3 || B is null || B.Count < 3) return null;

            double Aoffsetx = A.offsetx;
            double Aoffsety = A.offsety;
            double Boffsetx = B.offsetx;
            double Boffsety = B.offsety;

            // start at an extreme point that is guaranteed to be on the final polygon
            double miny = A[0].Y;
            TreeNode<NFPPoint> startPolygon = A;
            int startIndex = 0;

            for (int i = 0; i < A.Count; i++)
            {
                if (A[i].Y + Aoffsety < miny)
                {
                    miny = A[i].Y + Aoffsety;
                    startPolygon = A;
                    startIndex = i;
                }
            }

            for (int i = 0; i < B.Count; i++)
            {
                if (B[i].Y + Boffsety < miny)
                {
                    miny = B[i].Y + Boffsety;
                    startPolygon = B;
                    startIndex = i;
                }
            }

            // for simplicity we'll define polygon A as the starting polygon
            if (startPolygon == B)
            {
                B = A;
                A = startPolygon;
                Aoffsetx = A.offsetx;
                Aoffsety = A.offsety;
                Boffsetx = B.offsetx;
                Boffsety = B.offsety;
            }

            //A = A.slice(0);
            //B = B.slice(0);

            TreeNode<NFPPoint> C = new TreeNode<NFPPoint>();
            int current = startIndex;
            int intercept1 = -1;
            int intercept2 = -1;

            // scan forward from the starting point
            for (int i = 0; i < A.Count + 1; i++)
            {
                current = (current == A.Count) ? 0 : current;
                var next = (current == A.Count - 1) ? 0 : current + 1;
                bool touching = false;
                for (int j = 0; j < B.Count; j++)
                {
                    int nextj = (j == B.Count - 1) ? 0 : j + 1;
                    if (CommonUtil.AlmostEqual(A[current].X + Aoffsetx, B[j].X + Boffsetx, TOL) && CommonUtil.AlmostEqual(A[current].Y + Aoffsety, B[j].Y + Boffsety, TOL))
                    {
                        C.Add(new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        intercept1 = j;
                        touching = true;
                        break;
                    }
                    else if (CommonUtil.OnSegment(new TPoint2D(A[current].X + Aoffsetx, A[current].Y + Aoffsety), new TPoint2D(A[next].X + Aoffsetx, A[next].Y + Aoffsety), new TPoint2D(B[j].X + Boffsetx, B[j].Y + Boffsety)))
                    {
                        C.Add(new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        C.Add(new NFPPoint(B[j].X + Boffsetx, B[j].Y + Boffsety));
                        intercept1 = j;
                        touching = true;
                        break;
                    }
                    else if (CommonUtil.OnSegment(new TPoint2D(B[j].X + Boffsetx, B[j].Y + Boffsety), new TPoint2D(B[nextj].X + Boffsetx, B[nextj].Y + Boffsety), new TPoint2D(A[current].X + Aoffsetx, A[current].Y + Aoffsety)))
                    {
                        C.Add(new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        C.Add(new NFPPoint(B[nextj].X + Boffsetx, B[nextj].Y + Boffsety));
                        intercept1 = nextj;
                        touching = true;
                        break;
                    }
                }
                if (touching) break;
                C.Add(new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                current++;
            }

            // scan backward from the starting point
            current = startIndex - 1;
            for (int i = 0; i < A.Count + 1; i++)
            {
                current = (current < 0) ? A.Count - 1 : current;
                var next = (current == 0) ? A.Count - 1 : current - 1;
                var touching = false;
                for (int j = 0; j < B.Count; j++)
                {
                    var nextj = (j == B.Count - 1) ? 0 : j + 1;
                    if (CommonUtil.AlmostEqual(A[current].X + Aoffsetx, B[j].X + Boffsetx, TOL) && CommonUtil.AlmostEqual(A[current].Y, B[j].Y + Boffsety, TOL))
                    {
                        C.Insert(0, new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        intercept2 = j;
                        touching = true;
                        break;
                    }
                    else if (CommonUtil.OnSegment(new TPoint2D(A[current].X + Aoffsetx, A[current].Y + Aoffsety), new TPoint2D(A[next].X + Aoffsetx, A[next].Y + Aoffsety), new TPoint2D(B[j].X + Boffsetx, B[j].Y + Boffsety)))
                    {
                        C.Insert(0, new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        C.Insert(0, new NFPPoint(B[j].X + Boffsetx, B[j].Y + Boffsety));
                        intercept2 = j;
                        touching = true;
                        break;
                    }
                    else if (CommonUtil.OnSegment(new TPoint2D(B[j].X + Boffsetx, B[j].Y + Boffsety), new TPoint2D(B[nextj].X + Boffsetx, B[nextj].Y + Boffsety), new TPoint2D(A[current].X + Aoffsetx, A[current].Y + Aoffsety)))
                    {
                        C.Insert(0, new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));
                        intercept2 = j;
                        touching = true;
                        break;
                    }
                }

                if (touching)
                {
                    break;
                }

                C.Insert(0, new NFPPoint(A[current].X + Aoffsetx, A[current].Y + Aoffsety));

                current--;
            }

            if (intercept1 == -1 || intercept2 == -1)
            {
                // polygons not touching?
                return null;
            }

            // the relevant points on B now lie between intercept1 and intercept2
            current = intercept1 + 1;
            for (int i = 0; i < B.Count; i++)
            {
                current = (current == B.Count) ? 0 : current;
                C.Add(new NFPPoint(B[current].X + Boffsetx, B[current].Y + Boffsety));

                if (current == intercept2)
                {
                    break;
                }

                current++;
            }

            // dedupe
            for (int i = 0; i < C.Count; i++)
            {
                var next = (i == C.Count - 1) ? 0 : i + 1;
                if (CommonUtil.AlmostEqual(C[i].X, C[next].X, TOL) && CommonUtil.AlmostEqual(C[i].Y, C[next].Y, TOL))
                {
                    C.RemoveAt(i);
                    i--;
                }
            }

            return C;

        }

        /// <summary>
        /// returns true if point already exists in the given nfp
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nfp"></param>
        /// <returns></returns>
        public static bool InNFP(TPoint2D p, List<TreeNode<NFPPoint>> nfp_list)
        {
            if (nfp_list is null || nfp_list.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < nfp_list.Count; i++)
            {
                for (var j = 0; j < nfp_list[i].Count; j++)
                {
                    if (CommonUtil.AlmostEqual(p.X, nfp_list[i][j].X, TOL) && CommonUtil.AlmostEqual(p.Y, nfp_list[i][j].Y, TOL))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// returns the intersection of AB and EF or null if there are no intersections or other numerical error if the infinite flag is SetByRowElements, AE and EF describe infinite lines without 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="E"></param>
        /// <param name="F"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static NFPPoint LineIntersect(NFPPoint A, NFPPoint B, NFPPoint E, NFPPoint F, bool infinite = false)
        {
            double a1, a2, b1, b2, c1, c2, x, y;

            a1 = B.Y - A.Y;
            b1 = A.X - B.X;
            c1 = B.X * A.Y - A.X * B.Y;
            a2 = F.Y - E.Y;
            b2 = E.X - F.X;
            c2 = F.X * E.Y - E.X * F.Y;

            double denom = a1 * b2 - a2 * b1;

            x = (b1 * c2 - b2 * c1) / denom;
            y = (a2 * c1 - a1 * c2) / denom;

            if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                return null;
            }

            // lines are colinear
            /*var crossABE = (I.Y - A.Y) * (B.X - A.X) - (I.X - A.X) * (B.Y - A.Y);
            var crossABF = (F.Y - A.Y) * (B.X - A.X) - (F.X - A.X) * (B.Y - A.Y);
            if(_almostEqual(crossABE,0) && _almostEqual(crossABF,0)){
                return null;
            }*/

            if (!infinite)
            {
                // coincident points do not count as intersecting
                if (Math.Abs(A.X - B.X) > TOL && ((A.X < B.X) ? x < A.X || x > B.X : x > A.X || x < B.X)) return null;
                if (Math.Abs(A.Y - B.Y) > TOL && ((A.Y < B.Y) ? y < A.Y || y > B.Y : y > A.Y || y < B.Y)) return null;

                if (Math.Abs(E.X - F.X) > TOL && ((E.X < F.X) ? x < E.X || x > F.X : x > E.X || x < F.X)) return null;
                if (Math.Abs(E.Y - F.Y) > TOL && ((E.Y < F.Y) ? y < E.Y || y > F.Y : y > E.Y || y < F.Y)) return null;
            }

            return new NFPPoint(x, y);


        }

    }
}
