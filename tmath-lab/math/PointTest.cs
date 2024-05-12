using tmath;

namespace tmath_lab.math
{
    [TestClass]
    public class PointTest
    {
        [TestMethod("测试在TPointCollection的传值")]
        public void Test_TPointCollectionParams()
        {
            TPoint2DCollection pc1 = new TPoint2DCollection()
            {
                new TPoint2D(0,0),new TPoint2D(1,2),new TPoint2D(2,3),
            };
            TPoint2DCollection pc_copy = new TPoint2DCollection();
            pc_copy.AddRange(pc1);
            var tmp = pc1[0]; pc1[0] = pc1[1]; pc1[1] = pc1[2]; pc1[2] = tmp;

            Assert.IsTrue(pc_copy[0] != pc1[0]);
            Assert.IsTrue(pc_copy[1] != pc1[1]);
            Assert.IsTrue(pc_copy[2] != pc1[2]);

            Assert.IsTrue(pc_copy[0] == pc1[2]);
            Assert.IsTrue(pc_copy[1] == pc1[0]);
            Assert.IsTrue(pc_copy[2] == pc1[1]);
        }
    }
}
