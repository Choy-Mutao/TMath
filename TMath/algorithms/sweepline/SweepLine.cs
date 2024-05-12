using tmath.gs.trees;

namespace tmath.algorithms.sweepline
{

    // the Sweep Line itself
    public class SweepLine
    {
        public int nv; // number of vertices in polygon
        TPoint2DCollection Pn; // initial polygon
        AVLTree<SL_Seg> Tree; // BBT

#if DEBUG
        public int GetNV() => nv;
        public TPoint2DCollection GetPn() => Pn;
        public AVLTree<SL_Seg> GetTree() => Tree;
#endif

        public SweepLine(TPoint2DCollection P)
        {
            nv = P.Count;
            Pn = (TPoint2DCollection)P.Clone();
            Tree = new AVLTree<SL_Seg>();
        }

        ~SweepLine()
        {

        }

        public void CleanTree(AVLNode<SL_Seg> p)
        {
            if (p == null) return;
            CleanTree(p.Subtree(AVLNode<SL_Seg>.dir_t.LEFT));
            CleanTree(p.Subtree(AVLNode<SL_Seg>.dir_t.RIGHT));
        }

        public SL_Seg Add(ref SweepEvent E)
        {
            // fill in SL_Seg element data
            SL_Seg s = new SL_Seg();
            s.edge = E.edge;
            E.seg = s;

            // if it is being added, then it must be a LEFT edge event but need to determine which endpoint is the left one
            TPoint2D v1 = Pn[s.edge];
            TPoint2D eN = s.edge + 1 < Pn.Count ? Pn[s.edge + 1] : Pn[0];
            TPoint2D v2 = eN;

            if (TPoint2D.XYOrder(v1, v2) < 0) // determine which is leftmost
            {
                s.lP = v1;
                s.rP = v2;
            } else
            {
                s.lP = v2;
                s.rP = v1;
            }
            s.above = null;
            s.below = null;

            // add a node to the balanced binary tre
            AVLNode<SL_Seg> nd = Tree.Insert(s);
            AVLNode<SL_Seg> nx = Tree.Next(nd);
            AVLNode<SL_Seg> np = Tree.Prev(nd);

            if (nx != null)
            {
                s.above = (SL_Seg)nx.Data();
                s.above.below = s;
            }
            if (np != null)
            {
                s.below = (SL_Seg)(np.Data());
                s.below.above = s;
            }
            return s;
        }
        public SL_Seg Search(SweepEvent E)
        {
            // need a segment to find it in the tree
            SL_Seg s = new SL_Seg();
            s.edge = E.edge;
            s.above = null;
            s.below = null;

            AVLNode<SL_Seg> nd = Tree.Search(s);
            if (nd == null) return null;
            return (SL_Seg)nd.Data();
        }
        public bool Intersect(SL_Seg s1, SL_Seg s2)
        {
            if (s1 == null || s2 == null)
                return false;      // no intersect if either segment doesn't exist

            // check for consecutive edges in polygon
            int e1 = s1.edge;
            int e2 = s2.edge;
            if ((((e1 + 1) % nv) == e2) || (e1 == ((e2 + 1) % nv)))
                return false;      // no non-simple intersect since consecutive

            // test for existence of an intersect point
            double lsign, rsign;
            lsign = TPoint2D.IsLeft(s1.lP, s1.rP, s2.lP);    // s2 left point sign
            rsign = TPoint2D.IsLeft(s1.lP, s1.rP, s2.rP);    // s2 right point sign
            if (lsign * rsign > 0) // s2 endpoints have same sign relative to s1
                return false;      // => on same side => no intersect is possible

            lsign = TPoint2D.IsLeft(s2.lP, s2.rP, s1.lP);    // s1 left point sign
            rsign = TPoint2D.IsLeft(s2.lP, s2.rP, s1.rP);    // s1 right point sign
            if (lsign * rsign > 0) // s1 endpoints have same sign relative to s2
                return false;      // => on same side => no intersect is possible

            // the segments s1 and s2 straddle each other
            return true;           // => an intersect exists
        }
        public void Remove(SL_Seg s)
        {
            // remove the node from the balanced binary tree
            AVLNode<SL_Seg> nd = Tree.Search(s);
            if (nd == null)
            {
                //const char* m = "simple_Polygon internal error:  attempt to remove segment not in tree";
                //fprintf(stderr, "%s\n", m);
                //fprintf(stderr, "segment: (%g, %g) --> (%g, %g)\n",
                //        s->lP->x, s->lP->y, s->rP->x, s->rP->y);
                //Tree.DumpTree();
                //throw runtime_error(m);
                throw new System.Exception("simple_Polygon internal error:  attempt to remove segment not in tree");
            }

            // get the above and below segments pointing to each other
            AVLNode<SL_Seg> nx = Tree.Next(nd);
            if (nx != null)
            {
                SL_Seg sx = (SL_Seg)(nx.Data());
                sx.below = s.below;
            }
            AVLNode<SL_Seg> np = Tree.Prev(nd);
            if (np != null)
            {
                SL_Seg sp = (SL_Seg)(np.Data());
                sp.above = s.above;
            }
            Tree.Delete(nd.Key());       // now can safely remove it
        }

    }
}
