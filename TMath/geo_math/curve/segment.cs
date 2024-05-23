using System;

namespace tmath.geometry
{
    public interface ISegment { }
    // 线段
    public class TSegment<T, V> : ISegment where T : IPoint<T, V> where V : IVector<V>
    {
        protected const double SMALL_NUM = 1e-6;
        public virtual T SP { get; set; }
        public virtual T EP { get; set; }
        public TSegment() { }
        public TSegment(T s, T e) { SP = s; EP = e; }

        public virtual V Vector() { return EP.Sub(SP).ToVector(); }

        public virtual V Dir()
        {
            V v = Vector();
            v.Normalize();
            return v;
        }

        public virtual double Length() => EP.DistanceTo(SP);

        /// <summary>
        /// 判断点是否在线上
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        public virtual bool IsPointOn(T P, double tol = GeometryUtil.CommonUtil.EPSILON) => IsPointOn(P, new Tolerance(0, tol));

        /// <summary>
        /// 判断点是否在线上
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        public virtual bool IsPointOn(T P, Tolerance tol) { throw new NotImplementedException(); }


    }

    // 平面线段
    public class TLineSegment2d : TSegment<TPoint2D, TVector2D>
    {
        #region Constructor
        public TLineSegment2d(TPoint2D startPnt, TPoint2D endPnt) : base(startPnt, endPnt) { }
        public TLineSegment2d(double x1, double y1, double x2, double y2) : base(new TPoint2D(x1, y1), new TPoint2D(x2, y2)) { }
        #endregion

        #region Overide Methods
        public override bool IsPointOn(TPoint2D P, double tol = 0.001) => IsPointOn(P, new Tolerance(0, tol));

        public override bool IsPointOn(TPoint2D P, Tolerance tol)
        {
            TVector2D v = (P - SP).ToVector();
            return NumberUtils.CompValue(v.GetNormal().Dot(Dir()), 1) == 0 &&
                NumberUtils.CompValue(v.Length(), Length(), tol.EqualPoint) < 1;
        }
        public override TVector2D Vector() => (EP - SP).ToVector();
        #endregion

        #region Self Methods
        /// <summary>
        /// 按长度离散
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public TPoint2DCollection Discrete(double span = 1.0)
        {
            var result = new TPoint2DCollection();
            TPoint2D curpnt = new TPoint2D(SP);
            while (curpnt.DistanceTo(SP) < Length())
            {
                result.Add(curpnt);
                curpnt += Dir() * span;
            }
            return result;
        }

        /// <summary>
        /// 计算点 P 的投影是否在当前线段的区间上， 点在线段上的跨立实验
        /// </summary>
        /// <returns></returns>
        private int InSegment(TPoint2D P)
        {
            if (SP.X != EP.X)
            {    // S is not  vertical
                if (SP.X <= P.X && P.X <= EP.X)
                    return 1;
                if (SP.X >= P.X && P.X >= EP.X)
                    return 1;
            }
            else
            {    // S is vertical, so test Y  coordinate
                if (SP.Y <= P.Y && P.Y <= EP.Y)
                    return 1;
                if (SP.Y >= P.Y && P.Y >= EP.Y)
                    return 1;
            }
            return 0;
        }

        public INTER_NUM IntersectWith(TLineSegment2d segment, out TPoint2D I0, out TPoint2D I1) => IntersectWith(segment, out I0, out I1, Tolerance.Global);

        public INTER_NUM IntersectWith(TLineSegment2d segment, out TPoint2D I0, out TPoint2D I1, Tolerance tolerance)
        {

            I0 = default; I1 = default;

            TVector2D u = (this.EP - this.SP).ToVector();
            TVector2D v = (segment.EP - segment.SP).ToVector();
            TVector2D w = (this.SP - segment.SP).ToVector();
            double D = TVector2D.Cross(u, v);
            // test if  they are parallel (includes either being a point)
            if (Math.Abs(D) <= tolerance.EqualVector)
            {           // this and segment are parallel
                        //if (TVector2D.Cross(u, w) != 0 || TVector2D.Cross(v, w) != 0)
                        //{
                        //    return INTER_NUM.ZERO;                    // they are NOT collinear
                        //}

                if (u.GetNormal().Dot(w.GetNormal()) == v.GetNormal().Dot(w.GetNormal()))
                {
                    return INTER_NUM.ZERO;
                }
                // they are collinear or degenerate
                // check if they are degenerate  points
                double du = (u * u);
                double dv = (v * v);
                if (du == 0 && dv == 0)
                {            // both segments are points
                    if (this.SP != segment.SP)         // they are distinct  points
                        return 0;
                    I0 = this.SP;                 // they are the same point
                    return INTER_NUM.ONE;
                }
                if (du == 0)
                {                     // this is a single point
                    if (segment.InSegment(this.SP) == 0)  // but is not in segment
                        return 0;
                    I0 = this.SP;
                    return INTER_NUM.ONE;
                }
                if (dv == 0)
                {                     // segment a single point
                    if (InSegment(segment.SP) == 0)  // but is not in this
                        return 0;
                    I0 = segment.SP;
                    return INTER_NUM.ONE;
                }
                // they are collinear segments - get  overlap (or not)
                double t0, t1;                    // endpoints of this in eqn for segment
                TVector2D w2 = (this.EP - segment.SP).ToVector();
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
                if (t0 > t1 + tolerance.EqualPoint)
                {                   // must have t0 smaller than t1
                    double t = t0; t0 = t1; t1 = t;    // swap if not
                }
                if (t0 > 1 || t1 < 0)
                {
                    return INTER_NUM.ZERO;      // NO overlap
                }
                t0 = t0 < 0 ? 0 : t0;               // clip to min 0
                t1 = t1 > 1 ? 1 : t1;               // clip to max 1
                //if (t0 == t1)
                if (Math.Abs(t0 - t1) < tolerance.EqualPoint)
                {                  // intersect is a point
                    I0 = segment.SP + t0 * v;
                    return INTER_NUM.ONE;
                }

                // they overlap in a valid subsegment
                I0 = segment.SP + t0 * v;
                I1 = segment.SP + t1 * v;
                return INTER_NUM.TWO;
            }

            // the segments are skew and may intersect in a point
            // get the intersect parameter for S1
            double sI = TVector2D.Cross(v, w) / D;
            if (sI < -tolerance.EqualPoint || sI > 1 + tolerance.EqualPoint)                // no intersect with S1
                return INTER_NUM.ZERO;
            // get the intersect parameter for segment
            double tI = TVector2D.Cross(u, w) / D;
            if (tI < -tolerance.EqualPoint || tI > 1 + tolerance.EqualPoint)                // no intersect with segment
                return INTER_NUM.ZERO;
            I0 = this.SP + sI * u;                // compute S1 intersect point
            return INTER_NUM.ONE;

        }

        public INTER_NUM IntersectWith(TLine2D line, out TPoint2D P0, out TPoint2D P1)
        {
            P0 = TPoint2D.NULL; P1 = TPoint2D.NULL;
            var s1 = TPoint2D.IsLeft(line.P0, line.P1, SP);
            var s2 = TPoint2D.IsLeft(line.P0, line.P1, EP);
            if (s1 * s2 > 0) return INTER_NUM.ZERO;

            TLine2D line1 = new TLine2D(this), line2 = line;
            var res = line1.IntersectWith(line2, out P0);
            if (res == INTER_NUM.ZERO && line.IsPointOn(SP) && line.IsPointOn(EP))
            {
                P0 = SP; P1 = EP;
                return INTER_NUM.TWO;
            }
            return res;
        }

        public void RotateByPoint(double angle, TPoint2D ptBase)
        {
            SP = SP.RotateByPoint(angle, ptBase);
            EP = EP.RotateByPoint(angle, ptBase);
        }

        public double ClosestPointTo(TLineSegment2d other, out TPoint2D pnt_self, out TPoint2D pnt_other, double tolerance = TConstant.Epsilon)
        {
            TVector2D u = (this.EP - this.SP).ToVector();
            TVector2D v = (other.EP - other.SP).ToVector();
            TVector2D w = (this.SP - other.SP).ToVector();
            double a = u.Dot(u);         // always >= 0
            double b = u.Dot(v);
            double c = v.Dot(v);         // always >= 0
            double d = u.Dot(w);
            double e = v.Dot(w);
            double D = a * c - b * b;        // always >= 0
            double sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
            double tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            if (D < tolerance)
            { // the lines are almost parallel
                sN = 0.0;         // force using point P0 on segment this
                sD = 1.0;         // to prevent possible division by 0.0 later
                tN = e;
                tD = c;
            }
            else
            {                 // get the closest points on the infinite lines
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0.0)
                {        // sc < 0 => the s=0 edge is visible
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {  // sc > 1  => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0)
            {            // tc < 0 => the t=0 edge is visible
                tN = 0.0;
                // recompute sc for this edge
                if (-d < 0.0)
                    sN = 0.0;
                else if (-d > a)
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {      // tc > 1  => the t=1 edge is visible
                tN = tD;
                // recompute sc for this edge
                if ((-d + b) < 0.0)
                    sN = 0;
                else if ((-d + b) > a)
                    sN = sD;
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            sc = (Math.Abs(sN) < tolerance ? 0.0 : sN / sD);
            tc = (Math.Abs(tN) < tolerance ? 0.0 : tN / tD);

            pnt_self = this.SP + sc * u;
            pnt_other = other.SP + tc * v;
            return pnt_other.DistanceTo(pnt_self);

        }
        #endregion
    }

    // 空间线段
    public class TLineSegment3d : TSegment<TPoint3D, TVector3D>
    {
        public TLineSegment3d(TPoint3D startPnt, TPoint3D endPnt) : base(startPnt, endPnt) { }

        public bool IntersectWith(TLineSegment3d segment3D, out TPoint3D result_pnt, double tol = 0.001) => IntersectWith(segment3D, new Tolerance(tol, 0), out result_pnt);

        public bool IntersectWith(TLineSegment3d segment3D, Tolerance tol, out TPoint3D result_pnt)
        {
            TLine3D line1 = new TLine3D(this), line2 = new TLine3D(segment3D);
            bool iscloses = TLine3D.CalculateLineLineIntersection(line1, line2, out result_pnt, out TPoint3D other_pnt);
            if (iscloses && result_pnt.DistanceTo(other_pnt) < tol.EqualPoint) return true; else return false;
        }

        public bool IntersectWith(TPlane3D plane, out TPoint3D result_pnt, double tol = double.Epsilon)
        {
            TLine3D line1 = new TLine3D(this);
            bool intersected = plane.IntersectLine(line1, out result_pnt);
            if (intersected && IsPointOn(result_pnt)) return true;
            return false;
        }

        public override bool IsPointOn(TPoint3D P, double tol = 0.001) => IsPointOn(P, new Tolerance(0, tol));

        public override bool IsPointOn(TPoint3D P, Tolerance tol)
        {
            TVector3D v = (P - SP).ToVector();
            return NumberUtils.CompValue(v.GetNormal().Dot(Dir()), 1) == 0 &&
                NumberUtils.CompValue(v.Length(), Length(), tol.EqualPoint) < 1;
        }

        public override TVector3D Vector() => (EP - SP).ToVector();
    }
}
