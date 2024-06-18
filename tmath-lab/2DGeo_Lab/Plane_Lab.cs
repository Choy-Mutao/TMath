using tmath;
using tmath.geometry;

namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Plane_Lab
    {
        private double x = 2, y = 3, z = 4, w = 5;
        private TVector3D one3 = new TVector3D(1, 1, 1);
        private TPoint3D zero3 = new TPoint3D();

        #region Test Methods
        [TestMethod("Instancing")]
        public void Test_Plane_Constructor()
        {
            TPlane3D a = new TPlane3D();
            Assert.IsTrue(a.Normal.X == 0, "Passed!");
            Assert.IsTrue(a.Normal.Y == 0, "Passed!");
            Assert.IsTrue(a.Normal.Z == 1, "Passed!");
            Assert.IsTrue(a.Constant == 0, "Passed!");

            a = new TPlane3D(one3.Clone(), 0);
            Assert.IsTrue(a.Normal.X == 1, "Passed!");
            Assert.IsTrue(a.Normal.Y == 1, "Passed!");
            Assert.IsTrue(a.Normal.Z == 1, "Passed!");
            Assert.IsTrue(a.Constant == 0, "Passed!");

            a = new TPlane3D(one3.Clone(), 1);
            Assert.IsTrue(a.Normal.X == 1, "Passed!");
            Assert.IsTrue(a.Normal.Y == 1, "Passed!");
            Assert.IsTrue(a.Normal.Z == 1, "Passed!");
            Assert.IsTrue(a.Constant == 1, "Passed!");
        }

        [TestMethod("Set")]
        public void Test_Plane_Set()
        {
            TPlane3D a = new TPlane3D();
            Assert.IsTrue(a.Normal.X == 0, "Passed!");
            Assert.IsTrue(a.Normal.Y == 0, "Passed!");
            Assert.IsTrue(a.Normal.Z == 1, "Passed!");
            Assert.IsTrue(a.Constant == 0, "Passed!");

            TPlane3D b = a.Clone();
            b.Set(new TVector3D(x, y, z), w);
            Assert.IsTrue(b.Normal.X == x, "Passed!");
            Assert.IsTrue(b.Normal.Y == y, "Passed!");
            Assert.IsTrue(b.Normal.Z == z, "Passed!");
            Assert.IsTrue(b.Constant == w, "Passed!");
        }

        [TestMethod("SetComponents")]
        public void Test_Plane_SetComponents()
        {
            TPlane3D a = new TPlane3D();
            Assert.IsTrue(a.Normal.X == 0, "Passed!");
            Assert.IsTrue(a.Normal.Y == 0, "Passed!");
            Assert.IsTrue(a.Normal.Z == 1, "Passed!");
            Assert.IsTrue(a.Constant == 0, "Passed!");

            TPlane3D b = a.Clone();
            b.SetComponents(x, y, z, w);
            Assert.IsTrue(b.Normal.X == x, "Passed!");
            Assert.IsTrue(b.Normal.Y == y, "Passed!");
            Assert.IsTrue(b.Normal.Z == z, "Passed!");
            Assert.IsTrue(b.Constant == w, "Passed!");
        }

        [TestMethod("SetFromNormalAndCoplanarPoint")]
        public void Test_Plane_SetFromNormalAndCoplanarPoint()
        {
            TVector3D Normal = one3.Clone();
            Normal.Normalize();
            TPlane3D a = new TPlane3D().SetFromNormalAndCoplanarPoint(Normal, zero3);

            Assert.IsTrue(a.Normal.Equals(Normal), "Passed!");
            Assert.IsTrue(a.Constant == 0, "Passed!");
        }

        [TestMethod("SetFromCoplanarPoints")]
        public void Test_Plane_setFromCoplanarPoints()
        {
            TPlane3D a = new TPlane3D();
            TPoint3D v1 = new TPoint3D(2.0, 0.5, 0.25);
            TPoint3D v2 = new TPoint3D(2.0, -0.5, 1.25);
            TPoint3D v3 = new TPoint3D(2.0, -3.5, 2.2);
            TVector3D Normal = new TVector3D(1, 0, 0);
            double Constant = -2;

            a.SetFromCoplanarPoints(v1, v2, v3);

            Assert.IsTrue(a.Normal.Equals(Normal), "Check Normal");
            Assert.IsTrue(a.Constant == Constant, "Check Constant");
        }

        [TestMethod("Clone")]
        public void Test_Plane_Clone()
        {
            TPlane3D a = new TPlane3D(new TVector3D(2.0, 0.5, 0.25));
            TPlane3D b = a.Clone();

            Assert.IsTrue(a.Equals(b), "clones are equal");
        }

        [TestMethod("Copy")]
        public void Test_Plane_copy()
        {
            TPlane3D a = new TPlane3D(new TVector3D(x, y, z), w);
            TPlane3D b = new TPlane3D();
            b.Copy(a);
            Assert.IsTrue(b.Normal.X == x, "Passed!");
            Assert.IsTrue(b.Normal.Y == y, "Passed!");
            Assert.IsTrue(b.Normal.Z == z, "Passed!");
            Assert.IsTrue(b.Constant == w, "Passed!");

            // ensure that it is a true Copy
            var an = a.Normal;
            an.X = 0; an.Y = -1; an.Z = -2;
            var ac = a.Constant;
            ac = -3;
            a.Set(an, ac);
            Assert.IsTrue(b.Normal.X == x, "Passed!");
            Assert.IsTrue(b.Normal.Y == y, "Passed!");
            Assert.IsTrue(b.Normal.Z == z, "Passed!");
            Assert.IsTrue(b.Constant == w, "Passed!");
        }

        [TestMethod("Normalize")]
        public void Test_Plane_Normalize()
        {
            TPlane3D a = new TPlane3D(new TVector3D(2, 0, 0), 2);

            a.Normalize();
            Assert.IsTrue(a.Normal.Length() == 1, "Passed!");
            Assert.IsTrue(a.Normal.Equals(new TVector3D(1, 0, 0)), "Passed!");
            Assert.IsTrue(a.Constant == 1, "Passed!");
        }

        [TestMethod("GetNegate/DistanceToPoint")]
        public void Test_Plane_negate_distanceToPoint()
        {
            TPlane3D a = new TPlane3D(new TVector3D(2, 0, 0), -2);

            a.Normalize();
            Assert.IsTrue(a.DistanceToPoint(new TPoint3D(4, 0, 0)) == 3, "Passed!");
            Assert.IsTrue(a.DistanceToPoint(new TPoint3D(1, 0, 0)) == 0, "Passed!");

            a.Negate();
            Assert.IsTrue(a.DistanceToPoint(new TPoint3D(4, 0, 0)) == -3, "Passed!");
            Assert.IsTrue(a.DistanceToPoint(new TPoint3D(1, 0, 0)) == 0, "Passed!");
        }

        [TestMethod("DistanceToPoint")]
        public void Test_Plane_distanceToPoint()
        {
            TPlane3D a = new TPlane3D(new TVector3D(2, 0, 0), -2);

            a.Normalize();

            TPoint3D point = a.ProjectPoint(zero3.Clone());
            Assert.IsTrue(a.DistanceToPoint(point) == 0, "Passed!");
            Assert.IsTrue(a.DistanceToPoint(new TPoint3D(4, 0, 0)) == 3, "Passed!");
        }

        [TestMethod("DistanceToSphere")]
        public void Test_Plane_distanceToSphere()
        {
            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);

            TSphere b = new TSphere(new TPoint3D(2, 0, 0), 1);

            Assert.IsTrue(a.DistanceToSphere(b) == 1, "Passed!");

            a.Set(new TVector3D(1, 0, 0), 2);
            Assert.IsTrue(a.DistanceToSphere(b) == 3, "Passed!");
            a.Set(new TVector3D(1, 0, 0), -2);
            Assert.IsTrue(a.DistanceToSphere(b) == -1, "Passed!");
        }

        [TestMethod("ProjectPoint")]
        public void Test_Plane_projectPoint()
        {
            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);
            TPoint3D point;

            point = a.ProjectPoint(new TPoint3D(10, 0, 0));
            Assert.IsTrue(point.Equals(zero3), "Passed!");
            point = a.ProjectPoint(new TPoint3D(-10, 0, 0));
            Assert.IsTrue(point.Equals(zero3), "Passed!");

            a = new TPlane3D(new TVector3D(0, 1, 0), -1);
            point = a.ProjectPoint(new TPoint3D(0, 0, 0));
            Assert.IsTrue(point.Equals(new TPoint3D(0, 1, 0)), "Passed!");
            point = a.ProjectPoint(new TPoint3D(0, 1, 0));
            Assert.IsTrue(point.Equals(new TPoint3D(0, 1, 0)), "Passed!");
        }

        [TestMethod("IntersectLine")]
        public void Test_Plane_intersectLine()
        {
            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);

            TLine3D l1 = new TLine3D(new TPoint3D(-10, 0, 0), new TPoint3D(10, 0, 0));

            a.IntersectLine(l1, out TPoint3D point);
            Assert.IsTrue(point.Equals(new TPoint3D(0, 0, 0)), "Passed!");

            a = new TPlane3D(new TVector3D(1, 0, 0), -3);
            a.IntersectLine(l1, out point);
            Assert.IsTrue(point.Equals(new TPoint3D(3, 0, 0)), "Passed!");
        }

        [TestMethod("intersectsLine")]
        public void Test_Plane_intersectsLine()
        {
            // intersectsLine( line ) // - boolean variant of above
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }

        [TestMethod("IntersectsBox")]
        public void Test_Plane_intersectsBox()
        {
            TBox3D a = new TBox3D(zero3.Clone(), one3.ToPoint());
            TPlane3D b = new TPlane3D(new TVector3D(0, 1, 0), 1);
            TPlane3D c = new TPlane3D(new TVector3D(0, 1, 0), 1.25);
            TPlane3D d = new TPlane3D(new TVector3D(0, -1, 0), 1.25);
            TPlane3D e = new TPlane3D(new TVector3D(0, 1, 0), 0.25);
            TPlane3D f = new TPlane3D(new TVector3D(0, 1, 0), -0.25);
            TPlane3D g = new TPlane3D(new TVector3D(0, 1, 0), -0.75);
            TPlane3D h = new TPlane3D(new TVector3D(0, 1, 0), -1);
            TPlane3D i = new TPlane3D(new TVector3D(1, 1, 1).GetNormal(), -1.732);
            TPlane3D j = new TPlane3D(new TVector3D(1, 1, 1).GetNormal(), -1.733);

            Assert.IsTrue(!b.IntersectsBox(a), "Passed!");
            Assert.IsTrue(!c.IntersectsBox(a), "Passed!");
            Assert.IsTrue(!d.IntersectsBox(a), "Passed!");
            Assert.IsTrue(!e.IntersectsBox(a), "Passed!");
            Assert.IsTrue(f.IntersectsBox(a), "Passed!");
            Assert.IsTrue(g.IntersectsBox(a), "Passed!");
            Assert.IsTrue(h.IntersectsBox(a), "Passed!");
            Assert.IsTrue(i.IntersectsBox(a), "Passed!");
            Assert.IsTrue(!j.IntersectsBox(a), "Passed!");
        }
        [TestMethod("IntersectsSphere")]
        public void Test_Plane_intersectsSphere()
        {
            TSphere a = new TSphere(zero3.Clone(), 1);
            TPlane3D b = new TPlane3D(new TVector3D(0, 1, 0), 1);
            TPlane3D c = new TPlane3D(new TVector3D(0, 1, 0), 1.25);
            TPlane3D d = new TPlane3D(new TVector3D(0, -1, 0), 1.25);

            Assert.IsTrue(b.IntersectsSphere(a), "Passed!");
            Assert.IsTrue(!c.IntersectsSphere(a), "Passed!");
            Assert.IsTrue(!d.IntersectsSphere(a), "Passed!");
        }

        [TestMethod("CoplanarPoint")]
        public void Test_Plane_coplanarPoint()
        {

            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);
            TPoint3D point = a.CoplanarPoint();
            Assert.IsTrue(a.DistanceToPoint(point) == 0, "Passed!");

            a = new TPlane3D(new TVector3D(0, 1, 0), -1);
            point = a.CoplanarPoint();
            Assert.IsTrue(a.DistanceToPoint(point) == 0, "Passed!");
        }

        [TestMethod("ApplyMatrix4/Translate")]
        public void Test_Plane_applyMatrix4_Translate()
        {
            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);

            TMatrix4 m = TMatrix4.MakeRotationZ(Math.PI * 0.5);

            //Assert.IsTrue(comparePlane(a.Clone().ApplyMatrix4(m), new TPlane(new TVector3D(0, 1, 0), 0)), "Passed!");
            var a1 = a.Clone(); a1.ApplyMatrix4(m);
            Assert.IsTrue(a1.Equals(new TPlane3D(new TVector3D(0, 1, 0), 0)), "Passed!");

            a = new TPlane3D(new TVector3D(0, 1, 0), -1);
            //Assert.IsTrue(comparePlane(a.Clone().ApplyMatrix4(m), new TPlane(new TVector3D(-1, 0, 0), -1)), "Passed!");
            var a2 = a.Clone(); a2.ApplyMatrix4(m);
            Assert.IsTrue(a2.Equals(new TPlane3D(new TVector3D(-1, 0, 0), -1)), "Passed!");

            m = TMatrix4.MakeTranslation(1, 1, 1);
            //Assert.IsTrue(comparePlane(a.Clone().ApplyMatrix4(m), a.Clone().Translate(new TVector3D(1, 1, 1))), "Passed!");
            var a3 = a.Clone(); a3.ApplyMatrix4(m);
            var a4 = a.Clone(); a4.Translate(new TVector3D(1, 1, 1));
            Assert.IsTrue(a3.Equals(a4), "Passed!");
        }

        [TestMethod("Equals")]
        public void Test_Plane_equals()
        {
            TPlane3D a = new TPlane3D(new TVector3D(1, 0, 0), 0);
            TPlane3D b = new TPlane3D(new TVector3D(1, 0, 0), 1);
            TPlane3D c = new TPlane3D(new TVector3D(0, 1, 0), 0);

            Assert.IsTrue(a.Normal.Equals(b.Normal), "Normals: equal");
            Assert.IsFalse(a.Normal.Equals(c.Normal), "Normals: not equal");

            Assert.AreNotEqual(a.Constant, b.Constant, "Constants: not equal");
            Assert.AreEqual(a.Constant, c.Constant, "Constants: equal");

            Assert.IsFalse(a.Equals(b), "Planes: not equal");
            Assert.IsFalse(a.Equals(c), "Planes: not equal");

            a.Copy(b);
            Assert.IsTrue(a.Normal.Equals(b.Normal), "Normals after Copy(): equal");
            Assert.AreEqual(a.Constant, b.Constant, "Constants after Copy(): equal");
            Assert.IsTrue(a.Equals(b), "Planes after Copy(): equal");
        }
        #endregion
    }
}
