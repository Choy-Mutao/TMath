using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using tmath;

namespace tmath_lab.math
{
    [TestClass]
    public class QuaternionTest
    {
        static string[] orders = new string[6] { "XYZ", "YXZ", "ZXY", "ZYX", "YZX", "XZY" };

        static TEuler eulerAngles = new TEuler(0.1, -0.3, 0.25);

        private TEuler ChangeEulerOrder(TEuler euler, string order)
        {
            return new TEuler(euler.X, euler.Y, euler.Z, order);
        }

        [TestMethod("setFromEuler/setFromQuaternion")]
        public void Test_Quaternion_SetFromEuler_SetFromQuaternion()
        {
            TVector3D[] angles = new TVector3D[] { new TVector3D(1, 0, 0), new TVector3D(0, 1, 0), new TVector3D(0, 0, 1) };

            // ensure euler conversion to/from Quaternion matches.
            for (int i = 0; i < orders.Length; i++)
            {

                for (int j = 0; j < angles.Length; j++)
                {

                    TEuler eulers2 = TEuler.SetFromQuaternion(TQuaternion.SetFromEuler(new TEuler(angles[j].X, angles[j].Y, angles[j].Z, orders[i])), orders[i]);

                    var newAngle = new TVector3D(eulers2.X, eulers2.Y, eulers2.Z);
                    //Assert.IsTrue(newAngle.distanceTo(angles[j]) < 0.001, 'Passed!');
                    Assert.IsTrue(newAngle.IsEqualTo(angles[j], 0.001), "Passed!");
                }

            }

        }

        [TestMethod("setFromEuler/setFromRotationMatrix")]
        public void Test_Quaternion_SetFromEuler_SetFromRotationMatrix()
        {

            // ensure euler conversion for Quaternion matches that of Matrix4
            for (int i = 0; i < orders.Length; i++)
            {

                var q = TQuaternion.SetFromEuler(ChangeEulerOrder(eulerAngles, orders[i]));
                var m = TMatrix4.MakeRotationFromEuler(ChangeEulerOrder(eulerAngles, orders[i]));
                var q2 = TQuaternion.SetFromRotationMatrix(m);

                //assert.ok(qSub(q, q2).length() < 0.001, 'Passed!');
                Assert.IsTrue((q-q2).Length() < 0.001);
            }

        }
    }
}
