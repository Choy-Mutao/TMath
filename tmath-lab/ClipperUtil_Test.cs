using Clipper2Lib;
using tmath;

namespace tmath_lab
{
    [TestClass]
    public class ClipperUtil_Test
    {
        [TestMethod]
        public void Test_Clipperutil_PathsDToTPoints()
        {
            PathsD solution = new PathsD();
            solution.Add(new PathD() { new PointD(1, 2), new PointD(3, 4), new PointD(5, 6), new PointD(7, 8), new PointD(9, 0) });
            var result = ClipperUtil.ClipperPathsDToTPoints(solution);
            Assert.IsNotNull(result);
        }
    }
}
