using System;
using System.Collections.Generic;
using tmath.geo_math.curve;

namespace tmath.geometry
{
    // 直线 无向 无限
    public abstract class Line<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        protected T point0, point1;

        /// <summary>
        /// 判断点在线上
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract bool IsPointOn(T p);

        /// <summary>
        /// 获得 P 在 Line 上的投影点
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public abstract T GetProjectPoint(T P);

        public T P0 => point0;
        public T P1 => point1;

        public abstract Line<T, V> Clone();

        public void SetPoints(T p0, T p1)
        {
            point0 = p0; point1 = p1;
        }

        public double DistanceToPoint(T P, out T online)
        {
            V v = P1.Sub(P0).ToVector();
            V w = P.Sub(P0).ToVector();

            double c1 = v.Dot(w);
            double c2 = v.Dot(v);
            double b = c1 / c2;

            online = P0.Add(v.Multiple(b));
            return P.DistanceTo(online);
        }
    }

    // 2d 直线, 无向, 无限
    public class TLine2D : Line<TPoint2D, TVector2D>
    {
        #region Fields
        public double X1 => point1.X;
        public double Y1 => point1.Y;
        public double X0 => point0.X;
        public double Y0 => point0.Y;
        #endregion

        #region Constructor
        public TLine2D() { point1 = new TPoint2D(0, 0); point0 = new TPoint2D(1, 0); }
        public TLine2D(TPoint2D e1, TPoint2D e2) { point1 = e1; point0 = e2; }
        public TLine2D(TLineSegment2d segment) { point1 = segment.SP; point0 = segment.EP; }
        public TLine2D(double x1, double y1, double x2, double y2) { point0 = new TPoint2D(x1, y1); point1 = new TPoint2D(x2, y2); }
        #endregion

        #region Static Methods
        public static INTER_NUM DoLinesIntersect(TLine2D L1, TLine2D L2, out TPoint2D result_pnt)
        {
            result_pnt = TPoint2D.NULL;
            // Denominator for ua and ub are the same, so store this calculation
            double d =
               (L2.Y0 - L2.Y1) * (L1.X0 - L1.X1)
               -
               (L2.X0 - L2.X1) * (L1.Y0 - L1.Y1);

            //n_a and n_b are calculated as seperate values for readability
            double n_a =
               (L2.X0 - L2.X1) * (L1.Y1 - L2.Y1)
               -
               (L2.Y0 - L2.Y1) * (L1.X1 - L2.X1);

            double n_b =
               (L1.X0 - L1.X1) * (L1.Y1 - L2.Y1)
               -
               (L1.Y0 - L1.Y1) * (L1.X1 - L2.X1);

            // Make sure there is not a division by zero - this also indicates that
            // the lines are parallel.  
            // If n_a and n_b were both equal to zero the lines would be online top of each 
            // other (coincidental).  This check is not done because it is not 
            // necessary for this implementation (the parallel check accounts for this).
            if (d == 0)
                return INTER_NUM.ZERO;

            // Calculate the intermediate fractional point that the lines potentially intersect.
            double ua = n_a / d;
            double ub = n_b / d;

            // The fractional point will be between 0 and 1 inclusive if the lines
            // intersect.  If the fractional calculation is larger than 1 or smaller
            //// than 0 the lines would need to be longer to intersect.
            //if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            //{
            result_pnt.X = L1.X1 + (ua * (L1.X0 - L1.X1));
            result_pnt.Y = L1.Y1 + (ua * (L1.Y0 - L1.Y1));
            return INTER_NUM.ONE;
            //}
            //return INTER_NUM.ZERO;
        }
        #endregion

        #region Self Methodss
        public INTER_NUM IntersectWith(TLine2D line, out TPoint2D intersect_pnt) =>
            DoLinesIntersect(this, line, out intersect_pnt);

        public INTER_NUM IntersectWith(TLineSegment2d segment, out TPoint2D P0, out TPoint2D P1) => segment.IntersectWith(this, out P0, out P1);

        public bool IntersectWith(TArc2D arc, out List<TPoint2D> intersect_pnt) { throw new NotImplementedException(); }

        public void RotateByPoint(double angle, TPoint2D ptBase)
        {
            point0 = point0.RotateByPoint(angle, ptBase);
            point1 = point1.RotateByPoint(angle, ptBase);
        }

        /// <summary>
        /// 计算段在线上的投影点
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public override TPoint2D GetProjectPoint(TPoint2D P)
        {
            TVector2D v = (P1 - P0).ToVector();
            TVector2D w = (P - P0).ToVector();

            double c1 = w * v;
            double c2 = v * v;
            double b = c1 / c2;

            return P0 + b * v;
        }
        #endregion

        #region Override Methods
        public override bool IsPointOn(TPoint2D p) => TVector2D.Cross((p - P0).ToVector(), ((P1 - P0).ToVector())) == 0;

        public override Line<TPoint2D, TVector2D> Clone() => new TLine2D(P0, P1);
        #endregion
    }

    // 3d 直线, 无向, 无限
    public class TLine3D : Line<TPoint3D, TVector3D>
    {
        #region Constructor
        public TLine3D() { point1 = new TPoint3D(0, 0, 0); point0 = new TPoint3D(1, 0, 0); }
        public TLine3D(TPoint3D e1, TPoint3D e2) { point1 = e1; point0 = e2; }
        public TLine3D(TLineSegment3d segment) { point1 = segment.SP; point0 = segment.EP; }
        #endregion

        #region SelfMethods
        public TVector3D Delta() => (point0 - point1).ToVector();

        public static bool CalculateLineLineIntersection(TLine3D line1, TLine3D line2, out TPoint3D result_pnt1, out TPoint3D result_pnt2)
        {
            // Algorithm is ported from the C algorithm of 
            // Paul Bourke at http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline3d/
            result_pnt1 = new TPoint3D();
            result_pnt2 = new TPoint3D();

            TPoint3D p1 = line1.point1;
            TPoint3D p2 = line1.point0;
            TPoint3D p3 = line2.point1;
            TPoint3D p4 = line2.point0;

            TVector3D p13 = (p1 - p3).ToVector();
            TVector3D p43 = (p4 - p3).ToVector();

            if (p43.LengthSq() < TConstant.Epsilon)
            {
                return false;
            }
            TVector3D p21 = (p2 - p1).ToVector();
            if (p21.LengthSq() < TConstant.Epsilon)
            {
                return false;
            }

            double d1343 = p13.X * (double)p43.X + (double)p13.Y * p43.Y + (double)p13.Z * p43.Z;
            double d4321 = p43.X * (double)p21.X + (double)p43.Y * p21.Y + (double)p43.Z * p21.Z;
            double d1321 = p13.X * (double)p21.X + (double)p13.Y * p21.Y + (double)p13.Z * p21.Z;
            double d4343 = p43.X * (double)p43.X + (double)p43.Y * p43.Y + (double)p43.Z * p43.Z;
            double d2121 = p21.X * (double)p21.X + (double)p21.Y * p21.Y + (double)p21.Z * p21.Z;

            double denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < TConstant.Epsilon)
            {
                return false;
            }
            double numer = d1343 * d4321 - d1321 * d4343;

            double mua = numer / denom;
            double mub = (d1343 + d4321 * (mua)) / d4343;

            result_pnt1.X = (p1.X + mua * p21.X);
            result_pnt1.Y = (p1.Y + mua * p21.Y);
            result_pnt1.Z = (p1.Z + mua * p21.Z);
            result_pnt2.X = (p3.X + mub * p43.X);
            result_pnt2.Y = (p3.Y + mub * p43.Y);
            result_pnt2.Z = (p3.Z + mub * p43.Z);

            return true;
        }
        public override TPoint3D GetProjectPoint(TPoint3D P)
        {
            TVector3D v = (P1 - P0).ToVector();
            TVector3D w = (P - P0).ToVector();

            double c1 = w * v;
            double c2 = v * v;
            double b = c1 / c2;

            return P0 + b * v;
        }
        #endregion

        #region Override Methods
        public override bool IsPointOn(TPoint3D p) => (p - P0).ToVector() * ((P0 - P1).ToVector()) == 0;

        public override Line<TPoint3D, TVector3D> Clone() => new TLine3D(P0, P1);
        #endregion
    }
}
