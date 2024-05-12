using tmath;
using tmath.geometry;

namespace tmath_lab.math
{
    [TestClass]
    public class RayTest
    {
        [TestMethod("Instancing")]
        public void Test_Instancing()
        {
            TRay3d a = new TRay3d();
            Assert.IsTrue(a.origin.Equals(math_constans.pnt_zero3), "Passed!");
            Assert.IsTrue(a.direction.Equals(new TVector3D(0, 0, -1)), "Passed!");

            a = new TRay3d(math_constans.pnt_two3, math_constans.vec_one3);
            Assert.IsTrue(a.origin.Equals(math_constans.pnt_two3), "Passed!");
            Assert.IsTrue(a.direction.Equals(math_constans.vec_one3), "Passed!");
        }

        [TestMethod("SetByRowElements")]
        public void Test_Set()
        {
            TRay3d a = new TRay3d();

            a.Set(math_constans.pnt_one3, math_constans.vec_one3);
            Assert.IsTrue(a.origin.Equals(math_constans.pnt_one3), "Passed!");
            Assert.IsTrue(a.direction.Equals(math_constans.vec_one3), "Passed!");
        }

        [TestMethod("Recast/clone")]
        public void Test_Recast_Clone()
        {
            TRay3d a = new TRay3d(math_constans.pnt_one3, new TVector3D(0, 0, 1));
            Assert.IsTrue(a.Recast(0).Equals(a), "Passed!");
            TRay3d b = a.Clone();
            Assert.IsTrue(b.Recast(-1).Equals(new TRay3d(new TPoint3D(1, 1, 0), new TVector3D(0, 0, 1))), "Passed!");

            TRay3d c = a.Clone();
            Assert.IsTrue(c.Recast(1).Equals(new TRay3d(new TPoint3D(1, 1, 2), new TVector3D(0, 0, 1))), "Passed!");

            TRay3d d = a.Clone();
            TRay3d e = d.Clone().Recast(1);
            Assert.IsTrue(d.Equals(a), "Passed!");
            Assert.IsTrue(!e.Equals(d), "Passed!");
            Assert.IsTrue(e.Equals(c), "Passed!");
        }

        [TestMethod("copy/Equals")]
        public void Test_Copy_Equals()
        {
            TRay3d a = new TRay3d(math_constans.pnt_zero3, math_constans.vec_one3);
            TRay3d b = new TRay3d().Copy(a);
            Assert.IsTrue(b.origin.Equals(math_constans.pnt_zero3), "Passed!");
            Assert.IsTrue(b.direction.Equals(math_constans.vec_one3), "Passed!");

            // ensure that it is a true copy
            a.origin = math_constans.pnt_zero3;
            a.direction = math_constans.vec_one3;
            Assert.IsTrue(b.origin.Equals(math_constans.pnt_zero3), "Passed!");
            Assert.IsTrue(b.direction.Equals(math_constans.vec_one3), "Passed!");
        }

        [TestMethod("At")]
        public void Test_Clone()
        {
            TRay3d a = new TRay3d(math_constans.pnt_one3, new TVector3D(0, 0, 1));
            TPoint3D point = new TPoint3D();

            a.At(0, out point);
            Assert.IsTrue(point.Equals(math_constans.pnt_one3), "Passed!");
            a.At(-1, out point);
            Assert.IsTrue(point.Equals(new TPoint3D(1, 1, 0)), "Passed!");
            a.At(1, out point);
            Assert.IsTrue(point.Equals(new TPoint3D(1, 1, 2)), "Passed!");
        }

        [TestMethod("lookAt")]
        public void Test_LookAt()
        {
            TRay3d a = new TRay3d(math_constans.pnt_two3, math_constans.vec_one3);
            TPoint3D target = math_constans.pnt_one3;
            TVector3D expected = target.Sub(math_constans.pnt_two3).ToVector().GetNormal();

            a.LookAt(target);
            Assert.IsTrue(a.direction.Equals(expected), "Check if we\'re looking in the right direction");
        }

        [TestMethod("closestPointToPoint")]
        public void Test_ClosestPointToPoint()
        {

        }

        [TestMethod("distanceToPoint")]
        public void Test_DistanceToPoint()
        {

        }
        [TestMethod("distanceSqToPoint")]
        public void Test_DistanceSqToPoint() { }
        [TestMethod("distanceSqToSegment")]
        public void Test_DistanceSqToSegment() { }
        [TestMethod("intersectSphere")]
        public void Test_IntersectSphere() { }
        [TestMethod("intersectsSphere")]
        public void Test_IntersectsSphere() { }
        [TestMethod("distanceToPlane")]
        public void Test_DistanceToPlane()
        {
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }
        [TestMethod("intersectPlane")]
        public void Test_IntersectPlane()
        {
            TRay3d a = new TRay3d(math_constans.pnt_one3, new TVector3D(0, 0, 1));
            TPoint3D point = new TPoint3D();

            // parallel plane behind
            TPlane3D b = new TPlane3D().SetFromNormalAndCoplanarPoint(new TVector3D(0, 0, 1), new TPoint3D(1, 1, -1));
            point = a.IntersectPlane(b);
            Assert.IsTrue(point.Equals(math_constans.pnt_posInf3), "Passed!");

            // parallel plane coincident with origin
            TPlane3D c = new TPlane3D().SetFromNormalAndCoplanarPoint(new TVector3D(0, 0, 1), new TPoint3D(1, 1, 0));
            point = a.IntersectPlane(c);
            Assert.IsTrue(point.Equals(math_constans.pnt_posInf3), "Passed!");

            // parallel plane infront
            TPlane3D d = new TPlane3D().SetFromNormalAndCoplanarPoint(new TVector3D(0, 0, 1), new TPoint3D(1, 1, 1));
            point = a.IntersectPlane(d);
            Assert.IsTrue(point.Equals(a.origin), "Passed!");

            // perpendical ray that overlaps exactly
            TPlane3D e = new TPlane3D().SetFromNormalAndCoplanarPoint(new TVector3D(1, 0, 0), math_constans.pnt_one3);
            point = a.IntersectPlane(e);
            Assert.IsTrue(point.Equals(a.origin), "Passed!");

            // perpendical ray that doesn't overlap
            TPlane3D f = new TPlane3D().SetFromNormalAndCoplanarPoint(new TVector3D(1, 0, 0), math_constans.pnt_zero3);
            point = a.IntersectPlane(f);
            Assert.IsTrue(point.Equals(math_constans.pnt_posInf3), "Passed!");
        }
        [TestMethod("intersectsPlane")]
        public void TestIntersectsPlane() { }
        [TestMethod("intersectBox")]
        public void Test_IntersectBos() { }
        [TestMethod("intersectsBox")]
        public void Test_IntersectBox() { }
        [TestMethod("intersectTriangle")]
        public void Test_IntersectTriangle() { }
        [TestMethod("applyMatrix4")]
        public void Test_ApplyMatrix4() { }
        [TestMethod("Equals")]
        public void Test_Equals() { }
    }
}
