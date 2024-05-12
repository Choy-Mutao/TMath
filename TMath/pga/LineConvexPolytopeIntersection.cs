using System;
using tmath.geometry;

namespace tmath.pga
{
    internal class LineConvexPolytopeIntersection
    {
        const double SMALL_NUM = 0.00000001; // anything that avoids division overflow

        /// <summary>
        /// winding number test for a point in a polygon
        /// </summary>
        /// <param name="P"></param>
        /// <param name="V"></param>
        /// <returns>the winding number(=0 only when P is outside)</returns>
        int wn_PnPoly(TPoint2D P, TPoint2DCollection V)
        {
            int n = V.Count;
            int wn = 0; // the winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++) // edge from V[i] to V[i+1]
            {
                if (V[i].Y <= P.Y) // start y <= P.y
                {
                    if (V[i + 1].Y > P.Y) // an upward crossing
                        if (TPoint2D.IsLeft(V[i], V[i + 1], P) > 0) // P left of edge
                            ++wn; // have a valid up intersect
                }
                else // start y > P.y (no test needed)
                {
                    if (V[i + 1].Y <= P.Y)  // a downward corssing
                        if (TPoint2D.IsLeft(V[i], V[i + 1], P) < 0) // P right of edge
                            --wn; // have a valid down intersect
                }
            }
            return wn;
        }

        public int Intersection2D_SegPoly(TLineSegment2d S, TPolyLine2D V, out TLineSegment2d IS)
        {
            IS = default;
            int n = V.Count;
            if (S.SP == S.EP)
            {
                int ptst = wn_PnPoly(S.SP, V.ToTPoint2DCollection()); // winding number test
                if (ptst != 0) // S.P0 is not outside the polygon
                {
                    IS = S; // the point S.SP is the intersection
                }
                return ptst;
            }

            double tE = 0;              // the maximum entering segment parameter
            double tL = 1;              // the minimum leaving segment parameter
            double t, N, D;             // intersect parameter t = N / D

            TVector2D dS = (S.EP - S.SP).ToVector();    // the  segment direction vector
            TVector2D e;                // edge vector

            for (int i = 0; i < n; i++)   // process polygon edge V[i]V[i+1]
            {
                e = (V[i + 1] - V[i]).ToVector();
                N = e.Perp((S.SP - V[i]).ToVector()); // = -dot(ne, S.P0 - V[i])
                D = -e.Perp((dS));      // = dot(ne, dS)
                if (Math.Abs(D) < SMALL_NUM)    // S is nearly parallel to this edge
                {
                    if (N < 0)          // P0 is outside this edge, so
                        return 0;       // S is outside the polygon
                    else                // S cannot cross this edge, so
                        continue;       // ignore this edge
                }

                t = N / D;
                if (D < 0)              // segment S is entering across this edge
                {
                    if (t > tE)         // new max tE
                    {
                        tE = t;
                        if (tE > tL)    // S enters after leaving polygon
                            return 0;
                    }
                }
                else                    // segment S is leaving across this edge
                {
                    if (t < tL)         // new min tL
                    {
                        tL = t;
                        if (tL < tE)    // S leaves before entering polygon
                            return 0;
                    }
                }
            }

            // tE <= tL implies that there is a valid intersection subsegment
            IS.SP = S.SP + tE * dS;  // = P(tE) = point where S enters polygon
            IS.EP = S.SP + tL * dS;  // = P(tL) = point where S leaves polygon
            return 1;
        }
    }
}
