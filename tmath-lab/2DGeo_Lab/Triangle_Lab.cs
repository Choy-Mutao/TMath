using tmath;
using tmath.geometry;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Triangle_Lab
    {
        [TestMethod("GetNormal")]
        public void Test_GetNormal()
        {
            TPoint3D a = new TPoint3D(1, 0, 0);
            TPoint3D b = new TPoint3D(0, 1, 0);
            TPoint3D c = new TPoint3D(0, 0, 1);

            TVector3D normal = TTriangle.GetNormal(a, b, c);
            Assert.IsTrue(normal.IsSameDirectionTo(new TVector3D(0.5, 0.5, 0.5)));
        }

    }
}
