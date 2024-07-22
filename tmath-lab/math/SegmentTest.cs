using tmath;
using tmath.geo_math.curve;
using tmath.geometry;

namespace tmath_lab.math
{
    [TestClass]
    public class Segment2dTest
    {
        #region Test Methods
        #endregion
    }

    [TestClass]
    public class Segment3dTest
    {
        #region Test Methods
        [TestMethod("Instancing")]
        public void Test_Segment3d_Constructor_1()
        {

        }
        [TestMethod("Instancing")]
        public void Test_Segment3d_Constructor_2()
        {

        }
        [TestMethod("判断点是否在空间线段上")]
        public void Test_Segment3d_IsPointOn()
        {
            TLineSegment3d segment3d_1 = new TLineSegment3d(new TPoint3D(0, 0, 0), new TPoint3D(100, 100, 100));
            Assert.IsTrue(segment3d_1.IsPointOn(new TPoint3D(45, 45, 45)) == true);
            Assert.IsTrue(segment3d_1.IsPointOn(new TPoint3D(45, 46, 45)) == false);
        }
        [TestMethod("计算两个线段的交点")]
        public void Test_Segment3d_IntersectWith()
        {
            TLineSegment3d segment3d_1 = new TLineSegment3d(new TPoint3D(0, 0, 0), new TPoint3D(1, 1, 0));
            TLineSegment3d segment3d_2 = new TLineSegment3d(new TPoint3D(0, 1, 0), new TPoint3D(1, 0, 0));
            bool isInter = segment3d_1.IntersectWith(segment3d_2, new Tolerance(0, TConstant.Epsilon), out TPoint3D result);
            Assert.IsTrue(isInter && result.IsEqualTo(new TPoint3D(0.5, 0.5, 0)));
        }
        #endregion
    }
}
