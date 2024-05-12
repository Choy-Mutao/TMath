using System;
using System.Collections.Generic;
using tmath.gs.trees;

namespace tmath.algorithms.sweepline
{
    /// <summary>
    /// SweepLine segment data class,
    /// top-down
    /// </summary>
    public class SL_Seg : AVLComparable<SL_Seg>, IComparable<SL_Seg>
    {
        public int edge; // polygon edge i is V[i] to V[i+1]
        public TPoint2D lP; // leftmost vertex point
        public TPoint2D rP; // rightmost vertex point

        public SL_Seg above; // segment above this one
        public SL_Seg below; // segment below this one

        public SL_Seg() : base()
        {
            myKey = this;
        }

        public SL_Seg(TPoint2D p1, TPoint2D p2)
        {
            SetPoints(p1, p2);
        }

        public void SetPoints(TPoint2D p1, TPoint2D p2)
        {
            var o = TPoint2D.XYOrder(p1, p2);
            switch (o)
            {
                case -1:
                    lP = p1; rP = p2; break;
                case 0:
                    lP = p1; rP = p2; break;
                case 1:
                    lP = p2; rP = p1; break;
                default:
                    break;
            }
        }

        // return true if a is below b
        public static bool operator <(SL_Seg a, SL_Seg b)
        {
            if (a.lP.X <= b.lP.X)
            {
                double s = TPoint2D.IsLeft(a.lP, a.rP, b.lP);
                if (s != 0) return s > 0;
                else
                {
                    if (a.lP.X == a.rP.X) // special case of vertical line
                        return a.lP.Y < b.lP.Y;
                    else
                        return TPoint2D.IsLeft(a.lP, a.rP, b.rP) > 0;
                }
            }
            else
            {
                double s = TPoint2D.IsLeft(b.lP, b.rP, a.lP);
                if (s != 0) return s < 0;
                else return TPoint2D.IsLeft(b.lP, b.rP, a.rP) < 0;
            }
        }

        public static bool operator >(SL_Seg a, SL_Seg b) { return !(a == b || a < b); }

        public static bool operator ==(SL_Seg a, SL_Seg b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.edge == b.edge;
        }

        public static bool operator !=(SL_Seg a, SL_Seg b) { return !(a == b); }

        public int CompareTo(SL_Seg other)
        {
            if (this < other) return -1;
            else if (this == other) return 0;
            else return 1;
        }

        public override cmp_t Compare(SL_Seg key)
        {
            return key == this ? cmp_t.EQ_CMP : ((key < this) ? cmp_t.MIN_CMP : cmp_t.MAX_CMP);
        }

        public override bool Equals(object obj)
        {
            return obj is SL_Seg seg &&
                   EqualityComparer<SL_Seg>.Default.Equals(myKey, seg.myKey) &&
                   edge == seg.edge &&
                   lP.Equals(seg.lP) &&
                   rP.Equals(seg.rP) &&
                   EqualityComparer<SL_Seg>.Default.Equals(above, seg.above) &&
                   EqualityComparer<SL_Seg>.Default.Equals(below, seg.below);
        }

        public override int GetHashCode()
        {
            int hashCode = 1873184074;
            hashCode = hashCode * -1521134295 + EqualityComparer<SL_Seg>.Default.GetHashCode(myKey);
            hashCode = hashCode * -1521134295 + edge.GetHashCode();
            hashCode = hashCode * -1521134295 + lP.GetHashCode();
            hashCode = hashCode * -1521134295 + rP.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<SL_Seg>.Default.GetHashCode(above);
            hashCode = hashCode * -1521134295 + EqualityComparer<SL_Seg>.Default.GetHashCode(below);
            return hashCode;
        }
    }
}
