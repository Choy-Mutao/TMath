using tmath.geometry;

namespace tmath.pga
{
    public static class Lines
    {
        #region  Fields
        #endregion

        #region Public Methods
        /// <summary>
        /// find the closest 2D Point to a Line
        /// </summary>
        /// <param name="P">an array P[] of n points, and a Line L</param>
        /// <param name="L"></param>
        /// <returns>the index i of the Point P[i] closest to L</returns>
        public static int Cloest2D_Point_To_Line(TPoint2DCollection P, TLine2D L)
        {
            // Get coefficients of the implicit line equation.
            // Do NOT normalize since scaling by a constant
            // is irrelevant for just comparing distances.
            double a = L.P0.Y - L.P1.Y;
            double b = L.P1.X - L.P0.X;
            double c = L.P0.X * L.P1.Y - L.P1.X * L.P0.Y;

            // initialize min index and distance to P[0]
            int mi = 0;
            double min = a * P[0].X + b * P[0].Y + c;
            if (min < 0) min = -min;     // absolute value

            int n = P.Count;
            // loop through Point array testing for min distance to L
            for (int i = 1; i < n; i++)
            {
                // just use dist squared (sqrt not  needed for comparison)
                double dist = a * P[i].X + b * P[i].Y + c;
                if (dist < 0) dist = -dist;    // absolute value
                if (dist < min)
                {      // this point is closer
                    mi = i;              // so have a new minimum
                    min = dist;
                }
            }
            return mi;     // the index of the closest  Point P[mi]
        }

        /// <summary>
        /// get the distance of a point to a line
        /// </summary>
        /// <param name="P"> a Point P and a Line L (in any dimension) </param>
        /// <param name="L"> the shortest distance from P to L </param>
        /// <returns></returns>
        public static double Dist_Point_To_Line(TPoint2D P, TLine2D L)
        {
            TVector2D v = (L.P1 - L.P0).ToVector();
            TVector2D w = (P - L.P0).ToVector();

            double c1 = w * v;
            double c2 = v * v;
            double b = c1 / c2;

            TPoint2D Pb = L.P0 + b * v;
            return P.DistanceTo(Pb);
        }

        public static double Dist_Point_To_Segment(TPoint2D P, TLineSegment2d S)
        {
            TVector2D v = (S.EP - S.SP).ToVector();
            TVector2D w = (P - S.SP).ToVector();

            double c1 = w * v;
            if (c1 <= 0)
                return P.DistanceTo(S.SP);

            double c2 = v * v;
            if (c2 <= c1)
                return P.DistanceTo(S.EP);

            double b = c1 / c2;
            TPoint2D Pb = S.SP + b * v;
            return P.DistanceTo(Pb);
        }
        #endregion
    }
}
