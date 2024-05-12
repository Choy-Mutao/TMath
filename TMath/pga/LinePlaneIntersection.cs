using System;
using tmath.geometry;

namespace tmath.pga
{
    public static class LinePlaneIntersection
    {
        const double SMALL_NUM = 0.00000001;
        #region Static Methods

        /// <summary>
        /// determine if a point is inside a segment
        /// </summary>
        /// <param name="P">a point P</param>
        /// <param name="S"> a collinear segment S</param>
        /// <returns>
        /// 1 for P is inside S
        /// 0 for P is not inside S
        /// </returns>
        public static int InSegment(TPoint2D P, TLineSegment2d S)
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
        /// find the 2D intersection of 2 finite segments
        /// </summary>
        /// <param name="S1">first segment</param>
        /// <param name="S2">second segment</param>
        /// <param name="I0">intersect point (when it exists)</param>
        /// <param name="I1">endpoint of intersect segment [I0,I1] (when it exists)</param>
        /// <returns>
        /// 0=disjoint (no intersect)
        /// 1=intersect  in unique point I0
        /// 2=overlap  in segment from I0 to I1
        /// </returns>
        public static int Intersect2D_2Segments(TLineSegment2d S1, TLineSegment2d S2, out TPoint2D I0, out TPoint2D I1)
        {
            I0 = default; I1 = default;

            TVector2D u = (S1.EP - S1.SP).ToVector();
            TVector2D v = (S2.EP - S2.SP).ToVector();
            TVector2D w = (S1.SP - S2.SP).ToVector();
            double D = TVector2D.Cross(u, v);
            // test if  they are parallel (includes either being a point)
            if (Math.Abs(D) < SMALL_NUM)
            {           // S1 and S2 are parallel
                if (TVector2D.Cross(u, w) != 0 || TVector2D.Cross(v, w) != 0)
                {
                    return 0;                    // they are NOT collinear
                }
                // they are collinear or degenerate
                // check if they are degenerate  points
                double du = (u * u);
                double dv = (v * v);
                if (du == 0 && dv == 0)
                {            // both segments are points
                    if (S1.SP != S2.SP)         // they are distinct  points
                        return 0;
                    I0 = S1.SP;                 // they are the same point
                    return 1;
                }
                if (du == 0)
                {                     // S1 is a single point
                    if (InSegment(S1.SP, S2) == 0)  // but is not in S2
                        return 0;
                    I0 = S1.SP;
                    return 1;
                }
                if (dv == 0)
                {                     // S2 a single point
                    if (InSegment(S2.SP, S1) == 0)  // but is not in S1
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
            double sI = TVector2D.Cross(v, w) / D;
            if (sI < 0 || sI > 1)                // no intersect with S1
                return 0;
            // get the intersect parameter for S2
            double tI = TVector2D.Cross(u, w) / D;
            if (tI < 0 || tI > 1)                // no intersect with S2
                return 0;
            I0 = S1.SP + sI * u;                // compute S1 intersect point
            return 1;
        }

        /// <summary>
        /// find the 3D intersection of a segment and a plane
        /// </summary>
        /// <param name="S">a segment</param>
        /// <param name="Pn">Pn = a plane = {Point V0;  Vector Normal;}</param>
        /// <param name="I">the intersect point (when it exists)</param>
        /// <returns>
        /// 0 = disjoint (no intersection)
        /// 1 =  intersection in the unique point *I0
        /// 2 = the  segment lies in the plane
        /// </returns>
        public static int Intersect3D_SegmentPlane(TLineSegment3d S, TPlane3D Pn, out TPoint3D I)
        {
            I = default;

            TVector3D u = (S.EP - S.SP).ToVector();
            TVector3D w = (S.SP - Pn.Origin).ToVector();

            double D = (Pn.Normal * u);
            double N = -(Pn.Normal * w);

            if (Math.Abs(D) < SMALL_NUM)
            {           // segment is parallel to plane
                if (N == 0)                      // segment lies in plane
                    return 2;
                else
                    return 0;                    // no intersection
            }
            // they are not parallel
            // compute intersect param
            double sI = N / D;
            if (sI < 0 || sI > 1)
                return 0;                        // no intersection

            I = S.SP + sI * u;                  // compute segment intersect point
            return 1;
        }

        /// <summary>
        /// find the 3D intersection of two planes
        /// </summary>
        /// <param name="Pn1"> plane 1</param>
        /// <param name="Pn2"> plane 2</param>
        /// <param name="L">the intersection line (when it exists)</param>
        /// <returns>
        /// 0 = disjoint (no intersection)
        /// 1 = the two  planes coincide
        /// 2 =  intersection in the unique line *L
        /// </returns>
        public static int Intersect3D_2Planes(TPlane3D Pn1, TPlane3D Pn2, out TLine3D L)
        {
            L = default;

            TVector3D u = TVector3D.CrossProduct(Pn1.Normal, Pn2.Normal);          // cross product
            double ax = (u.X >= 0 ? u.X : -u.X);
            double ay = (u.Y >= 0 ? u.Y : -u.Y);
            double az = (u.Z >= 0 ? u.Z : -u.Z);

            // test if the two planes are parallel
            if ((ax + ay + az) < SMALL_NUM)
            {        // Pn1 and Pn2 are near parallel
                     // test if disjoint or coincide
                TVector3D v = (Pn2.Origin - Pn1.Origin).ToVector();
                if ((Pn1.Normal * v) == 0)          // Pn2.V0 lies in Pn1
                    return 1;                    // Pn1 and Pn2 coincide
                else
                    return 0;                    // Pn1 and Pn2 are disjoint
            }

            // Pn1 and Pn2 intersect in a line
            // first determine max abs coordinate of cross product
            int maxc;                       // max coordinate
            if (ax > ay)
            {
                if (ax > az)
                    maxc = 1;
                else maxc = 3;
            }
            else
            {
                if (ay > az)
                    maxc = 2;
                else maxc = 3;
            }

            // next, to get a point on the intersect line
            // zero the max coord, and solve for the other two
            TPoint3D iP = default;                // intersect point
            double d1, d2;            // the constants in the 2 plane equations
            d1 = -(Pn1.Normal * Pn1.Origin.ToVector());  // note: could be pre-stored  with plane
            d2 = -(Pn2.Normal * Pn2.Origin.ToVector());  // ditto

            switch (maxc)
            {             // select max coordinate
                case 1:                     // intersect with X=0
                    iP.X = 0;
                    iP.Y = (d2 * Pn1.Normal.Z - d1 * Pn2.Normal.Z) / u.X;
                    iP.Z = (d1 * Pn2.Normal.Y - d2 * Pn1.Normal.Y) / u.X;
                    break;
                case 2:                     // intersect with Y=0
                    iP.X = (d1 * Pn2.Normal.Z - d2 * Pn1.Normal.Z) / u.Y;
                    iP.Y = 0;
                    iP.Z = (d2 * Pn1.Normal.X - d1 * Pn2.Normal.X) / u.Y;
                    break;
                case 3:                     // intersect with Z=0
                    iP.X = (d2 * Pn1.Normal.Y - d1 * Pn2.Normal.Y) / u.Z;
                    iP.Y = (d1 * Pn2.Normal.X - d2 * Pn1.Normal.X) / u.Z;
                    iP.Z = 0;
                    break;
            }

            L = new TLine3D(iP, iP + u);
            return 2;
        }
        #endregion
    }
}
