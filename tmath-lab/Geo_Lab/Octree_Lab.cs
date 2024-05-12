using tmath;
using tmath.geometry;
using tmath.gs.spacial_structure;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Octree_Lab
    {
        #region Test Methods
        [TestMethod("Instancing")]
        public void Test_Octree_Constructor()
        {
            TBox3D oc_box = new TBox3D();
            Octree oc_tree = new Octree(oc_box);
            Assert.IsFalse(oc_tree is null);
        }

        [TestMethod("AddTriangles")]
        public void Test_Octree_AddTriangle()
        {
            TPoint3D t1 = new TPoint3D(0, 0, 0);
            TPoint3D t2 = new TPoint3D(1, 1, 0);
            TPoint3D t3 = new TPoint3D(0, 0, 1);

            TTriangle triangle = new TTriangle(t1, t2, t3);

            TBox3D oc_box = new TBox3D();
            Octree oc_tree = new Octree(oc_box);

            oc_tree.AddTriangle(triangle);

            Assert.IsFalse(oc_tree is null);
        }

        [TestMethod("RayIntersect")]
        public void Test_Octree_RayIntersect()
        {
            TPoint3D t1 = new TPoint3D(0, 0, 0);
            TPoint3D t2 = new TPoint3D(1, 1, 0);
            TPoint3D t3 = new TPoint3D(0, 0, 1);

            TTriangle triangle = new TTriangle(t1, t2, t3);

            TBox3D oc_box = new TBox3D();
            Octree oc_tree = new Octree(oc_box);

            oc_tree.AddTriangle(triangle);
            oc_tree.Build();

            // check
            TRay3d ray = new TRay3d(TPoint3D.O, TVector3D.XAxis);
            bool isInter = oc_tree.RayIntersect(ray, out double distance, out TTriangle tirangle, out TPoint3D position);

            Assert.IsFalse(isInter); // 不相交

            ray.origin = new TPoint3D(0, 0.25, 0.25);
            isInter = oc_tree.RayIntersect(ray, out distance, out tirangle, out position);
            Assert.IsFalse(isInter);

            ray.origin = new TPoint3D(1, 0.25, 0.25);
            ray.Negate();
            isInter = oc_tree.RayIntersect(ray, out distance, out tirangle, out position);
            Assert.IsTrue(isInter);
        }
        #endregion
    }
}
