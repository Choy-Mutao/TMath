namespace tmath.pga
{
    public static class PointInPolygon
    {
        public static int CrossingNumber(TPoint2D P, TPoint2DCollection V)
        {

            int n = V.Count;
            int cn = 0; // the crossing number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++) // edge from V[i] to V[i+1]
            {

            }

            return cn;
        }
    }
}
