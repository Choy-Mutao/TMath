namespace tmath.algorithms.sweepline
{
    public enum SEG_SIDE
    {
        LEFT,
        RIGHT,
    }

    public class SweepEvent
    {
        public int edge; // polygon edge i is V[i] to V[i+1]
        public SEG_SIDE type; // event type: LEFT or RIGHT vertex
        public TPoint2D eV; // event vertex;
        public SL_Seg seg
        {
            get;
            set;
        } // segment in tree;
        public SweepEvent otherEnd; // segment is [this, otherEnd]
    }
}
