using tmath.algorithms.sweepline;

namespace tmath.pga
{
    public class ShamosHoeyAVLCode
    {
        public bool Simple_Polygon(TPoint2DCollection Pn)
        {
            EventQueue Eq = new EventQueue(Pn);
            SweepLine SL = new SweepLine(Pn);

            SweepEvent e; // the current event;
            SL_Seg s; // the current sl segment;

            // This loop processes all events in the sorted queue
            // Events are only left or right vertices since
            // No new events will be added (an intersect => Done)
            while ((e = Eq.Next()) != null)
            {      // while there are events
                if (e.type == SEG_SIDE.LEFT)
                {     // process a left vertex
                    s = SL.Add(ref e);         // add it to the sweep line
                    if (SL.Intersect(s, s.above))
                        return false;          // Pn is NOT simple
                    if (SL.Intersect(s, s.below))
                        return false;          // Pn is NOT simple
                }
                else
                {                     // process a right vertex
                    s = e.otherEnd.seg;
                    if (SL.Intersect(s.above, s.below))
                        return false;         // Pn is NOT simple
                    SL.Remove(s);     // remove it from the sweep line
                }
            }
            return true;      // Pn is simple
        }
    }
}
