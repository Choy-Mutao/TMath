using System;

namespace tmath.algorithms.sweepline
{
    public class EventQueue
    {
#if DEBUG
        public int ne; // total number of events in array
        public int ix; // index of next event on queue
        public SweepEvent[] Edata; // array of all events;
        //public SweepEvent[] Eq; // sorted list of event items;
#else
        int ne; // total number of events in array
        int ix; // index of next event on queue
        SweepEvent[] Edata; // array of all events;
        //SweepEvent[] Eq; // sorted list of event items;
#endif

        public EventQueue(TPoint2DCollection P)
        {
            ix = 0;
            ne = 2 * P.Count; // 2 vertex events for each edge
            Edata = new SweepEvent[ne];

            P.MakeOpened();
            // Initialize event queue with edge segment endpoints
            for (int i = 0; i < P.Count; i++) // init data for edge i
            {
                Edata[2 * i] = new SweepEvent();
                Edata[2 * i + 1] = new SweepEvent();

                Edata[2 * i].edge = i;
                Edata[2 * i + 1].edge = i;

                Edata[2 * i].eV = P[i];
                TPoint2D pi1 = (i + 1 < P.Count) ? P[i + 1] : P[0];
                Edata[2 * i + 1].eV = pi1;

                Edata[2 * i].otherEnd = Edata[2 * i + 1];
                Edata[2 * i + 1].otherEnd = Edata[2 * i];

                Edata[2 * i].seg = Edata[2 * i + 1].seg = null;

                if (TPoint2D.XYOrder(P[i], pi1) < 0)
                {
                    Edata[2 * i].type = SEG_SIDE.LEFT;
                    Edata[2 * i + 1].type = SEG_SIDE.RIGHT;
                }
                else
                {
                    Edata[2 * i].type = SEG_SIDE.RIGHT;
                    Edata[2 * i + 1].type = SEG_SIDE.LEFT;
                }
            }

            Array.Sort(Edata, Event_Compare);
        }

        private int Event_Compare(SweepEvent pe1, SweepEvent pe2)
        {
            int r = TPoint2D.XYOrder((pe1).eV, (pe2).eV);
            if (r == 0)
            {
                if (pe1.type == pe2.type) return 0;
                if (pe1.type == SEG_SIDE.LEFT) return -1;
                else return 1;
            }
            else
                return r;
        }

        public SweepEvent Next()
        {
            if (ix >= ne) return null; // retuen null
            else return Edata[ix++];
        }
    }
}
