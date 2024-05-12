namespace tmath.pga
{
    public static class PolygonTangents
    {
        private static bool Above(TPoint2D P, TPoint2D vi, TPoint2D vj) => TPoint2D.IsLeft(P, vi, vj) > 0;
        private static bool Below(TPoint2D P, TPoint2D vi, TPoint2D vj) => TPoint2D.IsLeft(P, vi, vj) < 0;

        // brute_force;
        public static void Tangent_PointToPoly(TPoint2D P, TPoint2DCollection V, out int rtan, out int ltan)
        {
            double e_prev, e_next; // V[i] previous and next turn direction

            rtan = 0;
            ltan = 0;

            e_prev = TPoint2D.IsLeft(V[0], V[1], P);
            for (int i = 1; i < V.Count; i++)
            {
                e_next = TPoint2D.IsLeft(V[i], V[i + 1], P);
                if (e_prev <= 0 && e_next > 0)
                {
                    if (!Below(P, V[i], V[rtan])) rtan = i;
                }
                else if (e_prev > 0 && e_next <= 0)
                {
                    if (!Above(P, V[i], V[ltan])) ltan = i;
                }
                e_prev = e_next;
            }
            return;
        }

        /// <summary>
        /// fast binary search for tangents to a convex polygon
        /// </summary>
        /// <param name="P">a 2D point (exterior to the polygon)</param>
        /// <param name="V">array of vertices for a 2D convex polygon with V[n] = V[0]</param>
        /// <param name="rtan">index of rightmost tangent point</param>
        /// <param name="ltan">index of leftmost tangent point</param>
        public static void Tangent_PointPolyC(TPoint2D P, TPoint2DCollection V, out int rtan, out int ltan)
        {
            rtan = Rtangent_PointPolyC(P, V);
            ltan = Ltangent_PointPolyC(P, V);
        }

        /// <summary>
        /// binary search for convex polygon right tangent
        /// </summary>
        /// <param name="P">a 2D point (exterior to the polygon)</param>
        /// <param name="V">array of vertices for a 2D convex polygon with V[n] = V[0]</param>
        /// <returns></returns>
        private static int Rtangent_PointPolyC(TPoint2D P, TPoint2DCollection V)
        {
            int n = V.Count;
            // use binary search for large convex polygons
            int a, b, c;            // indices for edge chain endpoints
            bool upA, dnC;           // test for up direction of edges a and c

            // rightmost tangent = maximum for the isLeft() ordering
            // test if V[0] is a local maximum
            if (Below(P, V[1], V[0]) && !Above(P, V[n - 1], V[0]))
                return 0;               // V[0] is the maximum tangent point

            for (a = 0, b = n; ;)
            {          // start chain = [0,n] with V[n]=V[0]
                c = (a + b) / 2;        // midpoint of [a,b], and 0<c<n
                dnC = Below(P, V[c + 1], V[c]);
                if (dnC && !Above(P, V[c - 1], V[c]))
                    return c;          // V[c] is the maximum tangent point

                // no max yet, so continue with the binary search
                // pick one of the two subchains [a,c] or [c,b]
                upA = Above(P, V[a + 1], V[a]);
                if (upA)
                {                       // edge a points up
                    if (dnC)                         // edge c points down
                        b = c;                           // select [a,c]
                    else
                    {                           // edge c points up
                        if (Above(P, V[a], V[c]))     // V[a] above V[c]
                            b = c;                       // select [a,c]
                        else                          // V[a] below V[c]
                            a = c;                       // select [c,b]
                    }
                }
                else
                {                           // edge a points down
                    if (!dnC)                        // edge c points up
                        a = c;                           // select [c,b]
                    else
                    {                           // edge c points down
                        if (Below(P, V[a], V[c]))     // V[a] below V[c]
                            b = c;                       // select [a,c]
                        else                          // V[a] above V[c]
                            a = c;                       // select [c,b]
                    }
                }
            }
        }


        private static int Ltangent_PointPolyC(TPoint2D P, TPoint2DCollection V)
        {
            int n = V.Count;
            // use binary search for large convex polygons
            int a, b, c;            // indices for edge chain endpoints
            bool dnA, dnC;           // test for down direction of edges a and c

            // leftmost tangent = minimum for the isLeft() ordering
            // test if V[0] is a local minimum
            if (Above(P, V[n - 1], V[0]) && !Below(P, V[1], V[0]))
                return 0;               // V[0] is the minimum tangent point

            for (a = 0, b = n; ;)
            {          // start chain = [0,n] with V[n] = V[0]
                c = (a + b) / 2;        // midpoint of [a,b], and 0<c<n
                dnC = Below(P, V[c + 1], V[c]);
                if (Above(P, V[c - 1], V[c]) && !dnC)
                    return c;          // V[c] is the minimum tangent point

                // no min yet, so continue with the binary search
                // pick one of the two subchains [a,c] or [c,b]
                dnA = Below(P, V[a + 1], V[a]);
                if (dnA)
                {                       // edge a points down
                    if (!dnC)                        // edge c points up
                        b = c;                           // select [a,c]
                    else
                    {                           // edge c points down
                        if (Below(P, V[a], V[c]))     // V[a] below V[c]
                            b = c;                       // select [a,c]
                        else                          // V[a] above V[c]
                            a = c;                       // select [c,b]
                    }
                }
                else
                {                           // edge a points up
                    if (dnC)                         // edge c points down
                        a = c;                           // select [c,b]
                    else
                    {                           // edge c points up
                        if (Above(P, V[a], V[c]))     // V[a] above V[c]
                            b = c;                       // select [a,c]
                        else                          // V[a] below V[c]
                            a = c;                       // select [c,b]
                    }
                }
            }
        }
    }
}
