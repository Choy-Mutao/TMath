using tmath;
using tmath.pga;

namespace tmath_lab.PGA
{
    [TestClass]
    public class AreaTest
    {
        #region Test Methods
        [TestMethod]
        public void Test_IsLeft()
        {
            TPoint2D p1 = new TPoint2D(1.4, 0.7);
            TPoint2D p2 = new TPoint2D(0.7, 1.4);
            TPoint2D p3 = new TPoint2D(0.7, 0.7);

            double isLeft = Area.IsLeft(p1, p2, p3);
            var v1 = (p1 - p3).ToVector();
            var v2 = (p2 - p3).ToVector();
            double cross = v1.Perp(v2);
            Assert.IsTrue(cross == isLeft);
            Assert.IsTrue(isLeft > 0);

            isLeft = Area.IsLeft(p2, p1, p3);
            v1 = (p2 - p3).ToVector();
            v2 = (p1 - p3).ToVector();
            cross = v1.Perp(v2);
            Assert.IsTrue(cross == isLeft);
            Assert.IsTrue(isLeft < 0);
        }
        #endregion
    }
}
