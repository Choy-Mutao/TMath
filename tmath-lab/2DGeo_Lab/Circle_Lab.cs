using Clipper2Lib;
using System.Net;
using tmath;
using tmath.geo_math.curve;

namespace tmath_lab._2DGeo_Lab
{
    [TestClass]
    public class Circle_Lab
    {
        [TestMethod]
        public void Test_CircleGetPoint()
        {
            SvgWriter svg = new SvgWriter();

            var circle = new TCircle2D(new TPoint2D(0, 0), 5);
            double angle = 0, theta = NumberUtils.DegreeToRadian(10);
            var center = circle.Center;
            do
            {
                var pnt = circle.GetPointAtTheta(angle);
                SvgUtils.AddOpenSubject(svg, new PathsD() { new PathD() { new PointD(center.X, center.Y), new PointD(pnt.X, pnt.Y) } });
            } while ((angle += theta) < 2 * Math.PI);

            string filename = @"..\..\..\Test_CircleGetPoint.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_CircleDiscrete()
        {
            SvgWriter svg = new SvgWriter();

            var circle = new TCircle2D(new TPoint2D(0, 0), 5);
            double angle = 0, theta = NumberUtils.DegreeToRadian(10);
            var center = circle.Center;
            var dis = circle.Discretize(64);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis));

            string filename = @"..\..\..\Test_CircleGetPoint.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);

        }
    }
}
