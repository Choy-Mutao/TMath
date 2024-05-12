using Clipper2Lib;
using tmath;
using tmath.earcut;

namespace tmath_lab.math
{
    [TestClass]
    public class EarcutTest
    {
        #region Test Methods
        [TestMethod]
        public void Drawing_Earcut()
        {
            TPoint2DCollection point2Ds = new()
            {
                new TPoint2D(661, 112),
                new TPoint2D(661, 96),
                new TPoint2D(666, 96),
                new TPoint2D(666, 87),
                new TPoint2D(743, 87),
                new TPoint2D(771, 87),
                new TPoint2D(771, 114),
                new TPoint2D(750, 114),
                new TPoint2D(750, 113),
                new TPoint2D(742, 113),
                new TPoint2D(742, 106),
                new TPoint2D(710, 106),
                new TPoint2D(710, 113),
                new TPoint2D(666, 113),
                new TPoint2D(666, 112)
            };

            List<double> pointsarray = new List<double>();
            List<int> indices = new List<int>();
            point2Ds.ForEach(p => { pointsarray.Add(p.X); pointsarray.Add(p.Y); });
            var triangles = Earcut.Tessellate(pointsarray, indices);
            PathsD solution = new PathsD();
            for(int i = 0;  i < triangles.Count - 2; i+=3)
            {
                int ti1 = triangles[i] * 2, ti2 = triangles[i+1] * 2, ti3 = triangles[i+2] * 2;
                double x1 = pointsarray[ti1], y1 = pointsarray[ti1+1];
                double x2 = pointsarray[ti2], y2 = pointsarray[ti2+1];
                double x3 = pointsarray[ti3], y3 = pointsarray[ti3+1];
                solution.Add(new PathD() { new PointD(x1, y1), new PointD(x2, y2), new PointD(x3, y3) });
            }
            // drawing

            PathD subject = ClipperUtil.TPoint2DCollectionToClipperPathD(point2Ds);

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, subject);
            SvgUtils.AddSolution(svg, solution, true);
            string filename = @"..\..\..\Drawing_Earcut.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

            Assert.IsTrue(triangles.Any());
        }
        #endregion
    }
}
