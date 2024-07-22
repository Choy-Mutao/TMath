

using tmath;
using tmath.geo_math.curve;
using tmath.geometry;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Ray_Lab
    {
        [TestMethod]
        public void Test_IntersectWithSegment()
        {
            TRay2d ray2D = new TRay2d(TPoint2D.ORIGIN_2D, TVector2D.XAxis);
            TLineSegment2d segment1 = new TLineSegment2d(new TPoint2D(-1, -5), new TPoint2D(-1, 5));
            Assert.IsNull(ray2D.IntersectWithSegment(segment1));
            TLineSegment2d segment2 = new TLineSegment2d(new TPoint2D(1, -5), new TPoint2D(1, 5));
            Assert.IsNotNull(ray2D.IntersectWithSegment(segment2));
            Assert.IsTrue(((TPoint2D)ray2D.IntersectWithSegment(segment2)).IsEqualTo(new TPoint2D(1, 0)));
            TLineSegment2d segment3 = new TLineSegment2d(new TPoint2D(3, 5), new TPoint2D(3, 9));
            Assert.IsNull(ray2D.IntersectWithSegment(segment3));
            TLineSegment2d segment4 = new TLineSegment2d(new TPoint2D(-3, 5), new TPoint2D(-3, 9));
            Assert.IsNull(ray2D.IntersectWithSegment(segment4));
        }
    }
}
