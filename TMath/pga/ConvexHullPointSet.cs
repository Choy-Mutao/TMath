using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmath.pga
{
    public static class ConvexHullPointSet
    {

        /// <summary>
        /// Andrew's monotone chain 2D convex hull algorithm
        /// </summary>
        /// <param name="P"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        /// 
        [Obsolete("error")]
        public static int ChainHull_2D(TPoint2DCollection P, out TPoint2DCollection H)
        {
            H = new TPoint2DCollection();
            var n = P.Count;
            for (int hi = 0; hi < n; hi++) H.Add(TPoint2D.NULL);
            // the output array H[] will be used as the stack
            int bot = 0, top = (-1); // indices for bottom and top of the stack
            int i;

            // Get the indices of points with min x-coord and min|max y-coord
            int minmin = 0, minmax;
            double xmin = P[0].X;
            for (i = 1; i < n; i++)
                if (P[i].X != xmin) break;
            minmax = i - 1;
            if(minmax == n-1) // degenerate case: all x-coords == xmin
            {
                H[++top] = P[minmin];
                if (P[minmax].Y != P[minmin].Y)  // a nontrivial segment
                    H[++top] = P[minmax];
                H[++top] = P[minmin]; // add polygon endpoint
                H.RemoveAll(p => p == TPoint2D.NULL);
                return top + 1;
            }

            // Get the indices of points with max x-coord and min|max y-coord
            int maxmin, maxmax = n - 1;
            double xmax = P[n-1].X;
            for (i = n - 2; i >= 0; i--)
                if (P[i].X != xmax) break;
            maxmin = i + 1;

            // Compute the lower hull on the stack H
            H[++top] = P[minmin]; // push minmin point onto stack;
            i = minmax;
            while(++i < maxmin)
            {
                // the lower line joins P[minmin] with P[maxmin]
                if (TPoint2D.IsLeft(P[minmin], P[maxmin], P[i]) >= 0 && i < maxmin)
                    continue; // ignore P[i] above or on the lower line

                while(top>0) // there are at least 2 points on the stack
                {
                    // test if P[i] is left on the line at the stack top
                    if (TPoint2D.IsLeft(H[top - 1], H[top], P[i]) > 0)
                        break;
                    else
                        top--; // pop top point of stack;
                }
                H[++top] = P[i];
            }

            // Next, compute the upper hull on the stack H above  the bottom hull
            if (maxmax != maxmin)      // if  distinct xmax points
                H[++top] = P[maxmax];  // push maxmax point onto stack
            bot = top;                  // the bottom point of the upper hull stack
            i = maxmin;
            while (--i >= minmax)
            {
                // the upper line joins P[maxmax]  with P[minmax]
                if (TPoint2D.IsLeft(P[maxmax], P[minmax], P[i]) >= 0 && i > minmax)
                    continue;           // ignore P[i] below or on the upper line

                while (top > bot)     // at least 2 points on the upper stack
                {
                    // test if  P[i] is left of the line at the stack top
                    if (TPoint2D.IsLeft(H[top - 1], H[top], P[i]) > 0)
                        break;         // P[i] is a new hull  vertex
                    else
                        top--;         // pop top point off  stack
                }
                H[++top] = P[i];        // push P[i] onto stack
            }
            if (minmax != minmin)
                H[++top] = P[minmin];  // push  joining endpoint onto stack

            H.RemoveAll(p => p == TPoint2D.NULL);
            return top + 1;

        }
    }
}
