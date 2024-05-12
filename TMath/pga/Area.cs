using System;

namespace tmath.pga
{
    public static class Area
    {
        /// <summary>
        /// Test if A Point is Left | On | Right of an infinite 2D line;
        /// </summary>
        /// <param name="P0">point on line</param>
        /// <param name="P1">point on line</param>
        /// <param name="P2">tested point</param>
        /// <returns>
        /// >0 for P0 left of the line through P0 to P1
        /// =0 for P0 on the line
        /// <0 for P2 right of the line
        /// </returns>
        public static double IsLeft(TPoint2D P0, TPoint2D P1, TPoint2D P2)
        {
            return ((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
        }

        /// <summary>
        /// Test the orientation of a 2D triangle
        /// </summary>
        /// <param name="V0"></param>
        /// <param name="V1"></param>
        /// <param name="V2"></param>
        /// <returns>
        /// >0 for counter-clockwise
        /// 0= for none(degenerate)
        /// <0 for clockwise
        /// </returns>
        static int Orientation2D_Triangle(TPoint2D V0, TPoint2D V1, TPoint2D V2)
        {
            return (int)IsLeft(V0, V1, V2);
        }

        /// <summary>
        /// Compute the area of a 2D triangle
        /// </summary>
        /// <param name="V0"></param>
        /// <param name="V1"></param>
        /// <param name="V2"></param>
        /// <returns>The float area of triangle T</returns>
        static double Area2D_Triangle(TPoint2D V0, TPoint2D V1, TPoint2D V2)
        {
            return IsLeft(V0, V1, V2) / 2.0;
        }


        /// <summary>
        /// Test the orientation of a simple 2D polygon
        /// Note: this algorithm is faster than computing the signed area;
        /// </summary>
        /// <param name="V">an array of n+1 vertex points with V[n] = V[0]</param>
        /// <returns></returns>
        static int Orientation2D_Polygon(TPoint2DCollection V)
        {
            V.MakeClosed();
            // first find right most lowest vertex of the polygon
            int rmin = 0; // index of the min
            double xmin = V[0].X;
            double ymin = V[0].Y;

            int n = V.Count;
            for(int i = 1; i<n; i++)
            {
                if (V[i].Y > ymin) continue;
                if (V[i].Y == ymin) // just as low
                    if (V[i].X < xmin) continue; // and to left
                rmin = i; // a new rightmost lowest vertex;
                xmin = V[i].X;
                ymin = V[i].Y;
            }

            // test orientation at the rmin vertex
            // ccw <=> the edge leaving  V[rmin] is left of the entering edge
            if (rmin > 0)
                return (int)IsLeft(V[n - 1], V[0], V[1]);
            else
                return (int)IsLeft(V[rmin - 1], V[rmin], V[rmin + 1]);
        }

        /// <summary>
        /// compute the area of a 2D polygon
        /// </summary>
        /// <param name="V">an area of n+1 vertex points with V[n] = V[0]</param>
        /// <returns>the area of the polygon</returns>
        static double Area2D_Polygon(TPoint2DCollection V)
        {
            V.MakeClosed();
            double area = 0;
            int i, j, k;
            int n = V.Count;
            if (n < 3) return 0;  // a degenerate polygon

            for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
            {
                area += V[i].X * (V[j].Y - V[k].Y);
            }
            area += V[n].X * (V[1].Y - V[n - 1].Y);  // wrap-around term
            return area / 2.0;
        }

        /// <summary>
        /// compute the area of a 3D planar polygon
        /// </summary>
        /// <param name="V">an array of n+1 points in a 2D plane with V[n]=V[0]</param>
        /// <param name="N">a normal vector of the polygon's plane</param>
        /// <returns></returns>
        static double Area3D_Polygon(TPoint3DCollection V, TVector3D N)
        {
            V.MakeClosed();
            double area = 0;
            double an, ax, ay, az; // abs value of normal and its coords
            int coord;           // coord to ignore: 1=X, 2=Y, 3=Z
            int i, j, k;         // loop indices

            int n = V.Count;
            if (n < 3) return 0;  // a degenerate polygon

            // select largest abs coordinate to ignore for projection
            ax = (N.X > 0 ? N.X : -N.X);    // abs X-coord
            ay = (N.Y > 0 ? N.Y : -N.Y);    // abs Y-coord
            az = (N.Z > 0 ? N.Z : -N.Z);    // abs Z-coord

            coord = 3;                    // ignore Z-coord
            if (ax > ay)
            {
                if (ax > az) coord = 1;   // ignore X-coord
            }
            else if (ay > az) coord = 2;  // ignore Y-coord

            // compute area of the 2D projection
            switch (coord)
            {
                case 1:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (V[i].Y * (V[j].Z - V[k].Z));
                    break;
                case 2:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (V[i].Z * (V[j].X - V[k].X));
                    break;
                case 3:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (V[i].X * (V[j].Y - V[k].Y));
                    break;
            }
            switch (coord)
            {    // wrap-around term
                case 1:
                    area += (V[n].Y * (V[1].Z - V[n - 1].Z));
                    break;
                case 2:
                    area += (V[n].Z * (V[1].X - V[n - 1].X));
                    break;
                case 3:
                    area += (V[n].X * (V[1].Y - V[n - 1].Y));
                    break;
            }

            // scale to get area before projection
            an = Math.Sqrt(ax * ax + ay * ay + az * az); // length of normal vector
            switch (coord)
            {
                case 1:
                    area *= (an / (2 * N.X));
                    break;
                case 2:
                    area *= (an / (2 * N.Y));
                    break;
                case 3:
                    area *= (an / (2 * N.Z));
                    break;
            }
            return area;
        }
    }
}
