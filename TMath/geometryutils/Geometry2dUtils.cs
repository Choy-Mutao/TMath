using System;
using System.Collections.Generic;
using System.Linq;
using tmath.geo_math.curve;

namespace tmath.GeometryUtil
{
    public struct Bounds
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    /// <summary>
    /// 通用计算工具
    /// </summary>
    public static class CommonUtil
    {
        public const int NONE = -1;
        public const double EPSILON = 1e-3;
        public const double PIE = Math.PI; /*3.1415926535897931;*/

        public static double perp(TPoint2D u, TPoint2D v)
        {
            return ((u).X * (v).Y - (u).Y * (v).X);  // perp product  (2D
        }
        public static double perp(TVector2D u, TVector2D v)
        {
            return ((u).X * (v).Y - (u).Y * (v).X);  // perp product  (2D
        }
        public static double dot(TPoint2D u, TPoint2D v)
        {
            return ((u).X * (v).X + (u).Y * (v).Y);
        }
        public static double dot(TVector2D u, TVector2D v)
        {
            return ((u).X * (v).X + (u).Y * (v).Y);
        }
        public static double norm2(TPoint2D v) => dot(v, v);  // norm2 = squared Length of vector
        public static double norm2(TVector2D v) => dot(v, v);  // norm2 = squared Length of vector
        public static double norm(TPoint2D v) => Math.Sqrt(norm2(v));  // norm = Length of  vector
        public static double norm(TVector2D v) => Math.Sqrt(norm2(v));  // norm = Length of  vector
        public static double distance(TPoint2D u, TPoint2D v) => norm(u - v); // distance = norm of difference

        public static bool AlmostEqual(double a, double b, double tolerance = EPSILON)
        {
            return Math.Abs(a - b) < tolerance;
        }
        //===================================================================

        //===================================================================

        // isEqualPoint: test if two points is equal
        //		Input: two points: p1 and p2;
        //		Outout: true is equal;
        //				false is not;
        public static bool IsEqualPoint(TPoint2D p1, TPoint2D p2, double tol = EPSILON)
        {
            TVector2D p = (p1 - p2).ToVector();
            return NumberUtil.CompValue(norm(p), 0, tol) == 0;
        }
        //===================================================================
        // isLeft(): test if a point is Left|On|Right of an infinite 2D line.
        //    Input:  three points SP, EP, and P
        //    Return:   >0 for P left of the line through SP to EP
        //              =0 for P on the line
        //              <0 for P right of the line through SP to EP
        public static int IsLeft(TPoint2D SP, TPoint2D EP, TPoint2D P)
        {
            double dir = ((EP.X - SP.X) * (P.Y - SP.Y) - (P.X - SP.X) * (EP.Y - SP.Y));
            if (dir > 0) return 1;
            else if (dir < 0) return -1;
            else return 0;
        }
        //===================================================================
        // intersect2D_2Segments(): find the 2D intersection of 2 finite segments
        //    Input:  two finite segments S1 and S2
        //    Output: *I0 = intersect point (when it exists)
        //            *I1 =  endpoint of intersect segment [I0,I1] (when it exists)
        //    Return: 0=disjoint (no intersect)
        //            1=intersect  in unique point I0
        //            2=overlap  in segment from I0 to I1
        public static int intersect2D_2Segments(TLineSegment2d S1, TLineSegment2d S2, ref TPoint2D I0, ref TPoint2D I1)
        {

            TVector2D u = (S1.EP - S1.SP).ToVector();
            TVector2D v = (S2.EP - S2.SP).ToVector();
            TVector2D w = (S1.SP - S2.SP).ToVector();
            double D = perp(u, v);
            // test if  they are parallel (includes either being a point)
            if (Math.Abs(D) < EPSILON)
            {           // S1 and S2 are parallel
                if (perp(u, w) != 0 || perp(v, w) != 0)
                {
                    return 0;                    // they are NOT collinear
                }
                // they are collinear or degenerate
                // check if they are degenerate  points
                double du = dot(u, u);
                double dv = dot(v, v);
                if (du == 0 && dv == 0)
                {            // both segments are points
                    if (S1.SP != S2.SP)         // they are distinct  points
                        return 0;
                    I0 = S1.SP;                 // they are the same point
                    return 1;
                }
                if (du == 0)
                {                     // S1 is a single point
                    if (inSegment(S1.SP, S2) == 0)  // but is not in S2
                        return 0;
                    I0 = S1.SP;
                    return 1;
                }
                if (dv == 0)
                {                     // S2 a single point
                    if (inSegment(S2.SP, S1) == 0)  // but is not in S1
                        return 0;
                    I0 = S2.SP;
                    return 1;
                }
                // they are collinear segments - get  overlap (or not)
                double t0, t1;                    // endpoints of S1 in eqn for S2
                TVector2D w2 = (S1.EP - S2.SP).ToVector();
                if (v.X != 0)
                {
                    t0 = w.X / v.X;
                    t1 = w2.X / v.X;
                }
                else
                {
                    t0 = w.Y / v.Y;
                    t1 = w2.Y / v.Y;
                }
                if (t0 > t1)
                {                   // must have t0 smaller than t1
                    double t = t0; t0 = t1; t1 = t;    // swap if not
                }
                if (t0 > 1 || t1 < 0)
                {
                    return 0;      // NO overlap
                }
                t0 = t0 < 0 ? 0 : t0;               // clip to min 0
                t1 = t1 > 1 ? 1 : t1;               // clip to max 1
                if (t0 == t1)
                {                  // intersect is a point
                    I0 = S2.SP + t0 * v;
                    return 1;
                }

                // they overlap in a valid subsegment
                I0 = S2.SP + t0 * v;
                I1 = S2.SP + t1 * v;
                return 2;
            }

            // the segments are skew and may intersect in a point
            // get the intersect parameter for S1
            double sI = perp(v, w) / D;
            if (sI < 0 || sI > 1)                // no intersect with S1
                return 0;
            // get the intersect parameter for S2
            double tI = perp(u, w) / D;
            if (tI < 0 || tI > 1)                // no intersect with S2
                return 0;
            I0 = S1.SP + sI * u;                // compute S1 intersect point
            return 1;
        }
        // 计算直线方程的三个 A, B, C
        public static double[] getLineEquationFromTsegment(TLineSegment2d segment)
        {
            decimal x1 = new decimal(segment.SP.X), y1 = new decimal(segment.SP.Y);
            decimal x2 = new decimal(segment.EP.X), y2 = new decimal(segment.EP.Y);
            double[] line = new double[3];
            line[0] = (double)decimal.Subtract(y2, y1);
            line[1] = (double)decimal.Subtract(x1, x2);
            line[2] = (double)decimal.Subtract(decimal.Multiply(y1, x2), decimal.Multiply(x1, y2));
            return line;
        }
        // intersect2D_2Lines(): find the 2D intersection of 2 infinite lines
        public static TPoint2D? Intersect2D_2Lines(TLineSegment2d L1, TLineSegment2d L2)
        {
            double[] line1 = getLineEquationFromTsegment(L1), line2 = getLineEquationFromTsegment(L2);

            // 两直线平行，没有交点
            if (line1[0].CompareTo(0.0) == 0 && line2[0].CompareTo(0.0) == 0)
            {
                return null;
            }
            if (line1[0].CompareTo(0.0) != 0 && line2[0].CompareTo(0.0) != 0)
            {
                decimal d11 = new decimal(line1[1]), d10 = new decimal(line1[0]);
                decimal d21 = new decimal(line2[1]), d20 = new decimal(line2[0]);
                //if (Double.Compare(ArithUtil.div(line1[1], line1[0]), ArithUtil.div(line2[1], line2[0])) == 0)
                if (Decimal.Divide(d11, d10).CompareTo(Decimal.Divide(d21, d20)) == 0)
                    return null;
            }
            // 两直线不平行，有交点
            double x = (line1[1] * line2[2] - line2[1] * line1[2]) / (line1[0] * line2[1] - line2[0] * line1[1]);
            double y = (line2[0] * line1[2] - line1[0] * line2[2]) / (line1[0] * line2[1] - line2[0] * line1[1]);
            TPoint2D point = new TPoint2D(x, y);
            return point;
        }

        //===================================================================
        // inSegment(): determine if a point is inside a segment
        //    Input:  a point P, and a collinear segment S
        //    Return: 1 = P is inside S
        //            0 = P is  not inside S
        public static int inSegment(TPoint2D P, TLineSegment2d S)
        {
            if (S.SP.X != S.EP.X)
            {    // S is not  vertical
                if (S.SP.X <= P.X && P.X <= S.EP.X)
                    return 1;
                if (S.SP.X >= P.X && P.X >= S.EP.X)
                    return 1;
            }
            else
            {    // S is vertical, so test Y  coordinate
                if (S.SP.Y <= P.Y && P.Y <= S.EP.Y)
                    return 1;
                if (S.SP.Y >= P.Y && P.Y >= S.EP.Y)
                    return 1;
            }
            return 0;
        }
        /// <summary>
        /// return true if p lies on the line segment defined by AB, but not at any endpoints;
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [Obsolete("Multiple Implement")]
        public static bool OnSegment(TPoint2D A, TPoint2D B, TPoint2D p)
        {
            // vertical line
            if (AlmostEqual(A.X, B.X) && AlmostEqual(p.X, A.X))
            {
                if (!AlmostEqual(p.Y, B.Y) && !AlmostEqual(p.Y, A.Y) && p.Y < Math.Max(B.Y, A.Y) && p.Y > Math.Min(B.Y, A.Y)) return true;
                else return false;
            }

            // horizontal line
            if (AlmostEqual(A.Y, B.Y) && AlmostEqual(p.Y, A.Y))
            {
                if (!AlmostEqual(p.X, B.X) && !AlmostEqual(p.X, A.X) && p.X < Math.Max(B.X, A.X) && p.X > Math.Min(B.X, A.X))
                    return true;
                else return false;
            }

            // range check
            if ((p.X < A.X && p.X < B.X) || (p.X > A.X && p.X > B.X) || (p.Y < A.Y && p.Y < B.Y) || (p.Y > A.Y && p.Y > B.Y))
                return false;

            // exclude end points
            if ((AlmostEqual(p.X, A.X) && AlmostEqual(p.Y, A.Y)) || (AlmostEqual(p.X, B.X)) && AlmostEqual(p.Y, B.Y))
                return false;

            var cross = (p.Y - A.Y) * (B.X - A.X) - (p.X - A.X) * (B.Y - A.Y);

            if (Math.Abs(cross) > EPSILON)
            {
                return false;
            }

            var dot = (p.X - A.X) * (B.X - A.X) + (p.Y - A.Y) * (B.Y - A.Y);



            if (dot < 0 || AlmostEqual(dot, 0))
            {
                return false;
            }

            var len2 = (B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y);



            if (dot > len2 || AlmostEqual(dot, len2))
            {
                return false;
            }

            return true;
        }

        //===================================================================
        // isLineIinPolygon(): determine if a Segment two ends SP & EP
        // is inside a SetByRowElements of points;
        //    Input:  two points SP & EP, and a SetByRowElements of points points_array
        //    Return: true  = SP and EP  is      inside points_array
        //            false = SP and EP  is  not inside points_array
        public static bool isLineIinPolygon(TPoint2D sp, TPoint2D ep, List<TPoint2DCollection> points_array)
        {
            if (points_array.Count == 0) return false;
            TPoint2DCollection outer = points_array[0];
            if (outer.Count == 0) return false;

            if (outer.Area() < 0) outer.Reverse();
            if (isInPoints(sp, outer) && isInPoints(ep, outer))
            {
                for (int i = 1; i < points_array.Count; i++)
                {
                    TPoint2DCollection hole = points_array[i];
                    if (hole.Area() < 0) hole.Reverse();
                    if (isInPoints(sp, hole) || isInPoints(ep, hole) || isIntersect(sp, ep, hole)) { return false; }
                    else continue;
                }
                return true;
            }
            else
                return false;
        }
        //===================================================================
        // isInPoints(): determine if a point is inside a SetByRowElements of points;
        //    Input:  a points P, and a SetByRowElements of points points
        //    Return: true  = P  is      inside points
        //            false = P  is  not inside points
        public static bool isInPoints(TPoint2D P, TPoint2DCollection points)
        {
            int cn = 0;    // the  crossing number counter
            int n = points.Count;
            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {    // edge from points[i]  to points[i+1]
                if (((points[i].Y <= P.Y) && (points[(i + 1) % n].Y > P.Y))     // an upward crossing
                    || ((points[i].Y > P.Y) && (points[(i + 1) % n].Y <= P.Y)))
                { // a downward crossing
                  // compute  the actual edge-ray intersect X-coordinate
                    double vt = (P.Y - points[i].Y) / (points[(i + 1) % n].Y - points[i].Y);
                    if (P.X < points[i].X + vt * (points[(i + 1) % n].X - points[i].X)) // P.X < intersect
                        ++cn;   // a valid crossing of Y=P.Y right of P.X
                }
            }
            return (cn & 1) == 1 ? true : false;    // 0 if even (out), and 1 if  odd (in)
        }
        //===================================================================
        // isIntersect(): determine if a point is intersected with a SetByRowElements of points;
        //    Input:  two points p1 & p2, and a SetByRowElements of points points
        //    Return: true  = segment<p1, p2>  is     intersected with the points;
        //            false = segment<p1, p2>  is not intersected with the points;
        public static bool isIntersect(TPoint2D p1, TPoint2D p2, TPoint2DCollection points)
        {
            int n = points.Count;
            for (int i = 0; i < n; i++)
            {
                TPoint2D s = points[i], e = points[(i + 1) % n];
                if ((p1.X > p2.X ? p1.X : p2.X) < (s.X < e.X ? s.X : e.X) ||
                    (p1.Y > p2.Y ? p1.Y : p2.Y) < (s.Y < e.Y ? s.Y : e.Y) ||
                    (s.X > e.X ? s.X : e.X) < (p1.X < p2.X ? p1.X : p2.X) ||
                    (s.Y > e.Y ? s.Y : e.Y) < (p1.Y < p2.Y ? p1.Y : p2.Y))
                    continue;

                if ((((p1.X - s.X) * (e.Y - s.Y) - (p1.Y - s.Y) * (e.X - s.X)) *
                    ((p2.X - s.X) * (e.Y - s.Y) - (p2.Y - s.Y) * (e.X - s.X))) > 0 ||
                    (((s.X - p1.X) * (p2.Y - p1.Y) - (s.Y - p1.Y) * (p2.X - p1.X)) *
                    ((e.X - p1.X) * (p2.Y - p1.Y) - (e.Y - p1.Y) * (p2.X - p1.X))) > 0)
                    continue;
                return true;
            }
            return false;
        }
        public static bool isPointOnPolygon(TPoint2D A, TPoint2DCollection B)
        {
            if (!IsEqualPoint(B.First(), B.Last()))
                B.Add(B.First());
            int n = B.Count;
            for (int i = 1; i < n; i++)
            {
                TLineSegment2d seg = new TLineSegment2d(B[i - 1], B[i]);
                if (seg.IsPointOn(A)) return true;
            }
            return false;
        }
        // 判断点是否在轮廓内(射线法)
        // Input: check point P, and the boundary of polygon points
        // Return:  1 = P is inside of points
        //          0 = P is on the boundray of points
        //         -1 = P is outside of points 
        public static bool IsPointInPolygon(TPoint2D P, TPoint2DCollection points)
        {
            TPoint2DCollection points_copy = new TPoint2DCollection(points);
            if (!IsEqualPoint(points_copy.First(), points_copy.Last()))
                points_copy.Add(points_copy.First());

            List<TLineSegment2d> contour = new List<TLineSegment2d>();

            for (int i = 0; i < points_copy.Count - 1; i++)
            {
                TLineSegment2d lineSegment = new TLineSegment2d(points_copy[i], points_copy[i + 1]);
                contour.Add(lineSegment);
            }
            return IsPointInPolygon(P, contour);

        }
        // 判断点是否在轮廓内(射线法)
        // Input: check point P, and the boundary of polygon contour
        // Return:  1 = P is inside of points
        //          0 = P is on the boundray of points
        //         -1 = P is outside of points 
        public static bool IsPointInPolygon(TPoint2D P, List<TLineSegment2d> contour)
        {
            bool flag = false;
            // intersectionPoints为y=point.getY()直线与contour的交点
            HashSet<TPoint2D> intersectionPoints = new HashSet<TPoint2D>();
            foreach (TLineSegment2d lineSegment in contour)
            {
                TPoint2D? point1 = GetYPointOnSegment(lineSegment, P.Y);
                if (!(point1 is null))
                {
                    intersectionPoints.Add((TPoint2D)point1);
                }
            }
            int count = 0;
            foreach (TPoint2D p in intersectionPoints)
                if (p.X.CompareTo(P.X) < 0)
                    count++;

            // 是奇数，在轮廓里
            if (count % 2 != 0)
                flag = true;
            return flag;
        }
        // 知道一个点的y坐标, 获取该点在线上的坐标
        public static TPoint2D? GetYPointOnSegment(TLineSegment2d linesegment, double y)
        {
            TPoint2D first = linesegment.SP;
            TPoint2D second = linesegment.EP;
            double x1 = first.X, x2 = second.X;
            double y1 = first.Y, y2 = second.Y;

            if (y1.CompareTo(y) < 0 && y2.CompareTo(y) < 0) return null;
            if (y1.CompareTo(y) > 0 && y2.CompareTo(y) > 0) return null;
            if (y1.CompareTo(y2) == 0) return null;

            double a = (y - y1) * (x2 - x1) / (y2 - y1);
            double x = a + x1;
            return new TPoint2D(x, y);
        }
        //===================================================================
        //===================================================================
        // isRingAinRingB(): determine if a SetByRowElements of points is inside another SetByRowElements of points;
        //    Input:  a SetByRowElements of points A, and another SetByRowElements of points B
        //    Return: true  = A is      inside of B
        //            false = A is not  inside of B
        // A 环 是否在 B 环 中
        public static bool isRingAinRingB(TPoint2DCollection A, TPoint2DCollection B)
        {
            if (B.Area() < 0) B.Reverse();
            bool res = true;
            for (int i = 0; i < A.Count; i++)
                if (!isInPoints(A[i], B))
                    return false;
            return res;
        }
        public static bool isRingAInterRingB(TPoint2DCollection A, TPoint2DCollection B)
        {
            if (B.Area() < 0) B.Reverse();
            for (int i = 0; i < A.Count; i++)
                if (isPointOnPolygon(A[i], B)) continue;
                else if (isInPoints(A[i], B)) return true;
            return false;
        }
        //===================================================================


        /// <summary>
        /// 多边形的面积
        /// PntsArea(): calculate the area of 2D polygon
        /// Input:  an array of n+1 vertex points with V[n]=V[0]
        /// Return: the (double) area of the polygon
        /// </summary>
        /// <param name="poly_pnts"></param>
        /// <returns></returns>
        [Obsolete("并入 TPoints2DCollection 的方法中")]
        public static double PntsArea(in TPoint2DCollection _poly_pnts)
        {
            if (_poly_pnts.Count < 3) return 0.0;
            TPoint2DCollection poly_pnts = new TPoint2DCollection();
            _poly_pnts.ForEach(pnt => poly_pnts.Add(new TPoint2D(pnt)));
            double area = 0.0;
            if (!IsEqualPoint(poly_pnts.First(), poly_pnts.Last()))
                poly_pnts.Add(new TPoint2D(poly_pnts.First()));

            int n = poly_pnts.Count - 1;

            for (int i = 1, j = 2, k = 0; i < n; i++, j++, k++)
            {
                area += (poly_pnts[i].X * (poly_pnts[j].Y - poly_pnts[k].Y));
            }
            area += (poly_pnts[n].X * (poly_pnts[1].Y - poly_pnts[n - 1].Y));
            return area / 2.0;
        }

        public static decimal PntsDArea(TPoint2DCollection poly_pnts)
        {
            decimal area = new decimal(0.00000);
            if (poly_pnts.Count < 3) return area;
            if (!IsEqualPoint(poly_pnts.First(), poly_pnts.Last()))
                poly_pnts.Add(poly_pnts.First());

            int n = poly_pnts.Count - 1;

            for (int i = 1, j = 2, k = 0; i < n; i++, j++, k++)
            {
                //area += (poly_pnts[i].X * (poly_pnts[j].Y - poly_pnts[k].Y));
                area += new decimal(poly_pnts[i].X) * (new decimal(poly_pnts[j].Y) - new decimal(poly_pnts[k].Y));
            }
            //area += (poly_pnts[n].X * (poly_pnts[1].Y - poly_pnts[n - 1].Y));
            area += new decimal(poly_pnts[n].X) * (new decimal(poly_pnts[1].Y) - new decimal(poly_pnts[n - 1].Y));
            return area / new decimal(2.0);
        }
        //===================================================================
        // IsParallel(): determine if two vecto is parallel
        //    Input:  two vectors a & b;
        //    Return: true  = a and b is     parallel
        //            false = a and b is not parallel
        // 向量 a 和 向量 b 是否平行
        public static bool IsParallel(TVector2D a, TVector2D b, double tol = EPSILON)
        {
            double pro = a.X * b.X + a.Y * b.Y;
            double amod2 = a.X * a.X + a.Y * a.Y;
            double bmode2 = b.X * b.X + b.Y * b.Y;
            return NumberUtil.CompValue(pro * pro / (amod2 * bmode2), 1, tol) == 0;
        }

        [Obsolete("并入 TPoints2DCollection 的方法中")]
        public static int xyorder(TPoint2D p1, TPoint2D p2)
        {
            if (p1.X > p2.X) return 1;
            if (p1.X < p2.X) return -1;
            if (p1.Y > p2.Y) return 1;
            if (p1.Y < p2.Y) return -1;

            return 0;
        }
        /**
         * 判断多边形是否为顺时针方向
         * https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order
         *
         * @param points
         * @return true if contour is in clockwise
         */
        public static bool isClockwise(TPoint2DCollection points)
        {
            double sum = 0;
            for (int i = 0; i < points.Count; i++)
            {
                int next = (i + 1) % points.Count;

                double x1 = points[i].X;
                double y1 = points[i].Y;
                double x2 = points[next].X;
                double y2 = points[next].Y;

                sum += (x2 - x1) * (y1 + y2);
            }

            if (sum.CompareTo(0) > 0)
            {
                return true;
            }
            return false;
        }

        // 计算 2d 点集的轴向包围盒[AABB](Axis-aligned bounding box)
        public static bool Cal2dAABBox(IEnumerable<TPoint2D> pnts, ref TPoint2D lb, ref TPoint2D rt)
        {
            if (pnts.Count() == 0) return false;
            else
            {
                lb = new TPoint2D(pnts.ElementAt(0)); rt = new TPoint2D(pnts.ElementAt(0)); // 在 csharp 中， 对象间的引用一定不要用 = 
                foreach (TPoint2D pt in pnts)
                {
                    lb.X = Math.Min(lb.X, pt.X);
                    lb.Y = Math.Min(lb.Y, pt.Y);
                    rt.X = Math.Max(rt.X, pt.X);
                    rt.Y = Math.Max(rt.Y, pt.Y);
                }
                return true;
            }
        }
    }
    public static class NumberUtil
    {
        public static int CompValue(double a, double b, double tol = 1e-6)
        {
            if ((a - b) < -Math.Abs(tol))
                return -1; // a < b
            else if ((a - b) > Math.Abs(tol))
                return 1; // a > b
            else
                return 0; // a = b
        }
        // 角度转弧度
        public static double DegreeToRadian(double angle)
        {
            return angle / 180 * Math.PI;
        }
        // 弧度转角度
        public static double RadianToDegree(double radian)
        {
            return radian / Math.PI * 180;
        }

    }
    // 包围盒的相关计算
    public static class BoundingBox
    {
        //TODO: 三个点是否构成右拐
        private static bool IsRight(TPoint2D _first, TPoint2D _second, TPoint2D _third)
        {
            TVector2D v1 = (_second - _first).ToVector();
            TVector2D v2 = (_third - _second).ToVector();
            //TVector2d vNormal = v1.CrossProduct(v2);
            //return (vNormal.Z > 0);
            return v1.Perp(v2) > 0;
        }

        /// <summary>
        /// 计算点集的 Bounds
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static bool GetPolygonBounds(TPoint2DCollection polygon, out Bounds bounds)
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
            bounds.Width = Math.Abs(xmax - xmin);
            bounds.Height = Math.Abs(ymax - ymin);

            return true;
        }
        
        public static void CalRectOfPointset(TPoint2DCollection points, out TPoint2D lb, out TPoint2D rt)
        {
            double max_x = double.MinValue, max_y = double.MinValue, min_x = double.MaxValue, min_y = double.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                TPoint2D pnt = points[i];
                max_x = Math.Max(pnt.X, max_x);
                max_y = Math.Max(pnt.Y, max_y);

                min_x = Math.Min(pnt.X, min_x);
                min_y = Math.Min(pnt.Y, min_y);
            }
            if (points.Count == 0)
            {
                min_x = 0;
                min_y = 0;
                max_x = 0;
                max_y = 0;
            }
            lb = new TPoint2D(min_x, min_y);
            rt = new TPoint2D(max_x, max_y);
        }

        public static void CalRectOfPointset(List<TPoint2DCollection> points, out TPoint2D lb, out TPoint2D rt)
        {
            double max_x = double.MinValue, max_y = double.MinValue, min_x = double.MaxValue, min_y = double.MaxValue;
            if (points.Count == 0)
            {
                max_x = 0;
                max_y = 0;
                min_x = 0;
                min_y = 0;
            }
            else
            {
                foreach (var pnts in points)
                {
                    for (int i = 0; i < pnts.Count; i++)
                    {
                        TPoint2D pnt = pnts[i];
                        max_x = Math.Max(pnt.X, max_x);
                        max_y = Math.Max(pnt.Y, max_y);

                        min_x = Math.Min(pnt.X, min_x);
                        min_y = Math.Min(pnt.Y, min_y);
                    }
                }
            }
            lb = new TPoint2D(min_x, min_y);
            rt = new TPoint2D(max_x, max_y);
        }
    }

}
