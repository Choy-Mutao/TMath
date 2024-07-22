using Clipper2Lib;
using tmath;
using tmath.geo_math;
using tmath.geo_math.curve;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Arc_Lab
    {
        [TestMethod]
        public void Test_Arc()
        {
            SvgWriter svg = new SvgWriter();

            TArc2D arc2d = new TArc2D(new TPoint2D(0, 0), 10.0, (260.0 / 180.0 * Math.PI), (50.0 / 180.0 * Math.PI), ARC_DIR.CCW);
            TPoint2DCollection dis = (TPoint2DCollection)arc2d.Discretize();
            dis.Insert(0, arc2d.Center);
            dis.Add(arc2d.Center);

            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis));

            TArc2D arc2d2 = new TArc2D(new TPoint2D(0, 0), 10.0, (260.0 / 180.0 * Math.PI), Math.PI * 2.0 - (50.0 / 180.0 * Math.PI), ARC_DIR.CW);
            TPoint2DCollection dis2 = (TPoint2DCollection)arc2d2.Discretize();
            dis2.Insert(0, arc2d2.Center);
            dis2.Add(arc2d2.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            string filename = @"..\..\..\TArc_Discretize.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ArcSPandEP()
        {
            // 1/4 arc
            var quarter_arc = new TArc2D(new TPoint2D(0, 0), 1.0, 0, (0.5 * Math.PI), ARC_DIR.CCW);
            Assert.IsTrue(quarter_arc.StartPoint.Equals(new TPoint2D(1, 0)));
            Assert.IsTrue(quarter_arc.EndPoint.IsEqualTo(new TPoint2D(0, 1), 1e-3));
        }

        [TestMethod]
        public void Test_ArcDistanceToPoint()
        {
            SvgWriter svg = new SvgWriter();

            // 1/4 arc
            var quarter_arc = new TArc2D(new TPoint2D(0, 0), 1.0, 0, (0.5 * Math.PI), ARC_DIR.CCW);
            TPoint2DCollection dis2 = (TPoint2DCollection)quarter_arc.Discretize(64);
            dis2.Insert(0, quarter_arc.Center);
            dis2.Add(quarter_arc.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            TPoint2D closestpnt;
            var dp1 = quarter_arc.DistanceToPoint(new TPoint2D(1.5, 0), out closestpnt);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(1.5, 0), new PointD(closestpnt.X, closestpnt.Y) } });
            var dp2 = quarter_arc.DistanceToPoint(new TPoint2D(0, 1.5), out closestpnt);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(0, 1.5), new PointD(closestpnt.X, closestpnt.Y) } });
            var dp3 = quarter_arc.DistanceToPoint(new TPoint2D(0.4, 0.6), out closestpnt);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(0.4, 0.6), new PointD(closestpnt.X, closestpnt.Y) } });
            var dp4 = quarter_arc.DistanceToPoint(new TPoint2D(0.2, 0.8), out closestpnt);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(0.2, 0.8), new PointD(closestpnt.X, closestpnt.Y) } });
            var dp5 = quarter_arc.DistanceToPoint(new TPoint2D(1, 1), out closestpnt);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(1, 1), new PointD(closestpnt.X, closestpnt.Y) } });


            string filename = @"..\..\..\Test_ArcDistanceToPoint.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ArcDistanceToSegment()
        {
            SvgWriter svg = new SvgWriter();
            // 1/4 arc
            var quarter_arc = new TArc2D(new TPoint2D(0, 0), 5.0, 0, (0.5 * Math.PI), ARC_DIR.CCW);
            TPoint2DCollection dis2 = (TPoint2DCollection)quarter_arc.Discretize(64);
            dis2.Insert(0, quarter_arc.Center);
            dis2.Add(quarter_arc.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            var sp = new TPoint2D(6, 6);
            var ep = new TPoint2D(8, 8);
            double ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out TPoint2D pOnArc, out TPoint2D pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            //sp = new TPoint2D(1, 1);
            //ep = new TPoint2D(2, 2);
            //ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            // ds1 = 0; pOnArc
            sp = new TPoint2D(1, 6);
            ep = new TPoint2D(6, 1);
            ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            //sp = new TPoint2D(4, 0);
            //ep = new TPoint2D(0, 4);
            //ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            //sp = new TPoint2D(0, 9);
            //ep = new TPoint2D(11, 0);
            //ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            //sp = new TPoint2D(0, 10);
            //ep = new TPoint2D(10, 0);
            //ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            //sp = new TPoint2D(0, 11);
            //ep = new TPoint2D(9, 0);
            //ds1 = quarter_arc.DistanceToSegment(new TLineSegment2d(sp, ep), out pOnArc, out pOnSegment);
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(sp.X, sp.Y), new PointD(ep.X, ep.Y) } });
            //SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pOnArc.X, pOnArc.Y), new PointD(pOnSegment.X, pOnSegment.Y) } });

            string filename = @"..\..\..\Test_ArcDistanceToSegment.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ArcIsPointOn()
        {
            SvgWriter svg = new SvgWriter();
            // 1/4 arc
            var quarter_arc = new TArc2D(new TPoint2D(0, 0), 5.0, 0, (0.5 * Math.PI), ARC_DIR.CCW);
            var circle = new TCircle2D(quarter_arc.Center, quarter_arc.Radius);
            var center = quarter_arc.Center;
            TPoint2DCollection dis2 = (TPoint2DCollection)quarter_arc.Discretize(64);
            dis2.Insert(0, quarter_arc.Center);
            dis2.Add(quarter_arc.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            TPoint2D p0;
            double theta = NumberUtils.DegreeToRadian(15);

            for (int i = 0; i < 24; i++)
            {
                // get point by circle
                p0 = circle.GetPointAtTheta(theta * i);
                bool on = quarter_arc.IsPointOn(p0);
                if (i < 7)
                    Assert.IsTrue(on);
                else
                    Assert.IsFalse(on);
                SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(center.X, center.Y), new PointD(p0.X, p0.Y) } });
            }

            quarter_arc = new TArc2D(new TPoint2D(12, 0), 5.0, 0, (0.5 * Math.PI), ARC_DIR.CW);
            circle = new TCircle2D(quarter_arc.Center, quarter_arc.Radius);
            center = quarter_arc.Center;
            dis2 = (TPoint2DCollection)quarter_arc.Discretize(64);
            dis2.Insert(0, quarter_arc.Center);
            dis2.Add(quarter_arc.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            for (int i = 1; i < 24; i++)
            {
                // get point by circle
                p0 = circle.GetPointAtTheta(theta * i);
                bool on = quarter_arc.IsPointOn(p0);
                if (i > 17)
                    Assert.IsTrue(on);
                else
                    Assert.IsFalse(on);
                SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(center.X, center.Y), new PointD(p0.X, p0.Y) } });
            }

            string filename = @"..\..\..\Test_ArcIsPointOn.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ArcIntersectWith()
        {
            SvgWriter svg = new SvgWriter();
            // 1/4 arc
            var quarter_arc = new TArc2D(new TPoint2D(0, 0), 5.0, 0, (0.5 * Math.PI), ARC_DIR.CCW);
            TPoint2DCollection dis2 = (TPoint2DCollection)quarter_arc.Discretize(64);
            dis2.Insert(0, quarter_arc.Center);
            dis2.Add(quarter_arc.Center);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            // line [5,0] to [0,5]
            TPoint2D p0, p1;
            p0 = new TPoint2D(5, 0); p1 = new TPoint2D(0, 5);
            var tline = new TLine2D(p0, p1);
            TPoint2D ip1, ip2;
            var i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.TWO);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });

            p0 = new TPoint2D(6, 0); p1 = new TPoint2D(0, 6);
            tline = new TLine2D(p0, p1);
            i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.TWO);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });

            p0 = new TPoint2D(0, 0); p1 = new TPoint2D(6, 6);
            tline = new TLine2D(p0, p1);
            i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.ONE);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });

            p0 = new TPoint2D(3, 0); p1 = new TPoint2D(3, 6);
            tline = new TLine2D(p0, p1);
            i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.ONE);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });

            p0 = new TPoint2D(0, 3); p1 = new TPoint2D(6, 3);
            tline = new TLine2D(p0, p1);
            i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.ONE);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });

            p0 = new TPoint2D(0, -3); p1 = new TPoint2D(-3, 0);
            tline = new TLine2D(p0, p1);
            i = quarter_arc.IntersectWith(tline, out ip1, out ip2);
            Assert.IsTrue(i == INTER_NUM.ZERO);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(p0.X, p0.Y), new PointD(p1.X, p1.Y) } });


            string filename = @"..\..\..\Test_ArcDistanceToSegment.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_Ellipse2D()
        {
            SvgWriter svg = new SvgWriter();

            TEllipse2D ellipse2D = new TEllipse2D(new TPoint2D(9, 4), 35, 6);
            var e_dis = (TPoint2DCollection)ellipse2D.Discretize(64);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(e_dis));

            string filename = @"..\..\..\ClipperUtil_Ellipse2D.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }
    }
}
