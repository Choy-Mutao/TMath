using Clipper2Lib;
using tmath;
using tmath.geo_math.curve;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Arc_Lab
    {
        [TestMethod]
        public void Test_Discretize()
        {
            SvgWriter svg = new SvgWriter();


            //TArc2D arc2d = new TArc2D(new TPoint2D(10, 10), 100.0, (260.0 / 180.0 * Math.PI), (50.0 / 180.0 * Math.PI), ARC_DIR.CCW);
            //TPoint2DCollection dis = (TPoint2DCollection)arc2d.Discretize(64);
            //dis.Insert(0, new TPoint2D(10, 10));
            //dis.Add(new TPoint2D(10, 10));

            //SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis));

            //TArc2D arc2d2 = new TArc2D(new TPoint2D(10, 10), 100.0, (260.0 / 180.0 * Math.PI), Math.PI * 2.0 - (50.0 / 180.0 * Math.PI), ARC_DIR.CW);
            //TPoint2DCollection dis2 = (TPoint2DCollection)arc2d2.Discretize(64);
            //dis2.Insert(0, new TPoint2D(10, 10));
            //dis2.Add(new TPoint2D(10, 10));
            //SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(dis2));

            //TCircle2D circle = new TCircle2D(new TPoint2D(10, 10), 100.0);
            //TPoint2DCollection c_dis = (TPoint2DCollection)circle.Discretize(64);
            //SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(c_dis));

            TEllipse2D ellipse2D = new TEllipse2D(new TPoint2D(9, 4), 35, 6);
            var e_dis = (TPoint2DCollection)ellipse2D.Discretize(64);
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(e_dis));

            string filename = @"..\..\..\ClipperUtil_Test.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }
    }
}
