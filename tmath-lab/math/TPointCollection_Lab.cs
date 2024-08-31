using Clipper2Lib;
using tmath;
using tmath.geo_math;
using tmath.geo_math.curve;
using tmath.pga;

namespace tmath_lab.math
{
    [TestClass]
    public class TPointCollection_Lab
    {
        static TPoint2DCollection test2dpoints = new TPoint2DCollection()
        {
            new TPoint2D(1.0, 1.0),new TPoint2D(2.0, 1.0),new TPoint2D(2.0, 2.0),new TPoint2D(1.0, 2.0)
        };
        #region Test Methods
        [TestMethod]
        public void Test_CollectionIsClosed()
        {
            Assert.IsFalse(test2dpoints.IsClosed());
            test2dpoints.MakeClosed();
            Assert.IsTrue(test2dpoints.IsClosed());

            Assert.IsFalse(new TPoint2DCollection().IsClosed());
        }

        [TestMethod("判断点集是否Clockwise")]
        public void Test_CollectionIsClockwise()
        {
            // 矩形逆时针
            // (0,3) ------- (4, 3)
            //   |              |
            // (0,0) ------- (4, 0)
            var ccw_rect = new TPoint2DCollection() { new TPoint2D(0, 0), new TPoint2D(4, 0), new TPoint2D(4, 3), new TPoint2D(0, 3) };
            Assert.IsFalse(ccw_rect.IsClockwise(), "Passed");
            // 矩形顺时针
            var cw_rect = new TPoint2DCollection() { new TPoint2D(0, 0), new TPoint2D(0, 3), new TPoint2D(4, 3), new TPoint2D(4, 0) };
            Assert.IsTrue(cw_rect.IsClockwise(), "Passed");
            // 三角形顺时针
            var cw_tran = new TPoint2DCollection() { new TPoint2D(1, 1), new TPoint2D(2, 5), new TPoint2D(5, 2) };
            Assert.IsTrue(cw_tran.IsClockwise(), "Passed");
            // 三角形逆时针
            var ccw_tran = new TPoint2DCollection() { new TPoint2D(1, 1), new TPoint2D(5, 2), new TPoint2D(2, 5) };
            Assert.IsFalse(ccw_tran.IsClockwise(), "Passed");
            // 凹多边形
            var non_simple_polygon = new TPoint2DCollection() { new TPoint2D(0, 0), new TPoint2D(4, 0), new TPoint2D(4, 3), new TPoint2D(2, 2), new TPoint2D(0, 3) };
            Assert.IsFalse(non_simple_polygon.IsClockwise(), "Passed");
            var cw_polygon = new TPoint2DCollection() { new TPoint2D(2, 2), new TPoint2D(3, 5), new TPoint2D(6, 5), new TPoint2D(7, 2), new TPoint2D(4, 1) };
            Assert.IsTrue(cw_polygon.IsClockwise(), "Passed");
            var ccw_polygon = new TPoint2DCollection() { new TPoint2D(2, 2), new TPoint2D(4, 1), new TPoint2D(7, 2), new TPoint2D(6, 5), new TPoint2D(3, 5) };
            Assert.IsFalse(ccw_polygon.IsClockwise(), "Passed");
        }

        [TestMethod("Test_CollectionRandom")]
        public void Test_CollectionRandom()
        {
            TPoint2DCollection random_points = TPoint2DCollection.Random(10, new TBox2D(0, 0, 100, 100));

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(random_points));
            string filename = @"..\..\..\Test_CollectionRandom.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }

        [TestMethod("Test_CollectionConvexHull")]
        public void Test_CollectionConvexHull()
        {
            TPoint2DCollection random_points = TPoint2DCollection.Random(100, new TBox2D(0, 0, 1000, 100));

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(random_points));

            //var num1 = ConvexHullPointSet.ChainHull_2D(random_points, out TPoint2DCollection H);
            //var num2 = ConvexHullSimplePolyline.SimpleHull_2D(random_points, out TPoint2DCollection H);
            var num3 = ConvexHullApproximation.NearHull_2D(random_points, 1000000, out TPoint2DCollection H);

            SvgUtils.AddSolution(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(H) }, true);


            string filename = @"..\..\..\Test_CollectionRandom.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod("Test_extreampoint")]
        public void Test_CollcetionExtremePoint()
        {
            TPoint2DCollection random_points = TPoint2DCollection.Random(100, new TBox2D(0, 0, 1920, 1080));
            random_points.XYOrder();

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddOpenSolution(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(random_points) }, false);
            var H = random_points.ConvexHull();
            Assert.IsFalse(H.IsClockwise());

            SvgUtils.AddSolution(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(H) }, true);

            H.GetBox(out TPoint2D LB, out TPoint2D RT);


            TPoint2DCollection topline = new TPoint2DCollection() { new TPoint2D(LB.X, RT.Y + 100), new TPoint2D(RT.X, RT.Y + 100) };
            //TPoint2DCollection topline = new TPoint2DCollection() { new TPoint2D(LB.X, (LB.Y + RT.Y) * 0.5), new TPoint2D(RT.X, (LB.Y + RT.Y) * 0.5) };
            SvgUtils.AddOpenSubject(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(topline) });
            ConvexPolygonExtremePoints.Dist2D_Poly_To_Line(H, new TLine2D(topline[0], topline[1]), out TPoint2D extremePoint, out TPoint2D online);
            SvgUtils.AddOpenSolution(svg, new PathsD() { new PathD() { new PointD(extremePoint.X, extremePoint.Y), new PointD(online.X, online.Y) } }, true);

            TPoint2DCollection rightline = new TPoint2DCollection() { new TPoint2D(RT.X + 150, RT.Y), new TPoint2D(RT.X + 100, LB.Y) };
            SvgUtils.AddOpenSubject(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(rightline) });
            ConvexPolygonExtremePoints.Dist2D_Poly_To_Line(H, new TLine2D(rightline[0], rightline[1]), out extremePoint, out online);
            SvgUtils.AddOpenSolution(svg, new PathsD() { new PathD() { new PointD(extremePoint.X, extremePoint.Y), new PointD(online.X, online.Y) } }, true);

            TPoint2DCollection bottomline = new TPoint2DCollection() { new TPoint2D(RT.X, LB.Y - 100), new TPoint2D(LB.X, LB.Y - 150) };
            SvgUtils.AddOpenSubject(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(bottomline) });
            ConvexPolygonExtremePoints.Dist2D_Poly_To_Line(H, new TLine2D(bottomline[0], bottomline[1]), out extremePoint, out online);
            SvgUtils.AddOpenSolution(svg, new PathsD() { new PathD() { new PointD(extremePoint.X, extremePoint.Y), new PointD(online.X, online.Y) } }, true);

            TPoint2DCollection leftline = new TPoint2DCollection() { new TPoint2D(LB.X - 100, LB.Y), new TPoint2D(LB.X - 150, RT.Y) };
            SvgUtils.AddOpenSubject(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(leftline) });
            ConvexPolygonExtremePoints.Dist2D_Poly_To_Line(H, new TLine2D(leftline[0], leftline[1]), out extremePoint, out online);
            SvgUtils.AddOpenSolution(svg, new PathsD() { new PathD() { new PointD(extremePoint.X, extremePoint.Y), new PointD(online.X, online.Y) } }, true);


            string filename = @"..\..\..\Test_extreampoint.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 1920, 1080, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }

        [TestMethod("Test_ReductVertices")]
        public void Test_ReductVertices()
        {
            TPoint2DCollection redundant_collection = new TPoint2DCollection()
            {
                new TPoint2D(0,0), // 0
                new TPoint2D(100,0), // 1
                new TPoint2D(100.0001,0), // 2 reduce
                new TPoint2D(102,0), // 3
                new TPoint2D(102, 100), // 4
                new TPoint2D(102.000412, 99.99978), // 5 reduce
                new TPoint2D(101, 100), // 6
                new TPoint2D(0.10001,100.00003), // 7
                new TPoint2D(0.00001,100.00009), // 8
                new TPoint2D(0,100), // 9 reduce
            };
            redundant_collection.ReduceVertices();
            Assert.IsTrue(redundant_collection.Count == 7);
        }

        [TestMethod("Test_TPoint2ds decimate")]
        public void Test_CollctionDecimate()
        {
            TPoint2DCollection p1 = new TPoint2DCollection()
            {
                new TPoint2D(0,0),
                new TPoint2D(1,2),
                new TPoint2D(2,4),
                new TPoint2D(3,6),
                new TPoint2D(4,8),
                new TPoint2D(5,10),
            };
            p1.Decimate(0.01);
            Assert.IsTrue(p1.Count == 2);
            Assert.IsTrue(p1[0] == new TPoint2D(0, 0));
            Assert.IsTrue(p1[1] == new TPoint2D(5, 10));

            p1 = new TPoint2DCollection()
            {
                new TPoint2D(1,1),
                new TPoint2D(2,3),
                new TPoint2D(3,5),
                new TPoint2D(4,7),
                new TPoint2D(5,9),
                new TPoint2D(6,11),
            };
            p1.Decimate(0.01);
            Assert.IsTrue(p1.Count == 2);
            Assert.IsTrue(p1[0] == new TPoint2D(1, 1));
            Assert.IsTrue(p1[1] == new TPoint2D(6, 11));

            p1 = new TPoint2DCollection()
            {
                new TPoint2D(0,0),
                new TPoint2D(1,1),
                new TPoint2D(2,0),
                new TPoint2D(3,1),
                new TPoint2D(4,0),
                new TPoint2D(5,1),
                new TPoint2D(6,0),
            };
            p1.Decimate(1);
            Assert.IsTrue(p1.Count == 2);
            Assert.IsTrue(p1[0] == new TPoint2D(0, 0));
            Assert.IsTrue(p1[1] == new TPoint2D(6, 0));

            p1 = new TPoint2DCollection()
            {
                new TPoint2D(181.9, 157.1),
                new TPoint2D(286.8, 157.1),
                new TPoint2D(236.8, 157.1),
                new TPoint2D(181.9, 157.1),
            };
            p1.Decimate(0.1);
            Assert.IsTrue(p1.Count == 3);
            Assert.IsTrue(p1[0] == new TPoint2D(181.9, 157.1));
            Assert.IsTrue(p1[1] == new TPoint2D(286.8, 157.1));
            Assert.IsTrue(p1[2] == new TPoint2D(181.9, 157.1));

            p1 = new TPoint2DCollection()
            {
                new TPoint2D(187.7, 168.4),
                new TPoint2D(236.2, 168.4),
                new TPoint2D(287.7, 168.4),
                new TPoint2D(323.9, 210.2),
                new TPoint2D(187.7, 168.4),
            };
            p1.Decimate(0.1);
            Assert.IsTrue(p1.Count == 4);
            Assert.IsTrue(p1[0] == new TPoint2D(187.7, 168.4));
            Assert.IsTrue(p1[1] == new TPoint2D(287.7, 168.4));
            Assert.IsTrue(p1[2] == new TPoint2D(323.9, 210.2));
            Assert.IsTrue(p1[3] == new TPoint2D(187.7, 168.4));
        }

        [TestMethod("")]
        public void Test_CollectionArea()
        {
            TPoint2DCollection random_points = TPoint2DCollection.Random(100, new TBox2D(0, 0, 100, 100));
            double area = random_points.Area();
            Assert.IsTrue(true);
        }

        [TestMethod("对TPoint2dCollection进行 xy 增序列排列")]
        public void Test_TPoint2DCollection_XYOrder()
        {
            double[][] data = new double[][]
            {   new double[] {6.39, 0.25},new double[] {2.75, 2.23},new double[] {7.36, 6.77},new double[] {8.92, 0.87},new double[] {4.22, 0.30},new double[] {2.19, 5.05},new double[] {0.27, 1.99},new double[] {6.50, 5.45},new double[] {2.20, 5.89},new double[] {8.09, 0.06},new double[] {8.06, 6.98},new double[] {3.40, 1.55},new double[] {9.57, 3.37},new double[] {0.93, 0.97},new double[] {8.47, 6.04},new double[] {8.07, 7.30},new double[] {5.36, 9.73},new double[] {3.79, 5.52},new double[] {8.29, 6.19},new double[] {8.62, 5.77}
            };
            TPoint2DCollection collection2ds = new TPoint2DCollection(data);
            collection2ds.XYOrder();
            Assert.IsTrue(collection2ds[0].IsEqualTo(new TPoint2D(0.27, 1.99)));
            Assert.IsTrue(collection2ds[1].IsEqualTo(new TPoint2D(0.93, 0.97)));
            Assert.IsTrue(collection2ds[2].IsEqualTo(new TPoint2D(2.19, 5.05)));
            Assert.IsTrue(collection2ds[3].IsEqualTo(new TPoint2D(2.20, 5.89)));
            Assert.IsTrue(collection2ds[4].IsEqualTo(new TPoint2D(2.75, 2.23)));
            Assert.IsTrue(collection2ds[5].IsEqualTo(new TPoint2D(3.40, 1.55)));
            Assert.IsTrue(collection2ds[6].IsEqualTo(new TPoint2D(3.79, 5.52)));
            Assert.IsTrue(collection2ds[7].IsEqualTo(new TPoint2D(4.22, 0.30)));
            Assert.IsTrue(collection2ds[8].IsEqualTo(new TPoint2D(5.36, 9.73)));
            Assert.IsTrue(collection2ds[9].IsEqualTo(new TPoint2D(6.39, 0.25)));
            Assert.IsTrue(collection2ds[10].IsEqualTo(new TPoint2D(6.50, 5.45)));
            Assert.IsTrue(collection2ds[11].IsEqualTo(new TPoint2D(7.36, 6.77)));
            Assert.IsTrue(collection2ds[12].IsEqualTo(new TPoint2D(8.06, 6.98)));
            Assert.IsTrue(collection2ds[13].IsEqualTo(new TPoint2D(8.07, 7.30)));
            Assert.IsTrue(collection2ds[14].IsEqualTo(new TPoint2D(8.09, 0.06)));
            Assert.IsTrue(collection2ds[15].IsEqualTo(new TPoint2D(8.29, 6.19)));
            Assert.IsTrue(collection2ds[16].IsEqualTo(new TPoint2D(8.47, 6.04)));
            Assert.IsTrue(collection2ds[17].IsEqualTo(new TPoint2D(8.62, 5.77)));
            Assert.IsTrue(collection2ds[18].IsEqualTo(new TPoint2D(8.92, 0.87)));
            Assert.IsTrue(collection2ds[19].IsEqualTo(new TPoint2D(9.57, 3.37)));

        }

        [TestMethod("计算两个任意多边形的最近点")]
        public void Test_TPoint2dCollectionClosestPoint()
        {
            TPoint2DCollection collection1 = new TPoint2DCollection()
            {
                new TPoint2D(2,4),
                new TPoint2D(4,4),
                new TPoint2D(4,5),
                new TPoint2D(5,5),
                new TPoint2D(5,4),
                new TPoint2D(7,4),
                new TPoint2D(8,2),
                new TPoint2D(6,2),
                new TPoint2D(5,3),
                new TPoint2D(2,3),
            };
            TPoint2DCollection collection2 = new TPoint2DCollection()
            {
                new TPoint2D(2,1),
                new TPoint2D(4,1),
                new TPoint2D(4,2),
                new TPoint2D(5,2),
                new TPoint2D(5,1),
                new TPoint2D(7,1),
                new TPoint2D(8,-1),
                new TPoint2D(6,-1),
                new TPoint2D(5,0),
                new TPoint2D(2,0),
            };

            TPoint2DCollection collection3 = new TPoint2DCollection()
            {
                new TPoint2D(9,1),
                new TPoint2D(11,1),
                new TPoint2D(11,2),
                new TPoint2D(12,2),
                new TPoint2D(12,1),
                new TPoint2D(14,1),
                new TPoint2D(15,-1),
                new TPoint2D(13,-1),
                new TPoint2D(12,0),
                new TPoint2D(9,0),
            };

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(collection1));
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(collection2));
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(collection3));

            var dist1 = TPoint2DCollection.ClosestPointTo(collection1, collection2, out TPoint2D pon1, out TPoint2D pon2);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pon1.X, pon1.Y), new PointD(pon2.X, pon2.Y) } });
            var dist2 = TPoint2DCollection.ClosestPointTo(collection1, collection3, out pon1, out pon2);
            SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(pon1.X, pon1.Y), new PointD(pon2.X, pon2.Y) } });


            string filename = @"..\..\..\TPoint2dCollectionClosestPoint.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 1920, 1080, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }
        #endregion


    }
}
