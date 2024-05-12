using Clipper2Lib;
using tmath;

namespace tmath_lab
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("测试绘制结果")]
        public void TestMethod1()
        {
            Path64 p64_1 = new Path64();
            Path64 p64_2 = new Path64();
            Path64 p64_3 = new Path64();

            p64_1.Add(new Point64(100, 10));
            p64_1.Add(new Point64(0, 0));
            p64_1.Add(new Point64(100, 90));

            var area1 = Clipper.Area(p64_1);

            p64_2.Add(new Point64(0, 0));
            p64_2.Add(new Point64(200, 0));
            p64_2.Add(new Point64(200, 100));
            p64_2.Add(new Point64(0, 100));

            Clipper64 clipper = new Clipper64();
            clipper.AddSubject(p64_1);
            clipper.AddSubject(p64_2);

            Paths64 solution = new Paths64();
            clipper.Execute(ClipType.Difference, FillRule.NonZero, solution);


            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSolution(svg, solution, true);
            string filename = @"..\..\..\ClipperUtil_Test.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod("测试有洞的绘制结果")]
        public void TestMethod2()
        {
            Path64 p64_1 = new Path64();
            Path64 p64_2 = new Path64();
            Path64 p64_3 = new Path64();

            p64_1.Add(new Point64(100, 0));
            p64_1.Add(new Point64(100, 100));
            p64_1.Add(new Point64(0, 0)); p64_1.Reverse();

            p64_2.Add(new Point64(100, 0));
            p64_2.Add(new Point64(200, 0));
            p64_2.Add(new Point64(200, 100));
            p64_2.Add(new Point64(100, 100)); p64_2.Reverse();

            p64_3.Add(new Point64(-10, -10));
            p64_3.Add(new Point64(300, -10));
            p64_3.Add(new Point64(300, 200));
            p64_3.Add(new Point64(-10, 200));


            Paths64 solution = new Paths64
            {
                p64_3,
                p64_1,
                p64_2
            };

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSolution(svg, solution, true);
            string filename = @"..\..\..\ClipperUtil_WithHole_Test.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod("绘制矩形的移动和旋转")]
        public void Test_MultiplyDraw()
        {
            TPoint2DCollection rectangle = new()
            {
                new TPoint2D(0,0), new TPoint2D(10, 0), new TPoint2D(10, 5), new TPoint2D(0,5)
            };

            SvgWriter svg_writer = new SvgWriter();
            SvgUtils.AddSubject(svg_writer, ClipperUtil.TPoint2DCollectionToClipperPathD(rectangle));
            // start move
            TMatrix3 rotate_matrix = TMatrix3.MakeRotation(NumberUtils.DegreeToRadian(20));
            TMatrix3 move_matrix = TMatrix3.MakeTranslation(new TVector2D(10, 10));

            TMatrix3 matrix = TMatrix3.I;
            matrix = matrix.PreMultiply(move_matrix);
            matrix = matrix.PreMultiply(rotate_matrix);

            var solution = ClipperUtil.TPoint2DCollectionToClipperPathD(((TPoint2DCollection)rectangle.Clone()).ApplyMtrix3(matrix));
            SvgUtils.AddSolution(svg_writer, new PathsD() { solution }, true);

            string filename = @"..\..\testdrawing.svg";
            SvgUtils.SaveToFile(svg_writer, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }

        [TestMethod("Matrix3.Rotate")]
        public void Test_M3_rotate()
        {
            TPoint2DCollection rectangle = new()
            {
                new TPoint2D(0,0), new TPoint2D(10, 0), new TPoint2D(10, 5), new TPoint2D(0,5)
            };
            SvgWriter svg_writer = new SvgWriter();
            SvgUtils.AddSubject(svg_writer, ClipperUtil.TPoint2DCollectionToClipperPathD(rectangle));
            rectangle.ApplyMtrix3(TMatrix3.I.Rotate(NumberUtils.DegreeToRadian(20)));
            SvgUtils.AddSolution(svg_writer, ClipperUtil.TPointsToClipperPathsD(new List<TPoint2DCollection>() { rectangle }), true);
            string filename = @"..\..\test.svg";
            SvgUtils.SaveToFile(svg_writer, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod("Clipper.InflatePaths")]
        public void Test_Clipper_Inflate()
        {
            PathsD rectangle = new() { new PathD() { new PointD(0, 0), new PointD(100, 10), new PointD(100, 90), new PointD(0, 200) } };

            SvgWriter svgwriter = new SvgWriter();
            SvgUtils.AddSubject(svgwriter, rectangle);
            SvgUtils.AddSolution(svgwriter, Clipper.InflatePaths(rectangle, -10, JoinType.Miter, EndType.Polygon), true);
            string filename = @"..\..\inflate.svg";
            SvgUtils.SaveToFile(svgwriter, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);


        }
    }
}