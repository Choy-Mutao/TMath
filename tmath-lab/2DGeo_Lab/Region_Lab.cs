using Clipper2Lib;
using tmath;
using tmath.geo_math.face;
using tmath.geometry;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Region_Lab
    {
        [TestMethod("AddPath")]
        public void Test_RegionAddPath()
        {
            TRegion2d region2d = new TRegion2d();
            region2d.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(0,0),
                new TPoint2D(300, 0),
                new TPoint2D(300, 300),
                new TPoint2D(0, 300),
            });
            region2d.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(30, 30),
                new TPoint2D(30, 230),
                new TPoint2D(230, 230),
                new TPoint2D(230, 30),
            });
            region2d.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(-10,-10),
                new TPoint2D(-300, 0),
                new TPoint2D(-300, -300),
                new TPoint2D(0, -300),
            });
            DrawRegion(region2d, "AddPath");
        }

        [TestMethod("ShearRegion")]
        public void Test_RegionBoolOper()
        {

            TRegion2d region2D_subject = new TRegion2d();
            region2D_subject.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(0,0),
                new TPoint2D(300, 0),
                new TPoint2D(300, 300),
                new TPoint2D(0, 300),
            });
            region2D_subject.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(-10,-10),
                new TPoint2D(-300, 0),
                new TPoint2D(-300, -300),
                new TPoint2D(0, -300),
            });


            TRegion2d region2D_clip = new TRegion2d();
            region2D_clip.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(10, 10),
                new TPoint2D(30, 10),
                new TPoint2D(30, 30),
                new TPoint2D(10, 30),
            });

            region2D_subject.BooleanOper(region2D_clip, ClipType.Difference);
            DrawRegion(region2D_subject, "Differeent");

        }

        [TestMethod("UnionRegion")]
        public void Test_RegionBoolOper_Union()
        {

            TRegion2d region2D_subject = new TRegion2d();
            region2D_subject.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(0,0),
                new TPoint2D(300, 0),
                new TPoint2D(300, 300),
                new TPoint2D(0, 300),
            });


            TRegion2d region2D_clip = new TRegion2d();
            region2D_clip.AddPath(new TPoint2DCollection()
            {
                new TPoint2D(-10,-10),
                new TPoint2D(-300, 0),
                new TPoint2D(-300, -300),
                new TPoint2D(0, -300),
            });

            region2D_subject.BooleanOper(region2D_clip, ClipType.Union);
            DrawRegion(region2D_subject, "Union");

        }


        private void DrawRegion(TRegion2d region2d, string name = "")
        {
            PathsD solution = new PathsD();
            region2d.paths.ForEach(path => solution.Add(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour)));

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSolution(svg, solution, true);
            string filename = @"..\..\..\Labs\Geo_Lab\" + name + ".svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }
    }
}
