using Clipper2Lib;
using tmath;
using tmath.algorithms.sweepline;
using tmath.geometry;

namespace tmath_lab.algorithm_lab
{
    [TestClass]
    public class sweepline_lab
    {
        public readonly TPoint2DCollection test_data = TPoint2DCollection.Random(49, new tmath.geometry.TBox2D(new TPoint2D(-100, -100), new TPoint2D(100, 100)));

        [TestMethod]
        public void Test_SL_Seg()
        {
            SL_Seg s1 = new SL_Seg(new TPoint2D(1, 1), new TPoint2D(5, 5)); s1.edge = 0;
            SL_Seg s2 = new SL_Seg(new TPoint2D(5, 5), new TPoint2D(1, 1)); s2.edge = 0;
            Assert.IsTrue(s1.lP == s2.lP); Assert.IsTrue(s1.rP == s2.rP);

            SL_Seg s3 = new SL_Seg(new TPoint2D(1, 1), new TPoint2D(2, -2)); s3.edge = 1;
            Assert.IsTrue(s3 < s1);
            Assert.IsTrue(s1 > s3);

            s3 = new SL_Seg(new TPoint2D(-1, 5), new TPoint2D(2, -2)); s3.edge = 1;
            Assert.IsTrue(s3 < s1);
            Assert.IsTrue(s1 > s3);

            s3 = new SL_Seg(new TPoint2D(3, -10), new TPoint2D(3, 10)); s3.edge = 1;
            Assert.IsTrue(s3 < s1);

            s3 = new SL_Seg(new TPoint2D(3, 3), new TPoint2D(3, 10)); s3.edge = 1;
            Assert.IsTrue(s3 > s1);
        }

        [TestMethod]
        public void Test_SweepLine_Constructor()
        {
            SweepLine sweepline = new SweepLine(test_data);
            Assert.IsTrue(sweepline.GetNV() == test_data.Count);
            Assert.IsTrue(sweepline.GetPn().GetHashCode() != test_data.GetHashCode());
        }

        [TestMethod]
        public void Test_SweepEvent_Constructor()
        {
            SweepEvent sweepevent = new SweepEvent();
            sweepevent.edge = 0;
            sweepevent.eV = new TPoint2D(0, 0);
        }

        [TestMethod]
        public void Test_EveneQueue()
        {
            EventQueue eventqueue = new EventQueue(test_data);
            Assert.IsTrue(eventqueue.ix == 0);
            Assert.IsTrue(eventqueue.ne == test_data.Count * 2);

            while (eventqueue.ix < eventqueue.ne)
            {
                var next = eventqueue.Next();
                if (next == null) Assert.IsTrue(eventqueue.ix == eventqueue.ne);
                if (next == null) break;
            }
        }

        [TestMethod]
        public void Test_SimplePolgon()
        {
            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(test_data));

            EventQueue Eq = new EventQueue(test_data);
            SweepLine SL = new SweepLine(test_data);

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
                    if (SL.Intersect(s, s?.above))
                    {
                        // draw sweel_line
                        TLineSegment2d s1 = new TLineSegment2d(s.lP, s.rP);
                        TLineSegment2d s2 = new TLineSegment2d(s.above.lP, s.above.rP);
                        var inter = s1.IntersectWith(s2, out TPoint2D p1, out TPoint2D p2);
                        if (inter == (INTER_NUM)1)
                        {
                            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p1.X, -600), new PointD(p1.X, 600) } });
                        }
                    }
                    if (SL.Intersect(s, s?.below))
                    {
                        // draw sweel_line
                        TLineSegment2d s1 = new TLineSegment2d(s.lP, s.rP);
                        TLineSegment2d s2 = new TLineSegment2d(s.below.lP, s.below.rP);
                        var inter = s1.IntersectWith(s2, out TPoint2D p1, out TPoint2D p2);
                        if (inter == (INTER_NUM)1)
                        {
                            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p1.X, -600), new PointD(p1.X, 600) } });
                        }
                    }
                }
                else
                {                     // process a right vertex
                    s = e.otherEnd.seg;

                    if (SL.Intersect(s?.above, s?.below))
                    {
                        // draw sweel_line
                        TLineSegment2d s1 = new TLineSegment2d(s.above.lP, s.above.rP);
                        TLineSegment2d s2 = new TLineSegment2d(s.below.lP, s.below.rP);
                        var inter = s1.IntersectWith(s2, out TPoint2D p1, out TPoint2D p2);
                        if (inter == (INTER_NUM)1)
                        {
                            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p1.X, -600), new PointD(p1.X, 600) } });
                        }
                    }
                    SL.Remove(s);     // remove it from the sweep line
                }
            }

            string filename = @"..\..\..\Test_CollectionRandom.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }
    }
}
