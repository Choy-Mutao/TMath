using System;
using System.Linq;

namespace tmath.gs.line_structue
{
    internal enum SweepEnum
    {
        FALSE = 0,
        TRUE = 1,
        LEFT = 0,
        RIGHT = 1,
    }


    /// <summary>
    /// only use for TPoint2DCollection
    /// </summary>
    public class SweepLine
    {

        public TPoint2DCollection Pn;
        BBT<SL_Seg> Tree;

        public SweepLine(TPoint2DCollection collection)
        {
            Pn = new TPoint2DCollection(collection);
        }

        internal SL_Seg Add(Event E)
        {
            // fill in SLseg element data
            SL_Seg s = new SL_Seg();
            s.edge = E.edge;

            // if it is being added, then it must be a LEFT edge event
            // but need to determine which endpoint is the left one
            TPoint2D v1 = Pn[s.edge];
            TPoint2D v2 = Pn[s.edge + 1];
            if (TPoint2D.XYOrder(v1, v2) < 0) // determine which is leftmost
            {
                s.lP = v1;
                s.rP = v2;
            }
            else
            {
                s.rP = v1;
                s.lP = v2;
            }

            // add a node to the balanced binary tree
            var nd = Tree.Insert(s);
            var nx = Tree.Next(nd);
            var np = Tree.Prev(nd);

            if (nx != null)
            {
                s.above = nx.val;
                s.above.below = s;
            }
            if (np != null)
            {
                s.below = np.val;
                s.below.above = s;
            }
            return s;
        }

        internal void Remove(SL_Seg s)
        {
            // remove the node from the balanced binary tree
            var nd = Tree.Find(s);
            if (nd == null)
                return;       // not there

            // get the above and below segments pointing to each other
            var nx = Tree.Next(nd);
            if (nx != null)
            {
                SL_Seg sx = (nx.val);
                sx.below = s.below;
            }
            var np = Tree.Prev(nd);
            if (np != null)
            {
                var sp = (np.val);
                sp.above = s.above;
            }
            Tree.Remove(nd);       // now  can safely remove it
        }

        internal SL_Seg Find(Event E)
        {
            // need a segment to find it in the tree
            SL_Seg s = new SL_Seg();
            s.edge = E.edge;
            s.above = null;
            s.below = null;

            TNode<SL_Seg> nd = Tree.Find(s);
            if (nd == null)
                return null;

            return nd.val;
        }

        // test intersect of 2 segments and return: 0=none, 1=intersect
        internal SweepEnum Intersect(SL_Seg s1, SL_Seg s2)
        {
            if (s1 == null || s2 == null)
                return SweepEnum.FALSE;       // no intersect if either segment doesn't exist

            // check for consecutive edges in polygon
            int e1 = s1.edge;
            int e2 = s2.edge;
            if (((e1 + 1) % Pn.Count == e2) || (e1 == (e2 + 1) % Pn.Count))
                return SweepEnum.FALSE;       // no non-simple intersect since consecutive

            // test for existence of an intersect point
            float lsign, rsign;
            lsign = TPoint2D.IsLeft(s1.lP, s1.rP, s2.lP);    //  s2 left point sign
            rsign = TPoint2D.IsLeft(s1.lP, s1.rP, s2.rP);    //  s2 right point sign
            if (lsign * rsign > 0) // s2 endpoints have same sign  relative to s1
                return SweepEnum.FALSE;       // => on same side => no intersect is possible
            lsign = TPoint2D.IsLeft(s2.lP, s2.rP, s1.lP);    //  s1 left point sign
            rsign = TPoint2D.IsLeft(s2.lP, s2.rP, s1.rP);    //  s1 right point sign
            if (lsign * rsign > 0) // s1 endpoints have same sign  relative to s2
                return SweepEnum.FALSE;       // => on same side => no intersect is possible
                                              // the segments s1 and s2 straddle each other
            return SweepEnum.TRUE;            // => an intersect exists
        }
    }

    /// <summary>
    /// SweepLine segment data class,
    /// top-down
    /// </summary>
    internal class SL_Seg
    {
        public int edge; // polygon edge i is V[i] to V[i+1]
        public TPoint2D lP; // leftmost vertex point
        public TPoint2D rP; // rightmost vertex point
        public SL_Seg above; // segment above this one
        public SL_Seg below; // segment below this one
    }

    /// <summary>
    /// The EventQueue is a presorted array (no intertions needed)
    /// </summary>
    internal struct Event
    {
        public int edge;
        public SweepEnum type;
        public TPoint2D eV;
    }

    internal class EventQueue
    {
        public int ne; // total number of events in array;
        public int ix; // index of next event on queue;
        //public Event[] Edata; // array of all events;
        public Event[] Eq; // sorted list of event pointers;

        public EventQueue(TPoint2DCollection P)
        {
            ix = 0;
            ne = 2 * P.Count;

            //Edata = new Event[ne];
            Eq = new Event[ne];

            // Initialize event queue with edge segmen endpoints
            for (int i = 0; i < P.Count; i++)
            {
                // test for degenerate edges, and remove them
                if (P[i] == P[i + 1]) // edge endpoints are equal (degen edge)
                {
                    ne -= 2; // 2 fewer degenerate events
                    continue; // do not process a degenerate edge
                }
                Eq[2 * i].edge = i;
                Eq[2 * i + 1].edge = i;
                Eq[2 * i].eV = (P[i]);
                Eq[2 * i + 1].eV = (P[i + 1]);
                if (TPoint2D.XYOrder(P[i], P[i + 1]) < 0)
                { // determine type
                    Eq[2 * i].type = SweepEnum.LEFT;
                    Eq[2 * i + 1].type = SweepEnum.RIGHT;
                }
                else
                {
                    Eq[2 * i].type = SweepEnum.RIGHT;
                    Eq[2 * i + 1].type = SweepEnum.LEFT;
                }
            }

            // Sort Eq[] by increasing x and y
            if (ne > 0) // there are events in the queue
                Array.Sort(Eq, (a, b) => TPoint2D.XYOrder(a.eV, b.eV));

        }
        public Event Next()
        {
            if (ix >= ne) // could have ix == 0 for a degenerate ne == 0
                return Eq.First();
            else return Eq[ix++];
        }
    }

    #region BBT

    internal class TNode<D>
    {
        public D val;
    }

    internal interface BBT<D>
    {
        TNode<D> Insert(D data);        // insert data into the tree
        TNode<D> Find(D data);          // find data from the tree
        TNode<D> Next(TNode<D> data);   // get next tree node
        TNode<D> Prev(TNode<D> data);   // get previous tree node
        void Remove(TNode<D> data);     // remove node from the tree
        void DelTree();                 // free all tree data structs

    }
    #endregion
}
