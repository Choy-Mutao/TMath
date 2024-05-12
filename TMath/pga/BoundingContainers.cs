
using System;
using tmath.geo_math.bounding;

namespace tmath.pga
{
    public static class BoundingContainers
    {
        /// <summary>
        /// In 1990, Jack Ritter proposed a simple algorithm to find a non-minimal bounding sphere.Ritter's algorithm runs in time O(nd) on inputs consisting of n points in d-dimensional space, which makes it very efficient. However it gives only a coarse result which is ususally 5% to 20% larger than the optimum;
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Ball<TPoint2D, TVector2D> FastBall(TPoint2DCollection P)
        {
            TPoint2D center = new TPoint2D(); // Center of the ball;
            int n = P.Count;
            double rad, rad2; // radius, radius squared;
            double xmin, xmax, ymin, ymax; // bounding box extremes;
            int Pxmin = -1, Pxmax = -1, Pymin = -1, Pymax = -1; // index of P[] at box extreme;

            // find a large diameter to start with
            // first get the bounding box and P[] extreme points for it;
            xmin = xmax = P[0].X;
            ymin = ymax = P[0].Y;
            for (int i = 1; i < n; i++)
            {
                if (P[i].X < xmin)
                {
                    xmin = P[i].X;
                    Pxmin = i;
                }
                else if (P[i].X > xmax)
                {
                    xmax = P[i].X;
                    Pxmax = i;
                }
                if (P[i].Y < ymin)
                {
                    ymin = P[i].Y;
                    Pymin = i;
                }
                else if (P[i].Y > ymax)
                {
                    ymax = P[i].Y;
                    Pymax = i;
                }
            }

            // select the largest extent as an initial diameter for the ball
            TVector2D dPx = (P[Pxmax] - P[Pxmin]).ToVector(); // diff of Px max and min;
            TVector2D dPy = (P[Pymax] - P[Pymin]).ToVector(); // diff of Py max and min;
            double dx2 = dPx.LengthSq(); // Px diff squared
            double dy2 = dPy.LengthSq(); // Py diff squared
            if (dx2 > dy2) // x direction is largest extent
            {
                center = P[Pxmin] + (dPx / 2.0); // Center = midpoint of extremes
                rad2 = (P[Pxmax] - center).ToVector().LengthSq(); // radius squared
            }
            else // y direction is largest extent
            {
                center = P[Pymin] + (dPy / 2.0);          // Center = midpoint of extremes
                rad2 = (P[Pxmax] - center).ToVector().LengthSq();          // radius squared
            }
            rad = Math.Sqrt(rad2);

            // now check that all points P[i] are in the ball
            // and if not , expend the ball just enough to include them;
            TVector2D dP = new TVector2D();
            double dist = 0, dist2 = 0;
            for(int i = 0; i < n; i++)
            {
                dP = (P[i] - center).ToVector();
                dist2 = dP.LengthSq();
                if (dist2 <= rad2) continue; // P[i] is inside the ball already
                // P[i] not in ball, so expand ball to include it;
                dist = Math.Sqrt(dist2);
                rad = (rad + dist) * 0.5; // enlarge radius just enough
                rad2 = rad * rad;
                center += ((dist - rad) / dist) * dP; // shift Center toward P[i]
            }
            return new Ball<TPoint2D, TVector2D>(center, rad);
        }
    }
}
