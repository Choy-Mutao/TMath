using System;

namespace tmath
{

    [Obsolete("临时存放位置")]
    public struct UVParams
    {
        public double centerX;
        public double centerY;
        public double offsetX;
        public double offsetY;
        public double repeatX;
        public double repeatY;
        public double rotation;
    }

    public struct TMatrix3 : IEquatable<TMatrix3>
    {
        #region Fields
        /// <summary>
        /// 按 列 插入
        /// | 0 | 3 | 6 |
        /// | 1 | 4 | 7 |
        /// | 2 | 5 | 8 |
        /// </summary>
        private double[] _elements;

        /// <summary>
        /// 按 行 读取
        /// </summary>
        public double[] row_elements
        {
            get
            {
                return new double[9] {  _elements[0], _elements[3], _elements[6],
                                        _elements[1], _elements[4], _elements[7],
                                        _elements[2], _elements[5], _elements[8]};
            }
        }

        /// <summary>
        /// 按 列 读取
        /// </summary>
        public double[] elements
        {
            get
            {
                return new double[9]
                {
                    _elements[0],_elements[1],_elements[2],
                    _elements[3],_elements[4],_elements[5],
                    _elements[6], _elements[7],_elements[8]
                };
            }
        }
        public static TMatrix3 I { get; } = new TMatrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public TVector2D Translation => new TVector2D(_elements[6], _elements[7]);

        #endregion

        #region Constructor
        public TMatrix3(int _ = 1)
        {
            _elements = new double[9] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
        }
        public TMatrix3(double n11, double n12, double n13, double n21, double n22, double n23, double n31, double n32, double n33)
        {
            _elements = new double[9];

            _elements[0] = n11; _elements[1] = n21; _elements[2] = n31;
            _elements[3] = n12; _elements[4] = n22; _elements[5] = n32;
            _elements[6] = n13; _elements[7] = n23; _elements[8] = n33;
        }
        #endregion

        #region Static Methods
        public static TMatrix3 MultiplyMatrices(TMatrix3 a, TMatrix3 b)
        {
            double[] ae = a.elements;
            double[] be = b.elements;
            //const te = this.elements;
            double[] te = new double[9];

            double a11 = ae[0], a12 = ae[3], a13 = ae[6];
            double a21 = ae[1], a22 = ae[4], a23 = ae[7];
            double a31 = ae[2], a32 = ae[5], a33 = ae[8];

            double b11 = be[0], b12 = be[3], b13 = be[6];
            double b21 = be[1], b22 = be[4], b23 = be[7];
            double b31 = be[2], b32 = be[5], b33 = be[8];

            te[0] = a11 * b11 + a12 * b21 + a13 * b31;
            te[3] = a11 * b12 + a12 * b22 + a13 * b32;
            te[6] = a11 * b13 + a12 * b23 + a13 * b33;

            te[1] = a21 * b11 + a22 * b21 + a23 * b31;
            te[4] = a21 * b12 + a22 * b22 + a23 * b32;
            te[7] = a21 * b13 + a22 * b23 + a23 * b33;

            te[2] = a31 * b11 + a32 * b21 + a33 * b31;
            te[5] = a31 * b12 + a32 * b22 + a33 * b32;
            te[8] = a31 * b13 + a32 * b23 + a33 * b33;

            return new TMatrix3(te[0], te[3], te[6], te[1], te[4], te[7], te[2], te[5], te[8]);
        }

        public static TMatrix3 MakeScale(double x, double y)
        {
            return new TMatrix3(x, 0, 0,
                                    0, y, 0,
                                    0, 0, 1);
        }

        public static TMatrix3 MakeTranslation(double x, double y)
        {
            return new TMatrix3(1, 0, x,
                                0, 1, y,
                                0, 0, 1);
        }

        public static TMatrix3 MakeTranslation(TVector2D tran)
        {
            return new TMatrix3(1, 0, tran.X,
                                0, 1, tran.Y,
                                0, 0, 1);
        }

        /// <summary>
        /// 以弧度制旋转
        /// </summary>
        /// <param name="theta">弧度值</param>
        /// <returns></returns>
        public static TMatrix3 MakeRotation(double theta)
        {
            double c = Math.Cos(theta);
            double s = Math.Sin(theta);
            return new TMatrix3(c, -s, 0,
                                s, c, 0,
                                0, 0, 1);
        }
        #endregion

        #region Methods
        public void Copy(TMatrix3 m)
        {
            SetByRowElements(m.row_elements);
        }

        public void ExtractBasis(out TVector3D xAxis, out TVector3D yAxis, out TVector3D zAxis)
        {
            xAxis = new TVector3D(); yAxis = new TVector3D(); zAxis = new TVector3D();
            xAxis = TVector3D.SetFromMatrix3Column(this, 0);
            yAxis = TVector3D.SetFromMatrix3Column(this, 1);
            zAxis = TVector3D.SetFromMatrix3Column(this, 2);
        }

        public void SetFromMatrix4(TMatrix4 m)
        {
            var me = m.elements;
            Set(
            me[0], me[4], me[8],
            me[1], me[5], me[9],
            me[2], me[6], me[10]);
        }

        public void Set(double n11, double n12, double n13, double n21, double n22, double n23, double n31, double n32, double n33)
        {
            _elements = new double[9];

            _elements[0] = n11; _elements[1] = n21; _elements[2] = n31;
            _elements[3] = n12; _elements[4] = n22; _elements[5] = n32;
            _elements[6] = n13; _elements[7] = n23; _elements[8] = n33;
        }

        public void SetByRowElements(in double[] input)
        {
            if (input.Length != 9) throw new ArgumentException("TMatrix3 SetByRowElements Method needs 9 length double array input");
            _elements = new double[9];
            _elements[0] = input[0]; _elements[1] = input[3]; _elements[2] = input[6];
            _elements[3] = input[1]; _elements[4] = input[4]; _elements[5] = input[7];
            _elements[6] = input[2]; _elements[7] = input[5]; _elements[8] = input[8];
        }

        public void SetElements(in double[] input)
        {
            if (input.Length != 9) throw new ArgumentException("TMatrix3 SetByRowElements Method needs 9 length double array input");
            _elements = new double[9];
            _elements[0] = input[0]; _elements[1] = input[1]; _elements[2] = input[2];
            _elements[3] = input[3]; _elements[4] = input[4]; _elements[5] = input[5];
            _elements[6] = input[6]; _elements[7] = input[7]; _elements[8] = input[8];
        }

        public void Identity()
        {
            Set(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);
        }

        public TMatrix3 MultiplyScalar(double s)
        {
            for (int i = 0; i < _elements.Length; i++) _elements[i] *= s;
            return this;
        }

        public double Determinant()
        {
            double[] te = row_elements;

            double a = te[0], b = te[1], c = te[2],
                    d = te[3], e = te[4], f = te[5],
                    g = te[6], h = te[7], i = te[8];

            return a * e * i - a * f * h - b * d * i + b * f * g + c * d * h - c * e * g;
        }

        public TMatrix3 Invert()
        {

            var te = _elements;

            double n11 = te[0], n21 = te[1], n31 = te[2],
                     n12 = te[3], n22 = te[4], n32 = te[5],
                     n13 = te[6], n23 = te[7], n33 = te[8],

                     t11 = n33 * n22 - n32 * n23,
                     t12 = n32 * n13 - n33 * n12,
                     t13 = n23 * n12 - n22 * n13,

                    det = n11 * t11 + n21 * t12 + n31 * t13;

            if (det == 0) return new TMatrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);

            double detInv = 1 / det;

            te[0] = t11 * detInv;
            te[1] = (n31 * n23 - n33 * n21) * detInv;
            te[2] = (n32 * n21 - n31 * n22) * detInv;

            te[3] = t12 * detInv;
            te[4] = (n33 * n11 - n31 * n13) * detInv;
            te[5] = (n31 * n12 - n32 * n11) * detInv;

            te[6] = t13 * detInv;
            te[7] = (n21 * n13 - n23 * n11) * detInv;
            te[8] = (n22 * n11 - n21 * n12) * detInv;

            return this;
        }

        // 返回, 但不修改自己
        public TMatrix3 GetInvert()
        {
            var te = _elements;

            double n11 = te[0], n21 = te[1], n31 = te[2],
                    n12 = te[3], n22 = te[4], n32 = te[5],
                    n13 = te[6], n23 = te[7], n33 = te[8],

                    t11 = n33 * n22 - n32 * n23,
                    t12 = n32 * n13 - n33 * n12,
                    t13 = n23 * n12 - n22 * n13,

                    det = n11 * t11 + n21 * t12 + n31 * t13;

            if (det == 0) return new TMatrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);

            double detInv = 1 / det;

            var ne = new double[9];

            ne[0] = t11 * detInv;
            ne[1] = (n31 * n23 - n33 * n21) * detInv;
            ne[2] = (n32 * n21 - n31 * n22) * detInv;

            ne[3] = t12 * detInv;
            ne[4] = (n33 * n11 - n31 * n13) * detInv;
            ne[5] = (n31 * n12 - n32 * n11) * detInv;

            ne[6] = t13 * detInv;
            ne[7] = (n21 * n13 - n23 * n11) * detInv;
            ne[8] = (n22 * n11 - n21 * n12) * detInv;

            return new TMatrix3(ne[0], ne[3], ne[6], ne[1], ne[4], ne[7], ne[2], ne[5], ne[8]);
        }

        public TMatrix3 Transpose()
        {
            double tmp;
            var m = _elements;

            tmp = m[1]; m[1] = m[3]; m[3] = tmp;
            tmp = m[2]; m[2] = m[6]; m[6] = tmp;
            tmp = m[5]; m[5] = m[7]; m[7] = tmp;

            return this;
        }

        public TMatrix3 GetNormalMatrix(TMatrix4 matrix3)
        {
            SetFromMatrix4(matrix3);
            Invert();
            Transpose();
            return this;
        }

        // 转置为 array
        public double[] TransposeIntoArray()
        {
            double[] r = new double[9];
            var m = elements;
            r[0] = m[0];
            r[1] = m[3];
            r[2] = m[6];
            r[3] = m[1];
            r[4] = m[4];
            r[5] = m[7];
            r[6] = m[2];
            r[7] = m[5];
            r[8] = m[8];
            return r;
        }

        public TMatrix3 SetUVTransform(double tx, double ty, double sx, double sy, double rotation, double cx, double cy)
        {
            double c = Math.Cos(rotation);
            double s = Math.Sin(rotation);

            this.Set(
                sx * c, sx * s, -sx * (c * cx + s * cy) + cx + tx,
                -sy * s, sy * c, -sy * (-s * cx + c * cy) + cy + ty,
                0, 0, 1
            );

            return this;
        }

        public TMatrix3 PostMultiply(TMatrix3 m)
        {
            SetElements(MultiplyMatrices(this, m).elements);
            return this;
        }

        public TMatrix3 PreMultiply(TMatrix3 m)
        {
            SetElements(MultiplyMatrices(m, this).elements);
            return this;
        }

        public TMatrix3 Translate(double tx, double ty)
        {
            return PreMultiply(MakeTranslation(tx, ty));
        }

        public TMatrix3 Scale(double sx, double sy)
        {
            return PreMultiply(MakeScale(sx, sy));
        }

        [Obsolete("改变了自己的值")]
        public TMatrix3 Rotate(double theta)
        {
            return PreMultiply(MakeRotation(-theta));
        }

        public TMatrix3 FromArray(double[] array, int offset = 0)
        {
            _elements = new double[array.Length + offset];
            for (int i = 0; i < 9; i++) _elements[i] = array[i + offset];
            return this;
        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[9 + offset];
            double[] te = elements;

            for (int i = 0; i < offset; i++) array[i] = double.NaN;

            array[offset] = te[0];
            array[offset + 1] = te[1];
            array[offset + 2] = te[2];

            array[offset + 3] = te[3];
            array[offset + 4] = te[4];
            array[offset + 5] = te[5];

            array[offset + 6] = te[6];
            array[offset + 7] = te[7];
            array[offset + 8] = te[8];

            return array;
        }

        public TMatrix3 Clone() => new TMatrix3(row_elements[0], row_elements[1], row_elements[2], row_elements[3], row_elements[4], row_elements[5], row_elements[6], row_elements[7], row_elements[8]);

        public bool Equals(TMatrix3 other, double tolerance = 0.0001)
        {
            if (row_elements.Length != other.row_elements.Length)
            {

                return false;

            }

            for (int i = 0, il = row_elements.Length; i < il; i++)
            {

                double delta = row_elements[i] - other.row_elements[i];
                if (Math.Abs(delta) > tolerance)
                {
                    return false;
                }

            }

            return true;
        }

        public bool Equals(TMatrix3 other) => Equals(other, 0.0001);

        public TMatrix4 ToMatrix4()
        {
            TMatrix4 result = TMatrix4.I;
            double[] re = result.elements;
            double[] me = this.elements;
            re[0] = me[0];
            re[1] = me[1];
            re[2] = me[2];
            re[4] = me[3];
            re[5] = me[4];
            re[6] = me[5];
            re[8] = me[6];
            re[9] = me[7];
            re[10] = me[8];
            result.SetElements(re);
            return result;
        }
        #endregion
        public override string ToString()
        {
            string str = "TMatrix3:";
            for (int i = 0; i < 9; ++i)
            {
                if (i % 3 == 0) { str += "\n "; }
                str += (elements[i] + " ");
            }
            return str;
        }
    }
}

