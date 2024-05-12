using System.Security.Policy;

namespace tmath.pga
{
    public static class ConvexHullApproximation
    {
        const int NONE = -1;

        struct Bin
        {
            public int min; // index of min point P[] in bin  (>=0 or NONE)
            public int max; // index of min point P[] in bin  (>=0 or NONE)
        }

        public static int NearHull_2D(TPoint2DCollection P, int k, out TPoint2DCollection H)
        {
            int n = P.Count;
            H = new TPoint2DCollection();
            for (int i = 0; i < n; i++) H.Add(TPoint2D.NULL);

            int minmin = 0, minmax = 0;
            int maxmin = 0, maxmax = 0;
            double xmin = P[0].X, xmax = P[0].X;

            TPoint2D cP;                  // the current point being considered

            int bot = 0, top = (-1);  // indices for bottom and top of the stack

            // Get the points with (1) min-max x-coord, and (2) min-max y-coord
            for (int i = 1; i < n; i++)
            {
                cP = P[i];
                if (cP.X <= xmin)
                {
                    if (cP.X < xmin)
                    {         // new xmin
                        xmin = cP.X;
                        minmin = minmax = i;
                    }
                    else
                    {                       // another xmin
                        if (cP.Y < P[minmin].Y)
                            minmin = i;
                        else if (cP.Y > P[minmax].Y)
                            minmax = i;
                    }
                }
                if (cP.X >= xmax)
                {
                    if (cP.X > xmax)
                    {          // new xmax
                        xmax = cP.X;
                        maxmin = maxmax = i;
                    }
                    else
                    {                       // another xmax
                        if (cP.Y < P[maxmin].Y)
                            maxmin = i;
                        else if (cP.Y > P[maxmax].Y)
                            maxmax = i;
                    }
                }
            }
            if (xmin == xmax)
            {      //  degenerate case: all x-coords == xmin
                H[++top] = P[minmin];            // a point, or
                if (minmax != minmin)            // a nontrivial segment
                    H[++top] = P[minmax];
                H.RemoveAll(p => p == TPoint2D.NULL);
                return top + 1;                    // one or two points
            }

            // Next, get the max and min points in the k range bins
            Bin[] B = new Bin[k + 2];   // first allocate the bins
            B[0].min = minmin; B[0].max = minmax;        // set bin 0
            B[k + 1].min = maxmin; B[k + 1].max = maxmax;      // set bin k+1
            for (int b = 1; b <= k; b++)
            { // initially nothing is in the other bins
                B[b].min = B[b].max = NONE;
            }
            for (int b, i = 0; i < n; i++)
            {
                cP = P[i];
                if (cP.X == xmin || cP.X == xmax)  // already have bins 0 and k+1 
                    continue;
                // check if a lower or upper point
                if (TPoint2D.IsLeft(P[minmin], P[maxmin], cP) < 0)
                {  // below lower line
                    b = (int)(k * (cP.X - xmin) / (xmax - xmin)) + 1;  // bin #
                    if (B[b].min == NONE)       // no min point in this range
                        B[b].min = i;           //  first min
                    else if (cP.Y < P[B[b].min].Y)
                        B[b].min = i;           // new  min
                    continue;
                }
                if (TPoint2D.IsLeft(P[minmax], P[maxmax], cP) > 0)
                {  // above upper line
                    b = (int)(k * (cP.X - xmin) / (xmax - xmin)) + 1;  // bin #
                    if (B[b].max == NONE)        // no max point in this range
                        B[b].max = i;           //  first max
                    else if (cP.Y > P[B[b].max].Y)
                        B[b].max = i;           // new  max
                    continue;
                }
            }

            // Now, use the chain algorithm to get the lower and upper  hulls
            // the output array H[] will be used as the stack
            // First, compute the lower hull on the stack H
            for (int i = 0; i <= k + 1; ++i)
            {
                if (B[i].min == NONE)   // no min point in this range
                    continue;
                cP = P[B[i].min];    // select the current min point

                while (top > 0)         // there are at least 2 points on the stack
                {
                    // test if current point is left of the line at the stack top
                    if (TPoint2D.IsLeft(H[top - 1], H[top], cP) > 0)
                        break;         // cP is a new hull vertex
                    else
                        top--;         // pop top point off stack
                }
                H[++top] = cP;         // push current point onto stack
            }

            // Next, compute the upper hull on the stack H above the bottom hull
            if (maxmax != maxmin)       // if  distinct xmax points
                H[++top] = P[maxmax];  // push maxmax point onto stack
            bot = top;                  // the bottom point of the upper hull stack
            for (int i = k; i >= 0; --i)
            {
                if (B[i].max == NONE)   // no max  point in this range
                    continue;
                cP = P[B[i].max];    //  select the current max point

                while (top > bot)       // at least 2 points on the upper stack
                {
                    // test if  current point is left of the line at the stack top
                    if (TPoint2D.IsLeft(H[top - 1], H[top], cP) > 0)
                        break;         // current point is a new hull vertex
                    else
                        top--;         // pop top point off stack
                }
                H[++top] = cP;         // push current point onto stack
            }
            if (minmax != minmin)
                H[++top] = P[minmin];   // push joining endpoint onto stack

            H.RemoveAll(p => p == TPoint2D.NULL);
            return top + 1;               // # of points on the stack
        }
    }
}
