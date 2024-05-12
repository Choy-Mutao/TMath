using tmath.geometry;

namespace tmath.pga
{
    public static class ConvexPolygonExtremePoints
    {
        /// <summary>
        /// find a polygon's max vertex in a specified direction
        /// 
        /// Note: need to check if the open polyline also has extreme point?
        /// </summary>
        /// <param name="U">a 2D direction vector</param>
        /// <param name="V"></param>
        /// <returns>the Index of the maximum vertex</returns>
        public static int PolyMax_2D(TVector2D U, in TPoint2DCollection V)
        {
            int n = V.Count;
            if (n < 10)                                 // use brute force search for small polygons
            {
                int max = 0;
                for (int i = 1; i < n; i++)             // for each point in {V1, ..., Vn-1}
                    if (TPoint2D.Above(U, V[i], V[max]))// if V[i] is above prior V[max]
                        max = i;                        // new max index = i
                return max;
            }

            // use binary search for large polygons
            int a, b, c;                                // indices for edge chain endpoints;
            TVector2D A, C;                             // dege vectors at V[a] and V[c];
            bool upA, upC;                              // test for "up" direction of A and C;

            a = 0; b = n;                               // start chain = [0, n], whith V[n] = V[0];
            A = (V[1] - V[0]).ToVector();                            // first A
            upA = TVector2D.Up(U, A);

            if (upA == false && TPoint2D.Above(U, V[n - 1], V[0]) == false)
                return 0;

            for (; ; )
            {
                c = (a + b) / 2;                        // midpoint index of [a,b] and 0 < c < n
                C = (V[c + 1] - V[c]).ToVector();
                upC = TVector2D.Up(U, C);
                if (!upC && !TPoint2D.Above(U, V[c - 1], V[c])) // V[c] is a local maxium
                    return c;

                // no max yet, so continue with the binary search
                // pick one of the two subchains [a,c] or [c,b]
                if (upA)                                    // A points up
                {
                    if (!upC)                               // C points down
                    {
                        b = c;                              // select [a,c]
                    }
                    else                                    // C points up
                    {
                        if (TPoint2D.Above(U, V[a], V[c]))  // V[a] above V[c]
                        {
                            b = c;                          // select [a,c]
                        }
                        else                                // V[a] above V[c],  select[c,b]
                        {
                            a = c;
                            A = C;
                            upA = upC;
                        }
                    }
                }
                else                                        // A points down
                {
                    if(upC)                                 // C point up
                    {
                        a = c;                              // select [c,b]
                        A = C;
                        upA = upC;
                    }
                    else
                    {
                        if(TPoint2D.Below(U, V[a], V[c]))
                        {
                            b = c;
                        }
                        else
                        {
                            a = c;
                            A = C;
                            upA = upC;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// find the distance from a polygon to a line
        /// </summary>
        /// <param name="V">array vertices of a proper convex polygon</param>
        /// <param name="L">a Line(defined by 2 points P0 and P1)</param>
        /// <returns>minimum distance from V[] to L</returns>
        public static double Dist2D_Poly_To_Line(in TPoint2DCollection V, TLine2D L, out TPoint2D extremePoint, out TPoint2D online)
        {
            TVector2D U = new TVector2D(), N = new TVector2D();
            int max;

            // get a leftward normal N to L
            N.X = -(L.P1.Y - L.P0.Y);
            N.Y = (L.P1.X - L.P0.X);

            // get a normal U to L with V[0] on U-backside
            if (N.Dot((V[0] - L.P0).ToVector()) <= 0)
                U = N;
            else 
                U = -N;

            max = PolyMax_2D(U, V); // max vertex index of V in U directon
            extremePoint = V[max];

            if (U.Dot((V[max] - L.P0).ToVector()) > 0)
            {
                online = TPoint2D.NegativeInfinity;
                return 0;
            }
            else
                return L.DistanceToPoint(V[max], out online);
        }
    }
}
