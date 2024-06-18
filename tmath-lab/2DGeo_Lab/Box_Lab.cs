using tmath;
using tmath.geometry;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Box_Lab
    {
        #region Test Methods
        [TestMethod("Instancing")]
        public void Test_Box_Instancing()
        {
            var a = new TBox3D();
        }

        [TestMethod("GetCenter")]
        public void Test_Box_GetCenter()
        {
            var a = new TBox3D(TPoint3D.O, TPoint3D.O);
            var center = TPoint3D.O;

            var _get_center = a.GetCenter();
            Assert.IsTrue(_get_center.Equals(center));

            a = new TBox3D(TPoint3D.O, TPoint3D.I);
            var midPoint = TPoint3D.I * 0.5;
            _get_center = a.GetCenter();
            Assert.IsTrue(_get_center.Equals(midPoint));
        }

        [TestMethod("IntersectsTriangle")]
        public void Test_Box_IntersectsTriangle()
        {

            TBox3D a = new TBox3D(TPoint3D.I, TPoint3D.II);

            TTriangle b = new TTriangle(new TPoint3D(1.5, 1.5, 2.5), new TPoint3D(2.5, 1.5, 1.5), new TPoint3D(1.5, 2.5, 1.5));
            TTriangle c = new TTriangle(new TPoint3D(1.5, 1.5, 3.5), new TPoint3D(3.5, 1.5, 1.5), new TPoint3D(1.5, 1.5, 1.5));
            TTriangle d = new TTriangle(new TPoint3D(1.5, 1.75, 3), new TPoint3D(3, 1.75, 1.5), new TPoint3D(1.5, 2.5, 1.5));
            TTriangle e = new TTriangle(new TPoint3D(1.5, 1.8, 3), new TPoint3D(3, 1.8, 1.5), new TPoint3D(1.5, 2.5, 1.5));
            TTriangle f = new TTriangle(new TPoint3D(1.5, 2.5, 3), new TPoint3D(3, 2.5, 1.5), new TPoint3D(1.5, 2.5, 1.5));

            Assert.IsTrue(a.IntersectsTriangle(b), "Passed!");
            Assert.IsTrue(a.IntersectsTriangle(c), "Passed!");
            Assert.IsTrue(a.IntersectsTriangle(d), "Passed!");
            Assert.IsTrue(!a.IntersectsTriangle(e), "Passed!");
            Assert.IsTrue(!a.IntersectsTriangle(f), "Passed!");
        }
        #endregion
    }
}
