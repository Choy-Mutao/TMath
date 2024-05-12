using tmath;
using tmath.geometry;
using tmath.GeometryUtil;

namespace tmath_lab.math
{
    [TestClass]
    public class Matrix2D_Lab
    {
        #region Test Methods
        [TestMethod("Instancing/SetElements/SetByRow/SetByCol")]
        public void Test_Matrix2D_Instancing()
        {
            TMatrix3 a = TMatrix3.I;
            Assert.IsTrue(a.Determinant() == 1, "Passed!");

            TMatrix3 b = new TMatrix3();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsTrue(b.row_elements[0] == 0);
            Assert.IsTrue(b.row_elements[1] == 1);
            Assert.IsTrue(b.row_elements[2] == 2);
            Assert.IsTrue(b.row_elements[3] == 3);
            Assert.IsTrue(b.row_elements[4] == 4);
            Assert.IsTrue(b.row_elements[5] == 5);
            Assert.IsTrue(b.row_elements[6] == 6);
            Assert.IsTrue(b.row_elements[7] == 7);
            Assert.IsTrue(b.row_elements[8] == 8);

            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 3);
            Assert.IsTrue(b.elements[2] == 6);
            Assert.IsTrue(b.elements[3] == 1);
            Assert.IsTrue(b.elements[4] == 4);
            Assert.IsTrue(b.elements[5] == 7);
            Assert.IsTrue(b.elements[6] == 2);
            Assert.IsTrue(b.elements[7] == 5);
            Assert.IsTrue(b.elements[8] == 8);

            Assert.IsTrue(a.Equals(b) == false, "Passed!");

            TMatrix3 c = new TMatrix3();
            c.SetByRowElements(b.row_elements);
            Assert.IsTrue(c.row_elements[0] == 0);
            Assert.IsTrue(c.row_elements[1] == 1);
            Assert.IsTrue(c.row_elements[2] == 2);
            Assert.IsTrue(c.row_elements[3] == 3);
            Assert.IsTrue(c.row_elements[4] == 4);
            Assert.IsTrue(c.row_elements[5] == 5);
            Assert.IsTrue(c.row_elements[6] == 6);
            Assert.IsTrue(c.row_elements[7] == 7);
            Assert.IsTrue(c.row_elements[8] == 8);

            Assert.IsTrue(c.elements[0] == 0);
            Assert.IsTrue(c.elements[1] == 3);
            Assert.IsTrue(c.elements[2] == 6);
            Assert.IsTrue(c.elements[3] == 1);
            Assert.IsTrue(c.elements[4] == 4);
            Assert.IsTrue(c.elements[5] == 7);
            Assert.IsTrue(c.elements[6] == 2);
            Assert.IsTrue(c.elements[7] == 5);
            Assert.IsTrue(c.elements[8] == 8);

            Assert.IsTrue(a.Equals(c) == false, "Passed!");

            TMatrix3 d = new TMatrix3();
            d.SetElements(b.elements);
            Assert.IsTrue(d.row_elements[0] == 0);
            Assert.IsTrue(d.row_elements[1] == 1);
            Assert.IsTrue(d.row_elements[2] == 2);
            Assert.IsTrue(d.row_elements[3] == 3);
            Assert.IsTrue(d.row_elements[4] == 4);
            Assert.IsTrue(d.row_elements[5] == 5);
            Assert.IsTrue(d.row_elements[6] == 6);
            Assert.IsTrue(d.row_elements[7] == 7);
            Assert.IsTrue(d.row_elements[8] == 8);

            Assert.IsTrue(d.elements[0] == 0);
            Assert.IsTrue(d.elements[1] == 3);
            Assert.IsTrue(d.elements[2] == 6);
            Assert.IsTrue(d.elements[3] == 1);
            Assert.IsTrue(d.elements[4] == 4);
            Assert.IsTrue(d.elements[5] == 7);
            Assert.IsTrue(d.elements[6] == 2);
            Assert.IsTrue(d.elements[7] == 5);
            Assert.IsTrue(d.elements[8] == 8);

            TMatrix3 e = new TMatrix3();
            e.SetByRowElements(new double[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.IsTrue(e.row_elements[0] == 0);
            Assert.IsTrue(e.row_elements[1] == 1);
            Assert.IsTrue(e.row_elements[2] == 2);
            Assert.IsTrue(e.row_elements[3] == 3);
            Assert.IsTrue(e.row_elements[4] == 4);
            Assert.IsTrue(e.row_elements[5] == 5);
            Assert.IsTrue(e.row_elements[6] == 6);
            Assert.IsTrue(e.row_elements[7] == 7);
            Assert.IsTrue(e.row_elements[8] == 8);

            Assert.IsTrue(e.elements[0] == 0);
            Assert.IsTrue(e.elements[1] == 3);
            Assert.IsTrue(e.elements[2] == 6);
            Assert.IsTrue(e.elements[3] == 1);
            Assert.IsTrue(e.elements[4] == 4);
            Assert.IsTrue(e.elements[5] == 7);
            Assert.IsTrue(e.elements[6] == 2);
            Assert.IsTrue(e.elements[7] == 5);
            Assert.IsTrue(e.elements[8] == 8);


            TMatrix3 f = new TMatrix3();
            f.SetElements(new double[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.IsTrue(f.elements[0] == 0);
            Assert.IsTrue(f.elements[1] == 1);
            Assert.IsTrue(f.elements[2] == 2);
            Assert.IsTrue(f.elements[3] == 3);
            Assert.IsTrue(f.elements[4] == 4);
            Assert.IsTrue(f.elements[5] == 5);
            Assert.IsTrue(f.elements[6] == 6);
            Assert.IsTrue(f.elements[7] == 7);
            Assert.IsTrue(f.elements[8] == 8);

            Assert.IsTrue(f.row_elements[0] == 0);
            Assert.IsTrue(f.row_elements[1] == 3);
            Assert.IsTrue(f.row_elements[2] == 6);
            Assert.IsTrue(f.row_elements[3] == 1);
            Assert.IsTrue(f.row_elements[4] == 4);
            Assert.IsTrue(f.row_elements[5] == 7);
            Assert.IsTrue(f.row_elements[6] == 2);
            Assert.IsTrue(f.row_elements[7] == 5);
            Assert.IsTrue(f.row_elements[8] == 8);

            Assert.IsTrue(a.Equals(c) == false, "Passed!");
        }

        [TestMethod("I")]
        public void Test_Matrix2D_Identity()
        {

            TMatrix3 b = new TMatrix3();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);

            TMatrix3 a = TMatrix3.I;
            Assert.IsTrue(a.Equals(b) == false, "Passed!");

            b.Identity();
            Assert.IsTrue(a.Equals(b) == true, "Passed!");
        }

        [TestMethod("Clone")]
        public void Test_Matrix2D_Clone()
        {

            TMatrix3 a = new TMatrix3(0, 1, 2, 3, 4, 5, 6, 7, 8);
            TMatrix3 b = a.Clone();

            Assert.IsTrue(a.Equals(b), "Passed!");

            // ensure that it is a true copy
            var elements = a.row_elements; elements[0] = 2;
            a.SetByRowElements(elements);
            Assert.IsTrue(!a.Equals(b), "Passed!");

        }

        [TestMethod("Copy")]
        public void Test_Matrix2D_Copy()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            TMatrix3 b = new TMatrix3();
            b.Copy(a);

            Assert.IsTrue(a.Equals(b), "Passed!");

            // ensure that it is a true copy
            var elements = a.row_elements; elements[0] = 2;
            a.SetByRowElements(elements);
            Assert.IsTrue(!a.Equals(b), "Passed!");
        }

        [TestMethod("ExtractBasis")]
        public void Test_Matrix2D_ExtractBasis()
        {
            // extractBasis( xAxis, yAxis, zAxis )
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }

        [TestMethod("SetFromMatrix4")]
        public void Test_Matrix2D_SetFromMatrix4()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            TMatrix3 b = new TMatrix3();
            TMatrix3 c = new TMatrix3();
            c.Set(0, 1, 2, 4, 5, 6, 8, 9, 10);
            b.SetFromMatrix4(a);
            Assert.IsTrue(b.Equals(c));
        }

        [TestMethod("Postmultiply/Premultiply")]
        public void Test_Matrix2D_Multiply_Premultiply()
        {
            // both simply just wrap multiplyMatrices
            TMatrix3 a = new TMatrix3();
            a.Set(2, 3, 5, 7, 11, 13, 17, 19, 23);
            TMatrix3 b = new TMatrix3();
            b.Set(29, 31, 37, 41, 43, 47, 53, 59, 61);

            double[] expectedMultiply = new double[] { 446, 1343, 2491, 486, 1457, 2701, 520, 1569, 2925 };
            double[] expectedPremultiply = new double[] { 904, 1182, 1556, 1131, 1489, 1967, 1399, 1845, 2435 };

            a.PostMultiply(b);
            Assert.IsTrue(a.elements.SequenceEqual(expectedMultiply), "post-multiply: check result");

            a.Set(2, 3, 5, 7, 11, 13, 17, 19, 23);
            a.PreMultiply(b);
            Assert.IsTrue(a.elements.SequenceEqual(expectedPremultiply), "pre-multiply: check result");

            TMatrix3 I = TMatrix3.I;
            I.PreMultiply(TMatrix3.MakeTranslation(10, 10));
            Assert.IsTrue(true);
        }
        [TestMethod("MultiplyMatrices")]
        public void Test_Matrix2D_MultiplyMatrices()
        {
            // Reference:
            //
            // #!/usr/bin/env python
            // from __future__ import print_function
            // import numpy as np
            // print(
            //     np.dot(
            //         np.reshape([2, 3, 5, 7, 11, 13, 17, 19, 23], (3, 3)),
            //         np.reshape([29, 31, 37, 41, 43, 47, 53, 59, 61], (3, 3))
            //     )
            // )
            //
            // [[ 446  486  520]
            //  [1343 1457 1569]
            //  [2491 2701 2925]]
            TMatrix3 lhs = new TMatrix3();
            lhs.Set(2, 3, 5, 7, 11, 13, 17, 19, 23);
            TMatrix3 rhs = new TMatrix3();
            rhs.Set(29, 31, 37, 41, 43, 47, 53, 59, 61);

            TMatrix3 ans = TMatrix3.MultiplyMatrices(lhs, rhs);

            Assert.IsTrue(ans.elements[0] == 446);
            Assert.IsTrue(ans.elements[1] == 1343);
            Assert.IsTrue(ans.elements[2] == 2491);
            Assert.IsTrue(ans.elements[3] == 486);
            Assert.IsTrue(ans.elements[4] == 1457);
            Assert.IsTrue(ans.elements[5] == 2701);
            Assert.IsTrue(ans.elements[6] == 520);
            Assert.IsTrue(ans.elements[7] == 1569);
            Assert.IsTrue(ans.elements[8] == 2925);
        }
        [TestMethod("MultiplyScalar")]
        public void Test_Matrix2D_MultiplyScalar()
        {
            TMatrix3 b = new TMatrix3();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 3);
            Assert.IsTrue(b.elements[2] == 6);
            Assert.IsTrue(b.elements[3] == 1);
            Assert.IsTrue(b.elements[4] == 4);
            Assert.IsTrue(b.elements[5] == 7);
            Assert.IsTrue(b.elements[6] == 2);
            Assert.IsTrue(b.elements[7] == 5);
            Assert.IsTrue(b.elements[8] == 8);

            b.MultiplyScalar(2);
            Assert.IsTrue(b.elements[0] == 0 * 2);
            Assert.IsTrue(b.elements[1] == 3 * 2);
            Assert.IsTrue(b.elements[2] == 6 * 2);
            Assert.IsTrue(b.elements[3] == 1 * 2);
            Assert.IsTrue(b.elements[4] == 4 * 2);
            Assert.IsTrue(b.elements[5] == 7 * 2);
            Assert.IsTrue(b.elements[6] == 2 * 2);
            Assert.IsTrue(b.elements[7] == 5 * 2);
            Assert.IsTrue(b.elements[8] == 8 * 2);
        }

        [TestMethod("Determinant")]
        public void Test_Matrix2D_Determinant()
        {
            TMatrix3 a = TMatrix3.I;
            double[] ae = a.elements;

            Assert.IsTrue(a.Determinant() == 1, "Passed!");

            //a.elements[0] = 2;
            ae[0] = 2; a.SetElements(ae);
            Assert.IsTrue(a.Determinant() == 2, "Passed!");

            //a.elements[0] = 0;
            ae[0] = 0; a.SetElements(ae);
            Assert.IsTrue(a.Determinant() == 0, "Passed!");

            // calculated via http://www.euclideanspace.com/maths/algebra/matrix/functions/Determinant/threeD/index.htm
            a.Set(2, 3, 4, 5, 13, 7, 8, 9, 11);
            Assert.IsTrue(a.Determinant() == -73, "Passed!");
        }
        [TestMethod("Invert")]
        public void Test_Matrix2D_Invert()
        {
            TMatrix3 zero = new TMatrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
            TMatrix4 identity4 = TMatrix4.I;
            TMatrix3 a = new TMatrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
            TMatrix3 b = new TMatrix3();

            b.Copy(a);
            b.Invert();

            Assert.IsTrue(b.Equals(zero), "Matrix a is zero matrix");

            TMatrix4[] testMatrices = new TMatrix4[8]
            {
                TMatrix4.MakeRotationX(0.3),
                TMatrix4.MakeRotationX(-0.3),

                TMatrix4.MakeRotationY(0.3),
                TMatrix4.MakeRotationY(-0.3),

                TMatrix4.MakeRotationZ(0.3),
                TMatrix4.MakeRotationZ(-0.3),

                TMatrix4.MakeScale(1, 2, 3),
                TMatrix4.MakeScale(1 / 8.0, 1 / 2.0, 1 / 3.0)
            };

            for (int i = 0, il = testMatrices.Length; i < il; i++)
            {

                var m = testMatrices[i];

                a.SetFromMatrix4(m);
                b.Copy(a);
                TMatrix3 mInverse3 = b;
                mInverse3.Invert();

                TMatrix4 mInverse = mInverse3.ToMatrix4();

                // the determinant of the inverse should be the reciprocal
                Assert.IsTrue(Math.Abs(a.Determinant() * mInverse3.Determinant() - 1) < 0.0001, "Passed!");
                Assert.IsTrue(Math.Abs(m.Determinant() * mInverse.Determinant() - 1) < 0.0001, "Passed!");

                TMatrix4 mProduct = TMatrix4.MultiplyMatrices(m, mInverse);
                Assert.IsTrue(Math.Abs(mProduct.Determinant() - 1) < 0.0001, "Passed!");
                Assert.IsTrue(mProduct.Equals(identity4), "Passed!");

            }

        }
        [TestMethod("Transpose")]
        public void Test_Matrix2D_Transpose()
        {
            TMatrix3 a = TMatrix3.I;
            TMatrix3 b = a.Clone();
            b.Transpose();
            Assert.IsTrue(a.Equals(b), "Passed!");

            b = new TMatrix3();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            TMatrix3 c = b.Clone();
            c.Transpose();
            Assert.IsTrue(b.Equals(c) == false, "Passed!");
            c.Transpose();
            Assert.IsTrue(b.Equals(c) == true, "Passed!");
        }

        [TestMethod("GetNormalMatrix")]
        public void Test_Matrix2D_GetNormalMatrix()
        {
            TMatrix3 a = TMatrix3.I;
            TMatrix4 b = new TMatrix4();
            b.Set(
                2, 3, 5, 7,
                11, 13, 17, 19,
                23, 29, 31, 37,
                41, 43, 47, 57
            );
            TMatrix3 expected = new TMatrix3();
            expected.Set(
                -1.2857142857142856, 0.7142857142857143, 0.2857142857142857,
                0.7428571428571429, -0.7571428571428571, 0.15714285714285714,
                -0.19999999999999998, 0.3, -0.09999999999999999
            );

            a.GetNormalMatrix(b);
            Assert.IsTrue(a.Equals(expected), "Check resulting TMatrix3");
        }

        [TestMethod("TransposeIntoArray")]
        public void Test_Matrix2D_TransposeIntoArray()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            double[] b = a.TransposeIntoArray();
            Assert.IsTrue(b[0] == 0);
            Assert.IsTrue(b[1] == 1);
            Assert.IsTrue(b[2] == 2);
            Assert.IsTrue(b[3] == 3);
            Assert.IsTrue(b[4] == 4);
            Assert.IsTrue(b[5] == 5);
            Assert.IsTrue(b[5] == 5);
            Assert.IsTrue(b[6] == 6);
            Assert.IsTrue(b[7] == 7);
            Assert.IsTrue(b[8] == 8);
        }

        [TestMethod("SetUVTransform")]
        public void Test_Matrix2D_SetUVTransform()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(
                0.1767766952966369, 0.17677669529663687, 0.32322330470336313,
                -0.17677669529663687, 0.1767766952966369, 0.5,
                0, 0, 1
            );

            UVParams param = new UVParams();

            param.centerX = 0.5;
            param.centerY = 0.5;
            param.offsetX = 0;
            param.offsetY = 0;
            param.repeatX = 0.25;
            param.repeatY = 0.25;
            param.rotation = 0.7753981633974483;

            TMatrix3 b = TMatrix3.I;
            a.SetUVTransform(param.offsetX, param.offsetY,
                             param.repeatX, param.repeatY,
                             param.rotation,
                             param.centerX, param.centerY);

            b.Identity();
            b.Translate(-param.centerX, -param.centerY);
            b.Rotate(param.rotation);
            b.Scale(param.repeatX, param.repeatY);
            b.Translate(param.centerX, param.centerY);
            b.Translate(param.offsetX, param.offsetY);

            TMatrix3 expected = new TMatrix3();
            expected.Set(
                        0.1785355940258599, 0.17500011904519763, 0.32323214346447127,
                        -0.17500011904519763, 0.1785355940258599, 0.4982322625096689,
                        0, 0, 1
                    );

            Assert.IsTrue(a.Equals(expected), "Check direct method");
            Assert.IsTrue(b.Equals(expected), "Check indirect method");
        }

        [TestMethod("Scale")]
        public void Test_Matrix2D_Scale()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9);
            TMatrix3 expected = new TMatrix3();
            expected.Set(
                0.25, 0.5, 0.75,
                1, 1.25, 1.5,
                7, 8, 9
            );

            a.Scale(0.25, 0.25);
            Assert.IsTrue(a.Equals(expected), "Check scaling result");
        }

        [TestMethod("Rotate")]
        public void Test_Matrix2D_Rotate()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9);
            TMatrix3 expected = new TMatrix3();
            expected.Set(
                3.5355339059327373, 4.949747468305833, 6.363961030678928,
                2.121320343559643, 2.121320343559643, 2.1213203435596433,
                7, 8, 9
            );

            a.Rotate(Math.PI / 4);
            Assert.IsTrue(a.Equals(expected), "Check rotated result");
        }

        [TestMethod("Translate")]
        public void Test_Matrix2D_Translate()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9);
            TMatrix3 expected = new TMatrix3();
            expected.Set(22, 26, 30, 53, 61, 69, 7, 8, 9);

            a.Translate(3, 7);
            Assert.IsTrue(a.Equals(expected), "Check translation result");
        }

        [TestMethod("MakeTranslation")]
        public void Test_Matrix2D_MakeTranslation()
        {
            TVector2D b = new TVector2D(1, 2);

            TMatrix3 c = new TMatrix3();
            c.Set(1, 0, 1, 0, 1, 2, 0, 0, 1);

            TMatrix3 a = TMatrix3.MakeTranslation(b.X, b.Y);
            Assert.IsTrue(a.Equals(c), "Check translation result");

            a = TMatrix3.MakeTranslation(b);
            Assert.IsTrue(a.Equals(c), "Check translation result");
        }

        [TestMethod("MakeRotation")]
        public void Test_Matrix2D_MakeRotation()
        {
            // makeRotation( theta ) // counterclockwise
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }

        [TestMethod("MakeScale")]
        public void Test_Matrix2D_MakeScale()
        {
            // makeScale( x, y )
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }

        [TestMethod("Equals")]
        public void Test_Matrix2D_Equals()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8);
            TMatrix3 b = new TMatrix3();
            b.Set(0, -1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsFalse(a.Equals(b), "Check that a does not equal b");
            Assert.IsFalse(b.Equals(a), "Check that a does not equal b");

            a.Copy(b);
            Assert.IsTrue(a.Equals(b), "Check that a equals b after copy()");
            Assert.IsTrue(b.Equals(a), "Check that b equals a after copy()");
        }

        [TestMethod("FromArray")]
        public void Test_Matrix2D_FromArray()
        {
            TMatrix3 b = new TMatrix3();
            b.FromArray(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 1);
            Assert.IsTrue(b.elements[2] == 2);
            Assert.IsTrue(b.elements[3] == 3);
            Assert.IsTrue(b.elements[4] == 4);
            Assert.IsTrue(b.elements[5] == 5);
            Assert.IsTrue(b.elements[6] == 6);
            Assert.IsTrue(b.elements[7] == 7);
            Assert.IsTrue(b.elements[8] == 8);

            b = new TMatrix3();
            b.FromArray(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }, 10);

            Assert.IsTrue(b.elements[0] == 10);
            Assert.IsTrue(b.elements[1] == 11);
            Assert.IsTrue(b.elements[2] == 12);
            Assert.IsTrue(b.elements[3] == 13);
            Assert.IsTrue(b.elements[4] == 14);
            Assert.IsTrue(b.elements[5] == 15);
            Assert.IsTrue(b.elements[6] == 16);
            Assert.IsTrue(b.elements[7] == 17);
            Assert.IsTrue(b.elements[8] == 18);
        }

        [TestMethod("ToArray")]
        public void Test_Matrix2D_ToArray()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9);
            double[] noOffset = new double[] { 1, 4, 7, 2, 5, 8, 3, 6, 9 };
            double[] withOffset = new double[] { double.NaN, 1, 4, 7, 2, 5, 8, 3, 6, 9 };

            double[] array = a.ToArray();
            Assert.IsTrue(array.SequenceEqual(noOffset), "No array, no offset");

            array = a.ToArray();
            Assert.IsTrue(array.SequenceEqual(noOffset), "With array, no offset");

            array = a.ToArray(1);
            Assert.IsTrue(array.SequenceEqual(withOffset), "With array, with offset");
        }

        #endregion
    }

    [TestClass]
    public class Matrix3D_Lab
    {
        #region Test Methods
        [TestMethod("Instancing")]
        public void Test_Matrix3d_Intancing()
        {
            TMatrix4 a = TMatrix4.I;
            Assert.IsTrue(a.Determinant() == 1, "Passed!");

            TMatrix4 b = new TMatrix4();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 4);
            Assert.IsTrue(b.elements[2] == 8);
            Assert.IsTrue(b.elements[3] == 12);
            Assert.IsTrue(b.elements[4] == 1);
            Assert.IsTrue(b.elements[5] == 5);
            Assert.IsTrue(b.elements[6] == 9);
            Assert.IsTrue(b.elements[7] == 13);
            Assert.IsTrue(b.elements[8] == 2);
            Assert.IsTrue(b.elements[9] == 6);
            Assert.IsTrue(b.elements[10] == 10);
            Assert.IsTrue(b.elements[11] == 14);
            Assert.IsTrue(b.elements[12] == 3);
            Assert.IsTrue(b.elements[13] == 7);
            Assert.IsTrue(b.elements[14] == 11);
            Assert.IsTrue(b.elements[15] == 15);

            Assert.IsTrue(!a.Equals(b), "Passed!");

            TMatrix4 c = new TMatrix4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(c.elements[0] == 0);
            Assert.IsTrue(c.elements[1] == 4);
            Assert.IsTrue(c.elements[2] == 8);
            Assert.IsTrue(c.elements[3] == 12);
            Assert.IsTrue(c.elements[4] == 1);
            Assert.IsTrue(c.elements[5] == 5);
            Assert.IsTrue(c.elements[6] == 9);
            Assert.IsTrue(c.elements[7] == 13);
            Assert.IsTrue(c.elements[8] == 2);
            Assert.IsTrue(c.elements[9] == 6);
            Assert.IsTrue(c.elements[10] == 10);
            Assert.IsTrue(c.elements[11] == 14);
            Assert.IsTrue(c.elements[12] == 3);
            Assert.IsTrue(c.elements[13] == 7);
            Assert.IsTrue(c.elements[14] == 11);
            Assert.IsTrue(c.elements[15] == 15);

            Assert.IsTrue(!a.Equals(c), "Passed!");
        }

        [TestMethod("SetByRowElements")]
        public void Test_Matrix3d_Set()
        {
            TMatrix4 b = TMatrix4.I;
            Assert.IsTrue(b.Determinant() == 1, "Passed!");

            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 4);
            Assert.IsTrue(b.elements[2] == 8);
            Assert.IsTrue(b.elements[3] == 12);
            Assert.IsTrue(b.elements[4] == 1);
            Assert.IsTrue(b.elements[5] == 5);
            Assert.IsTrue(b.elements[6] == 9);
            Assert.IsTrue(b.elements[7] == 13);
            Assert.IsTrue(b.elements[8] == 2);
            Assert.IsTrue(b.elements[9] == 6);
            Assert.IsTrue(b.elements[10] == 10);
            Assert.IsTrue(b.elements[11] == 14);
            Assert.IsTrue(b.elements[12] == 3);
            Assert.IsTrue(b.elements[13] == 7);
            Assert.IsTrue(b.elements[14] == 11);
            Assert.IsTrue(b.elements[15] == 15);
        }

        [TestMethod("Identity")]
        public void Test_Matrix3d_Identity()
        {
            TMatrix4 b = new TMatrix4();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 4);
            Assert.IsTrue(b.elements[2] == 8);
            Assert.IsTrue(b.elements[3] == 12);
            Assert.IsTrue(b.elements[4] == 1);
            Assert.IsTrue(b.elements[5] == 5);
            Assert.IsTrue(b.elements[6] == 9);
            Assert.IsTrue(b.elements[7] == 13);
            Assert.IsTrue(b.elements[8] == 2);
            Assert.IsTrue(b.elements[9] == 6);
            Assert.IsTrue(b.elements[10] == 10);
            Assert.IsTrue(b.elements[11] == 14);
            Assert.IsTrue(b.elements[12] == 3);
            Assert.IsTrue(b.elements[13] == 7);
            Assert.IsTrue(b.elements[14] == 11);
            Assert.IsTrue(b.elements[15] == 15);

            TMatrix4 a = TMatrix4.I;
            Assert.IsTrue(!a.Equals(b), "Passed!");

            b.Identity();
            Assert.IsTrue(a.Equals(b), "Passed!");
        }

        [TestMethod("Clone")]
        public void Test_Matrix3d_Clone()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            TMatrix4 b = a.Clone();

            Assert.IsTrue(a.Equals(b), "Passed!");

            // ensure that it is a true copy
            var ae = a.elements; ae[0] = 2;
            a.SetElements(ae);
            Assert.IsTrue(!a.Equals(b), "Passed!");
        }

        [TestMethod("Copy")]
        public void Test_Matrix3d_Copy()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            TMatrix4 b = new TMatrix4();
            b.Copy(a);

            Assert.IsTrue(a.Equals(b), "Passed!");

            // ensure that it is a true copy
            var ae = a.elements; ae[0] = 2;
            a.SetElements(ae);
            Assert.IsTrue(!a.Equals(b), "Passed!");
        }

        [TestMethod("SetFromMatrix3")]
        public void Test_Matrix3d_SetFromMatrix3()
        {
            TMatrix3 a = new TMatrix3();
            a.Set(
                0, 1, 2,
                3, 4, 5,
                6, 7, 8
            );
            TMatrix4 b = new TMatrix4();
            TMatrix4 c = new TMatrix4();
            c.Set(
                0, 1, 2, 0,
                3, 4, 5, 0,
                6, 7, 8, 0,
                0, 0, 0, 1
            );
            b.SetFromMatrix3(a);
            Assert.IsTrue(b.Equals(c));
        }

        [TestMethod("CopyPosition")]
        public void Test_Matrix3d_CopyPosition()
        {
            TMatrix4 a = new TMatrix4(); a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            TMatrix4 b = new TMatrix4(); b.Set(1, 2, 3, 0, 5, 6, 7, 0, 9, 10, 11, 0, 13, 14, 15, 16);

            Assert.IsFalse(a.Equals(b), "a and b initially not equal");

            b.CopyPosition(a);
            Assert.IsTrue(a.Equals(b), "a and b equal after copyPosition()");
        }

        [TestMethod("MakeBasis/ExtractBasis")]
        public void Test_Matrix3d_MakeBasis_ExtractBasis()
        {
            TVector3D[] identityBasis = new TVector3D[3] { new TVector3D(1, 0, 0), new TVector3D(0, 1, 0), new TVector3D(0, 0, 1) };
            TMatrix4 a = new TMatrix4();
            a.MakeBasis(identityBasis[0], identityBasis[1], identityBasis[2]);

            TMatrix4 identity = TMatrix4.I;

            Assert.IsTrue(a.Equals(identity), "Passed!");

            TVector3D[][] testBases = new TVector3D[][] { new TVector3D[3] { new TVector3D(0, 1, 0), new TVector3D(-1, 0, 0), new TVector3D(0, 0, 1) } };
            for (int i = 0; i < testBases.Length; i++)
            {
                TVector3D[] testBasis = testBases[i];

                TMatrix4 b = new TMatrix4();
                b.MakeBasis(testBasis[0], testBasis[1], testBasis[2]);

                TVector3D[] outBasis = new TVector3D[3] { new TVector3D(), new TVector3D(), new TVector3D() };
                b.ExtractBasis(out outBasis[0], out outBasis[1], out outBasis[2]);

                // check what goes in, is what comes out.
                for (int j = 0; j < outBasis.Length; j++)
                {
                    Assert.IsTrue(outBasis[j].Equals(testBasis[j]), "Passed!");
                }

                // get the basis out the hard war
                for (int j = 0; j < identityBasis.Length; j++)
                {
                    outBasis[j].Copy(identityBasis[j]);
                    outBasis[j].ApplyMatrix4(b);
                }

                // did the multiply method of basis extraction work?
                for (int j = 0; j < outBasis.Length; j++)
                {
                    Assert.IsTrue(outBasis[j].Equals(testBasis[j]), "Passed!");
                }

            }

        }

        [TestMethod("MakeRotationFromEuler/extractRotation")]
        public void Test_Matrix3d_MakeRotationFromEuler_ExtractRotation()
        {
            TEuler[] testValues = new TEuler[5] {
                new TEuler(0, 0, 0, "XYZ"),
                new TEuler(1, 0, 0, "XYZ"),
                new TEuler(0, 1, 0, "ZYX"),
                new TEuler(0, 0, 0.5, "YZX"),
                new TEuler(0, 0, -0.5, "YZX") };

            for (int i = 0; i < testValues.Length; i++)
            {

                TEuler v = testValues[i];
                TMatrix4 m = TMatrix4.MakeRotationFromEuler(v);

                TEuler v2 = TEuler.SetFromRotationMatrix(m, v.Order);
                TMatrix4 m2 = TMatrix4.MakeRotationFromEuler(v2);

                Assert.IsTrue(m.Equals(m2, TConstant.Epsilon), "makeRotationFromEuler #" + i + ": original and TEuler-derived matrices are equal");
                Assert.IsTrue(v.Equals(v2, TConstant.Epsilon), "makeRotationFromEuler #" + i + ": original and matrix-derived Eulers are equal");

                TMatrix4 m3 = TMatrix4.ExtractRotation(m2);
                TEuler v3 = TEuler.SetFromRotationMatrix(m3, v.Order);

                Assert.IsTrue(m.Equals(m3, TConstant.Epsilon), "extractRotation #" + i + ": original and extracted matrices are equal");
                Assert.IsTrue(v.Equals(v3, TConstant.Epsilon), "extractRotation #" + i + ": original and extracted Eulers are equal");

            }

        }

        [TestMethod("MakeRotationFromQuaternion")]
        public void Test_Matrix3d_MakeRotationFromQuaternion()
        {
            // makeRotationFromQuaternion( q )
            Assert.IsTrue(false, "everything\'s gonna be alright");
        }

        [TestMethod("LookAt")]
        public void Test_Matrix3d_LookAt()
        {
            TMatrix4 a;

            TMatrix4 expected = new TMatrix4();
            expected.Identity();

            TVector3D eye = new TVector3D(0, 0, 0);
            TVector3D target = new TVector3D(0, 1, -1);
            TVector3D up = new TVector3D(0, 1, 0);

            a = TMatrix4.LookAt(eye, target, up);
            TEuler rotation = TEuler.SetFromRotationMatrix(a);
            Assert.IsTrue(rotation.X * (180 / Math.PI) == 45, "Check the rotation");

            // eye and target are in the same position
            eye.Copy(target);
            a = TMatrix4.LookAt(eye, target, up);
            Assert.IsTrue(a.Equals(expected), "Check the result for eye == target");

            // up and z are parallel
            eye.Set(0, 1, 0);
            target.Set(0, 0, 0);
            a = TMatrix4.LookAt(eye, target, up);
            expected.Set(
                1, 0, 0, 0,
                0, 0.0001, 1, 0,
                0, -1, 0.0001, 0,
                0, 0, 0, 1
            );
            Assert.IsTrue(a.Equals(expected), "Check the result for when up and z are parallel");
        }

        [TestMethod("Postmultiply")]
        public void Test_Matrix3d_Postmultiply()
        {
            TMatrix4 lhs = new TMatrix4(); lhs.Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);
            TMatrix4 rhs = new TMatrix4(); rhs.Set(59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131);

            lhs.PostMultiply(rhs);

            Assert.IsTrue(lhs.elements[0] == 1585);
            Assert.IsTrue(lhs.elements[1] == 5318);
            Assert.IsTrue(lhs.elements[2] == 10514);
            Assert.IsTrue(lhs.elements[3] == 15894);
            Assert.IsTrue(lhs.elements[4] == 1655);
            Assert.IsTrue(lhs.elements[5] == 5562);
            Assert.IsTrue(lhs.elements[6] == 11006);
            Assert.IsTrue(lhs.elements[7] == 16634);
            Assert.IsTrue(lhs.elements[8] == 1787);
            Assert.IsTrue(lhs.elements[9] == 5980);
            Assert.IsTrue(lhs.elements[10] == 11840);
            Assert.IsTrue(lhs.elements[11] == 17888);
            Assert.IsTrue(lhs.elements[12] == 1861);
            Assert.IsTrue(lhs.elements[13] == 6246);
            Assert.IsTrue(lhs.elements[14] == 12378);
            Assert.IsTrue(lhs.elements[15] == 18710);
        }

        [TestMethod("Premultiply")]
        public void Test_Matrix3d_Premultiply()
        {
            TMatrix4 lhs = new TMatrix4(); lhs.Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);
            TMatrix4 rhs = new TMatrix4(); rhs.Set(59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131);

            rhs.PreMultiply(lhs);

            Assert.IsTrue(rhs.elements[0] == 1585);
            Assert.IsTrue(rhs.elements[1] == 5318);
            Assert.IsTrue(rhs.elements[2] == 10514);
            Assert.IsTrue(rhs.elements[3] == 15894);
            Assert.IsTrue(rhs.elements[4] == 1655);
            Assert.IsTrue(rhs.elements[5] == 5562);
            Assert.IsTrue(rhs.elements[6] == 11006);
            Assert.IsTrue(rhs.elements[7] == 16634);
            Assert.IsTrue(rhs.elements[8] == 1787);
            Assert.IsTrue(rhs.elements[9] == 5980);
            Assert.IsTrue(rhs.elements[10] == 11840);
            Assert.IsTrue(rhs.elements[11] == 17888);
            Assert.IsTrue(rhs.elements[12] == 1861);
            Assert.IsTrue(rhs.elements[13] == 6246);
            Assert.IsTrue(rhs.elements[14] == 12378);
            Assert.IsTrue(rhs.elements[15] == 18710);
        }

        [TestMethod("MultiplyMatrices")]
        public void Test_Matrix3d_MultiplyMatrices()
        {
            // Reference:
            //
            // #!/usr/bin/env python
            // from __future__ import print_function
            // import numpy as np
            // print(
            //     np.dot(
            //         np.reshape([2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53], (4, 4)),
            //         np.reshape([59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131], (4, 4))
            //     )
            // )
            //
            // [[ 1585  1655  1787  1861]
            //  [ 5318  5562  5980  6246]
            //  [10514 11006 11840 12378]
            //  [15894 16634 17888 18710]]
            TMatrix4 lhs = new TMatrix4(); lhs.Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);
            TMatrix4 rhs = new TMatrix4(); rhs.Set(59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131);
            TMatrix4 ans = TMatrix4.MultiplyMatrices(lhs, rhs);

            Assert.IsTrue(ans.elements[0] == 1585);
            Assert.IsTrue(ans.elements[1] == 5318);
            Assert.IsTrue(ans.elements[2] == 10514);
            Assert.IsTrue(ans.elements[3] == 15894);
            Assert.IsTrue(ans.elements[4] == 1655);
            Assert.IsTrue(ans.elements[5] == 5562);
            Assert.IsTrue(ans.elements[6] == 11006);
            Assert.IsTrue(ans.elements[7] == 16634);
            Assert.IsTrue(ans.elements[8] == 1787);
            Assert.IsTrue(ans.elements[9] == 5980);
            Assert.IsTrue(ans.elements[10] == 11840);
            Assert.IsTrue(ans.elements[11] == 17888);
            Assert.IsTrue(ans.elements[12] == 1861);
            Assert.IsTrue(ans.elements[13] == 6246);
            Assert.IsTrue(ans.elements[14] == 12378);
            Assert.IsTrue(ans.elements[15] == 18710);
        }

        [TestMethod("MultiplyScalar")]
        public void Test_Matrix3d_MultiplyScalar()
        {
            TMatrix4 b = new TMatrix4();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(b.elements[0] == 0);
            Assert.IsTrue(b.elements[1] == 4);
            Assert.IsTrue(b.elements[2] == 8);
            Assert.IsTrue(b.elements[3] == 12);
            Assert.IsTrue(b.elements[4] == 1);
            Assert.IsTrue(b.elements[5] == 5);
            Assert.IsTrue(b.elements[6] == 9);
            Assert.IsTrue(b.elements[7] == 13);
            Assert.IsTrue(b.elements[8] == 2);
            Assert.IsTrue(b.elements[9] == 6);
            Assert.IsTrue(b.elements[10] == 10);
            Assert.IsTrue(b.elements[11] == 14);
            Assert.IsTrue(b.elements[12] == 3);
            Assert.IsTrue(b.elements[13] == 7);
            Assert.IsTrue(b.elements[14] == 11);
            Assert.IsTrue(b.elements[15] == 15);

            b.MultiplyScalar(2);
            Assert.IsTrue(b.elements[0] == 0 * 2);
            Assert.IsTrue(b.elements[1] == 4 * 2);
            Assert.IsTrue(b.elements[2] == 8 * 2);
            Assert.IsTrue(b.elements[3] == 12 * 2);
            Assert.IsTrue(b.elements[4] == 1 * 2);
            Assert.IsTrue(b.elements[5] == 5 * 2);
            Assert.IsTrue(b.elements[6] == 9 * 2);
            Assert.IsTrue(b.elements[7] == 13 * 2);
            Assert.IsTrue(b.elements[8] == 2 * 2);
            Assert.IsTrue(b.elements[9] == 6 * 2);
            Assert.IsTrue(b.elements[10] == 10 * 2);
            Assert.IsTrue(b.elements[11] == 14 * 2);
            Assert.IsTrue(b.elements[12] == 3 * 2);
            Assert.IsTrue(b.elements[13] == 7 * 2);
            Assert.IsTrue(b.elements[14] == 11 * 2);
            Assert.IsTrue(b.elements[15] == 15 * 2);
        }

        [TestMethod("Determinant")]
        public void Test_Matrix3d_Determinant()
        {
            TMatrix4 a = TMatrix4.I;
            Assert.IsTrue(a.Determinant() == 1, "Passed!");

            var ae = a.elements;
            ae[0] = 2;
            a.SetElements(ae);
            Assert.IsTrue(a.Determinant() == 2, "Passed!");

            ae[0] = 0;
            a.SetElements(ae);
            Assert.IsTrue(a.Determinant() == 0, "Passed!");

            // calculated via http://www.euclideanspace.com/maths/algebra/matrix/functions/Determinant/fourD/index.htm
            a.Set(2, 3, 4, 5, -1, -21, -3, -4, 6, 7, 8, 10, -8, -9, -10, -12);
            Assert.IsTrue(a.Determinant() == 76, "Passed!");
        }

        [TestMethod("Transpose")]
        public void Test_Matrix3d_Transpose()
        {
            TMatrix4 a = TMatrix4.I;
            TMatrix4 b = a.Clone(); b.Transpose();
            Assert.IsTrue(a.Equals(b), "Passed!");

            b = new TMatrix4();
            b.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            TMatrix4 c = b.Clone(); c.Transpose();
            Assert.IsTrue(!b.Equals(c), "Passed!");

            c.Transpose();
            Assert.IsTrue(b.Equals(c), "Passed!");
        }

        [TestMethod("SetPosition")]
        public void Test_Matrix3d_SetPosition()
        {
            TMatrix4 a = new TMatrix4(); a.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            TPoint3D b = new TPoint3D(-1, -2, -3);

            TMatrix4 c = new TMatrix4(); c.Set(0, 1, 2, -1, 4, 5, 6, -2, 8, 9, 10, -3, 12, 13, 14, 15);

            a.SetPosition(b);
            Assert.IsTrue(a.Equals(c), "Passed!");

            TMatrix4 d = new TMatrix4(); d.Set(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            TMatrix4 e = new TMatrix4(); e.Set(0, 1, 2, -1, 4, 5, 6, -2, 8, 9, 10, -3, 12, 13, 14, 15);

            d.SetPosition(-1, -2, -3);
            Assert.IsTrue(d.Equals(e), "Passed!");
        }

        [TestMethod("Invert")]
        public void Test_Matrix3d_Invert()
        {
            TMatrix4 zero = new TMatrix4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            TMatrix4 identity = TMatrix4.I;

            TMatrix4 a = new TMatrix4();
            TMatrix4 b = new TMatrix4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            a.Copy(b); a.Invert();
            Assert.IsTrue(a.Equals(zero), "Passed!");


            TMatrix4[] testMatrices = new TMatrix4[] {
                TMatrix4.MakeRotationX(0.3),
                TMatrix4.MakeRotationX(-0.3),
                TMatrix4.MakeRotationY(0.3),
                TMatrix4.MakeRotationY(-0.3),
                TMatrix4.MakeRotationZ(0.3),
                TMatrix4.MakeRotationZ(-0.3),
                TMatrix4.MakeScale(1, 2, 3),
                TMatrix4.MakeScale(1 / 8.0, 1 / 2.0, 1 / 3.0),
                TMatrix4.MakePerspective(-1, 1, 1, -1, 1, 1000),
                TMatrix4.MakePerspective(-16, 16, 9, -9, 0.1, 10000),
                TMatrix4.MakeTranslation(1, 2, 3)
            };

            for (int i = 0, il = testMatrices.Length; i < il; i++)
            {

                TMatrix4 m = testMatrices[i];

                TMatrix4 mInverse = new TMatrix4();
                mInverse.Copy(m); mInverse.Invert();

                TMatrix4 mSelfInverse = m.Clone();
                mSelfInverse.Copy(mSelfInverse); mSelfInverse.Invert();

                // self-inverse should the same as inverse
                Assert.IsTrue(mSelfInverse.Equals(mInverse), "Passed!");

                // the determinant of the inverse should be the reciprocal
                Assert.IsTrue(Math.Abs(m.Determinant() * mInverse.Determinant() - 1) < 0.0001, "Passed!");

                TMatrix4 mProduct = TMatrix4.MultiplyMatrices(m, mInverse);

                // the determinant of the identity matrix is 1
                Assert.IsTrue(Math.Abs(mProduct.Determinant() - 1) < 0.0001, "Passed!");
                Assert.IsTrue(mProduct.Equals(identity), "Passed!");

            }

        }

        [TestMethod("Scale")]
        public void Test_Matrix3d_Scale()
        {
            TMatrix4 a = new TMatrix4(); a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            TVector3D b = new TVector3D(2, 3, 4);
            TMatrix4 c = new TMatrix4(); c.Set(2, 6, 12, 4, 10, 18, 28, 8, 18, 30, 44, 12, 26, 42, 60, 16);

            a.Scale(b);
            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("GetMaxScaleOnAxis")]
        public void Test_Matrix3d_GetMaxScaleOnAxis()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            double expected = Math.Sqrt(3 * 3 + 7 * 7 + 11 * 11);

            Assert.IsTrue(Math.Abs(a.GetMaxScaleOnAxis() - expected) <= double.Epsilon, "Check result");
        }

        [TestMethod("MakeTranslation")]
        public void Test_Matrix3d_MakeTranslation()
        {
            TMatrix4 a;
            TVector3D b = new TVector3D(2, 3, 4);
            TMatrix4 c = new TMatrix4();
            c.Set(1, 0, 0, 2, 0, 1, 0, 3, 0, 0, 1, 4, 0, 0, 0, 1);

            a = TMatrix4.MakeTranslation(b.X, b.Y, b.Z);
            Assert.IsTrue(a.Equals(c), "Passed!");

            a = TMatrix4.MakeTranslation(b);
            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("MakeRotationX")]
        public void Test_Matrix3d_MakeRotationX()
        {
            TMatrix4 a = new TMatrix4();
            double b = Math.Sqrt(3) / 2;
            TMatrix4 c = new TMatrix4();
            c.Set(1, 0, 0, 0, 0, b, -0.5, 0, 0, 0.5, b, 0, 0, 0, 0, 1);

            a = TMatrix4.MakeRotationX(Math.PI / 6);
            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("MakeRotationY")]
        public void Test_Matrix3d_MakeRotationY()
        {
            TMatrix4 a = new TMatrix4();
            double b = Math.Sqrt(3) / 2;
            TMatrix4 c = new TMatrix4();
            c.Set(b, 0, 0.5, 0, 0, 1, 0, 0, -0.5, 0, b, 0, 0, 0, 0, 1);

            a = TMatrix4.MakeRotationY(Math.PI / 6);
            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("MakeRotationZ")]
        public void Test_Matrix3d_MakeRotationZ()
        {
            TMatrix4 a = new TMatrix4();
            double b = Math.Sqrt(3) / 2;
            TMatrix4 c = new TMatrix4();
            c.Set(b, -0.5, 0, 0, 0.5, b, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            a = TMatrix4.MakeRotationZ(Math.PI / 6);
            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("MakeRotationAxis")]
        public void Test_Matrix3d_MakeRotationAxis()
        {
            TVector3D axis = new TVector3D(1.5, 0.0, 1.0);
            axis.Normalize();

            double radians = NumberUtil.DegreeToRadian(45);
            TMatrix4 a = TMatrix4.MakeRotationAxis(axis, radians);

            TMatrix4 expected = new TMatrix4(
                0.9098790095958609, -0.39223227027636803, 0.13518148560620882, 0,
                0.39223227027636803, 0.7071067811865476, -0.588348405414552, 0,
                0.13518148560620882, 0.588348405414552, 0.7972277715906868, 0,
                0, 0, 0, 1
            );

            Assert.IsTrue(a.Equals(expected), "Check numeric result");
        }

        [TestMethod("MakeScale")]
        public void Test_Matrix3d_MakeScale()
        {
            TMatrix4 a = TMatrix4.MakeScale(2, 3, 4);
            TMatrix4 c = new TMatrix4(2, 0, 0, 0, 0, 3, 0, 0, 0, 0, 4, 0, 0, 0, 0, 1);

            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("MakeShear")]
        public void Test_Matrix3d_MakeShear()
        {
            TMatrix4 a = TMatrix4.MakeShear(1, 2, 3, 4, 5, 6);
            TMatrix4 c = new TMatrix4(1, 3, 5, 0, 1, 1, 6, 0, 2, 4, 1, 0, 0, 0, 0, 1);

            Assert.IsTrue(a.Equals(c), "Passed!");
        }

        [TestMethod("Compose/Decompose")]
        public void Test_Matrix3d_Compose_Decompose()
        {
            TPoint3D[] tValues = new TPoint3D[]
            {
                    new TPoint3D(),
                    new TPoint3D(3, 0, 0),
                    new TPoint3D(0, 4, 0),
                    new TPoint3D(0, 0, 5),
                    new TPoint3D(-6, 0, 0),
                    new TPoint3D(0, -7, 0),
                    new TPoint3D(0, 0, -8),
                    new TPoint3D(-2, 5, -9),
                    new TPoint3D(-2, -5, -9)
            };

            TVector3D[] sValues = new TVector3D[] {
                    new TVector3D(1, 1, 1),
                    new TVector3D(2, 2, 2),
                    new TVector3D(1, -1, 1),
                    new TVector3D(-1, 1, 1),
                    new TVector3D(1, 1, -1),
                    new TVector3D(2, -2, 1),
                    new TVector3D(-1, 2, -2),
                    new TVector3D(-1, -1, -1),
                    new TVector3D(-2, -2, -2)
            };

            TQuaternion[] rValues = new TQuaternion[] {
                    new TQuaternion(),
                    TQuaternion.SetFromEuler(new TEuler(1, 1, 0)),
                    TQuaternion.SetFromEuler(new TEuler(1, -1, 1)),
                    new TQuaternion(0, 0.9238795292366128, 0, 0.38268342717215614)
            };

            for (int ti = 0; ti < tValues.Length; ti++)
            {

                for (int si = 0; si < sValues.Length; si++)
                {

                    for (int ri = 0; ri < rValues.Length; ri++)
                    {

                        var t = tValues[ti];
                        var s = sValues[si];
                        var r = rValues[ri];

                        var m = TMatrix4.Compose(t, r, s);
                        //var t2 = new TPoint3D();
                        //var r2 = new TQuaternion();
                        //var s2 = new TVector3D();

                        m.Decompose(out TPoint3D t2, out TQuaternion r2, out TVector3D s2);

                        var m2 = TMatrix4.Compose(t2, r2, s2);

                        /*
                        // debug code
                        const matrixIsSame = matrixEquals4( m, m2 );
                        if ( ! matrixIsSame ) {

                            console.log( t, s, r );
                            console.log( t2, s2, r2 );
                            console.log( m, m2 );

                        }
                        */

                        //Assert.IsTrue(matrixEquals4(m, m2), "Passed!");
                        Assert.IsTrue(m.Equals(m2), "Passed!");
                    }

                }

            }
        }

        [TestMethod("MakePerspective")]
        public void Test_Matrix3d_MakePerspective()
        {
            TMatrix4 a = TMatrix4.MakePerspective(-1, 1, -1, 1, 1, 100);
            TMatrix4 expected = new TMatrix4();
            expected.Set(
                1, 0, 0, 0,
                0, -1, 0, 0,
                0, 0, -101.0 / 99.0, -200.0 / 99.0,
                0, 0, -1, 0
            );
            Assert.IsTrue(a.Equals(expected), "Check result");
        }

        [TestMethod("MakeOrthographic")]
        public void Test_Matrix3d_MakeOrthographic()
        {
            TMatrix4 a = TMatrix4.MakeOrthographic(-1, 1, -1, 1, 1, 100);
            TMatrix4 expected = new TMatrix4();
            expected.Set(
                1, 0, 0, 0,
                0, -1, 0, 0,
                0, 0, -2.0 / 99.0, -101.0 / 99.0,
                0, 0, 0, 1
            );

            Assert.IsTrue(a.Equals(expected), "Check result");
        }

        [TestMethod("Equals")]
        public void Test_Matrix3d_Equals()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            TMatrix4 b = new TMatrix4();
            b.Set(-1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsFalse(a.Equals(b), "Check that a does not equal b");
            Assert.IsFalse(b.Equals(a), "Check that b does not equal a");

            a.Copy(b);
            Assert.IsTrue(a.Equals(b), "Check that a equals b after copy()");
            Assert.IsTrue(b.Equals(a), "Check that b equals a after copy()");
        }

        [TestMethod("FromArray")]
        public void Test_Matrix3d_FromArray()
        {
            TMatrix4 a = new TMatrix4();
            TMatrix4 b = new TMatrix4();
            b.Set(1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16);

            a.FromArray(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            Assert.IsTrue(a.Equals(b), "Passed");
        }

        [TestMethod("ToArray")]
        public void Test_Matrix3d_ToArray()
        {
            TMatrix4 a = new TMatrix4();
            a.Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            double[] noOffset = new double[] { 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16 };
            double[] withOffset = new double[] { double.NaN, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16 };

            double[] array = a.ToArray();
            Assert.IsTrue(array.SequenceEqual(noOffset), "No array, no offset");

            array = a.ToArray();
            Assert.IsTrue(array.SequenceEqual(noOffset), "With array, no offset");

            array = a.ToArray(1);
            Assert.IsTrue(array.SequenceEqual(withOffset), "With array, with offset");
        }

        [TestMethod("World To Plane")]
        public void Test_Matrix3D_WorldToPlane()
        {
            TPlane3D plane_1 = new TPlane3D(new TPoint3D(0, 0, 100), new TPoint3D(100, 0, 100), new TPoint3D(100, 100, 100)); // 高度为 100 的平面
            TMatrix4 plane_matrix = TMatrix4.WorldToPlane(plane_1);
            TMatrix4 ans_matrix = new TMatrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 100, 0, 0, 0, 1);
            Assert.IsTrue(plane_matrix.Equals(ans_matrix));

            TPoint3D plane_origin_in_world = new TPoint3D(0, 0, 0);
            plane_origin_in_world.ApplyMatrix4(plane_matrix);
            Assert.IsTrue(plane_origin_in_world.IsEqualTo(new TPoint3D(0, 0, 100)));
        }
        #endregion
    }

    [TestClass]
    public class Matrix_Demo
    {
        [TestMethod]
        public void MatrixElements()
        {
            TMatrix4 matrix = TMatrix4.MakeTranslation(new TVector3D(1, 10, 9));
            var element = matrix.elements;
            Assert.IsTrue(element[3] == 1);
            Assert.IsTrue(element[7] == 10);
            Assert.IsTrue(element[11] == 9);
        }
    }
}
