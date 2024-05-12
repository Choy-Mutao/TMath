using System;
using System.Linq;
using tmath.geometry;

namespace tmath
{
    /// <summary>
    /// 标准列向量矩阵
    /// </summary>
    public struct TMatrix4 : IEquatable<TMatrix4>
    {
        #region Fields
        /// <summary>
        /// 按 列 插入
        /// | 00 | 04 | 08 | 12 |
        /// | 01 | 05 | 09 | 13 |
        /// | 02 | 06 | 10 | 14 |
        /// | 03 | 07 | 11 | 15 |
        /// </summary>
        private double[] _elements;

        public double[] elements
        {
            get
            {
                return new double[16]
                {
                    _elements[0],_elements[1],_elements[2],_elements[3],
                    _elements[4],_elements[5],_elements[6],_elements[7],
                    _elements[8],_elements[9],_elements[10],_elements[11],
                    _elements[12],_elements[13],_elements[14],_elements[15],
                };
            }
        }

        public static TMatrix4 I = new TMatrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        public readonly static TMatrix4 WCS = new TMatrix4().MakeBasis(TVector3D.XAxis, TVector3D.YAxis, TVector3D.ZAxis);
        #endregion

        #region Constructor
        public TMatrix4(double n11, double n12, double n13, double n14,
                        double n21, double n22, double n23, double n24,
                        double n31, double n32, double n33, double n34,
                        double n41, double n42, double n43, double n44)
        {
            _elements = new double[16];
            _elements[0] = n11; _elements[4] = n12; _elements[8] = n13; _elements[12] = n14;
            _elements[1] = n21; _elements[5] = n22; _elements[9] = n23; _elements[13] = n24;
            _elements[2] = n31; _elements[6] = n32; _elements[10] = n33; _elements[14] = n34;
            _elements[3] = n41; _elements[7] = n42; _elements[11] = n43; _elements[15] = n44;
        }

        #endregion

        #region Static Methods
        public static TMatrix4 MultiplyMatrices(TMatrix4 a, TMatrix4 b)
        {

            double[] ae = a.elements;
            double[] be = b.elements;


            double a11 = ae[0], a12 = ae[4], a13 = ae[8], a14 = ae[12];
            double a21 = ae[1], a22 = ae[5], a23 = ae[9], a24 = ae[13];
            double a31 = ae[2], a32 = ae[6], a33 = ae[10], a34 = ae[14];
            double a41 = ae[3], a42 = ae[7], a43 = ae[11], a44 = ae[15];

            double b11 = be[0], b12 = be[4], b13 = be[8], b14 = be[12];
            double b21 = be[1], b22 = be[5], b23 = be[9], b24 = be[13];
            double b31 = be[2], b32 = be[6], b33 = be[10], b34 = be[14];
            double b41 = be[3], b42 = be[7], b43 = be[11], b44 = be[15];

            double[] te = new double[16];
            te[0] = a11 * b11 + a12 * b21 + a13 * b31 + a14 * b41;
            te[4] = a11 * b12 + a12 * b22 + a13 * b32 + a14 * b42;
            te[8] = a11 * b13 + a12 * b23 + a13 * b33 + a14 * b43;
            te[12] = a11 * b14 + a12 * b24 + a13 * b34 + a14 * b44;

            te[1] = a21 * b11 + a22 * b21 + a23 * b31 + a24 * b41;
            te[5] = a21 * b12 + a22 * b22 + a23 * b32 + a24 * b42;
            te[9] = a21 * b13 + a22 * b23 + a23 * b33 + a24 * b43;
            te[13] = a21 * b14 + a22 * b24 + a23 * b34 + a24 * b44;

            te[2] = a31 * b11 + a32 * b21 + a33 * b31 + a34 * b41;
            te[6] = a31 * b12 + a32 * b22 + a33 * b32 + a34 * b42;
            te[10] = a31 * b13 + a32 * b23 + a33 * b33 + a34 * b43;
            te[14] = a31 * b14 + a32 * b24 + a33 * b34 + a34 * b44;

            te[3] = a41 * b11 + a42 * b21 + a43 * b31 + a44 * b41;
            te[7] = a41 * b12 + a42 * b22 + a43 * b32 + a44 * b42;
            te[11] = a41 * b13 + a42 * b23 + a43 * b33 + a44 * b43;
            te[15] = a41 * b14 + a42 * b24 + a43 * b34 + a44 * b44;

            return new TMatrix4().FromArray(te);
        }

        public static TMatrix4 MakeTranslation(TVector3D vector)
        {
            return new TMatrix4(1, 0, 0, vector.X,
                                    0, 1, 0, vector.Y,
                                    0, 0, 1, vector.Z,
                                    0, 0, 0, 1);
        }

        public static TMatrix4 MakeTranslation(double x, double y, double z)
        {
            return new TMatrix4(1, 0, 0, x,
                                    0, 1, 0, y,
                                    0, 0, 1, z,
                                    0, 0, 0, 1);
        }

        // 从世界坐标系, 到 平面坐标系(局部)的转换矩阵
        public static TMatrix4 WorldToPlane(TPlane3D m_plane)
        {
            TVector3D planeXAxis = TVector3D.CrossProduct(TVector3D.ZAxis, m_plane.Normal);
            if (planeXAxis.Length() == 0) planeXAxis = TVector3D.XAxis;
            TVector3D planeYAxis = TVector3D.CrossProduct(m_plane.Normal, planeXAxis);
            TMatrix4 matrix = new TMatrix4().MakeBasis(planeXAxis, planeYAxis, m_plane.Normal);
            matrix.Translate(new TVector3D(m_plane.Origin.ToArray()));
            return matrix;
        }

        // 从平面坐标系(局部), 到 世界坐标系的转换矩阵
        public static TMatrix4 PlaneToWorld(TPlane3D m_plane)
        {
            throw new NotImplementedException();
        }

        public static TMatrix4 MakeRotationX(double theta)
        {

            double c = Math.Cos(theta), s = Math.Sin(theta);

            return new TMatrix4(
                1, 0, 0, 0,
                0, c, -s, 0,
                0, s, c, 0,
                0, 0, 0, 1);

        }

        public static TMatrix4 MakeRotationY(double theta)
        {
            double c = Math.Cos(theta), s = Math.Sin(theta);
            return new TMatrix4(
                 c, 0, s, 0,
                 0, 1, 0, 0,
                -s, 0, c, 0,
                 0, 0, 0, 1);
        }

        public static TMatrix4 MakeRotationZ(double theta)
        {

            double c = Math.Cos(theta), s = Math.Sin(theta);

            return new TMatrix4(
                c, -s, 0, 0,
                s, c, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

        }
        public static TMatrix4 MakeRotationAxis(TVector3D axis, double angle)
        {

            // Based on http://www.gamedev.net/reference/articles/article1199.asp

            double c = Math.Cos(angle);
            double s = Math.Sin(angle);
            double t = 1 - c;
            double x = axis.X, y = axis.Y, z = axis.Z;
            double tx = t * x, ty = t * y;

            return new TMatrix4(
                tx * x + c, tx * y - s * z, tx * z + s * y, 0,
                tx * y + s * z, ty * y + c, ty * z - s * x, 0,
                tx * z - s * y, ty * z + s * x, t * z * z + c, 0,
                0, 0, 0, 1
                );
        }

        public static TMatrix4 MakeScale(double x, double y, double z)
        {
            return new TMatrix4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        public static TMatrix4 MakeShear(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return new TMatrix4(
                1, yx, zx, 0,
                xy, 1, zy, 0,
                xz, yz, 1, 0,
                0, 0, 0, 1);
        }

        public static TMatrix4 MakeRotationFromEuler(TEuler euler)
        {
            TMatrix4 result = TMatrix4.I;
            double[] te = result.elements;

            double x = euler.X, y = euler.Y, z = euler.Z;
            double a = Math.Cos(x), b = Math.Sin(x);
            double c = Math.Cos(y), d = Math.Sin(y);
            double e = Math.Cos(z), f = Math.Sin(z);

            if (euler.Order == "XYZ")
            {

                double ae = a * e, af = a * f, be = b * e, bf = b * f;

                te[0] = c * e;
                te[4] = -c * f;
                te[8] = d;

                te[1] = af + be * d;
                te[5] = ae - bf * d;
                te[9] = -b * c;

                te[2] = bf - ae * d;
                te[6] = be + af * d;
                te[10] = a * c;

            }
            else if (euler.Order == "YXZ")
            {

                double ce = c * e, cf = c * f, de = d * e, df = d * f;

                te[0] = ce + df * b;
                te[4] = de * b - cf;
                te[8] = a * d;

                te[1] = a * f;
                te[5] = a * e;
                te[9] = -b;

                te[2] = cf * b - de;
                te[6] = df + ce * b;
                te[10] = a * c;

            }
            else if (euler.Order == "ZXY")
            {

                double ce = c * e, cf = c * f, de = d * e, df = d * f;

                te[0] = ce - df * b;
                te[4] = -a * f;
                te[8] = de + cf * b;

                te[1] = cf + de * b;
                te[5] = a * e;
                te[9] = df - ce * b;

                te[2] = -a * d;
                te[6] = b;
                te[10] = a * c;

            }
            else if (euler.Order == "ZYX")
            {

                double ae = a * e, af = a * f, be = b * e, bf = b * f;

                te[0] = c * e;
                te[4] = be * d - af;
                te[8] = ae * d + bf;

                te[1] = c * f;
                te[5] = bf * d + ae;
                te[9] = af * d - be;

                te[2] = -d;
                te[6] = b * c;
                te[10] = a * c;

            }
            else if (euler.Order == "YZX")
            {

                double ac = a * c, ad = a * d, bc = b * c, bd = b * d;

                te[0] = c * e;
                te[4] = bd - ac * f;
                te[8] = bc * f + ad;

                te[1] = f;
                te[5] = a * e;
                te[9] = -b * e;

                te[2] = -d * e;
                te[6] = ad * f + bc;
                te[10] = ac - bd * f;

            }
            else if (euler.Order == "XZY")
            {

                double ac = a * c, ad = a * d, bc = b * c, bd = b * d;

                te[0] = c * e;
                te[4] = -f;
                te[8] = d * e;

                te[1] = ac * f + bd;
                te[5] = a * e;
                te[9] = ad * f - bc;

                te[2] = bc * f - ad;
                te[6] = b * e;
                te[10] = bd * f + ac;

            }

            // bottom row
            te[3] = 0;
            te[7] = 0;
            te[11] = 0;

            // last column
            te[12] = 0;
            te[13] = 0;
            te[14] = 0;
            te[15] = 1;

            result.SetElements(te);
            return result;
        }

        public static TMatrix4 ExtractRotation(TMatrix4 m)
        {
            TMatrix4 result = TMatrix4.I;

            // this method does not support reflection matrices

            double[] te = result.elements;
            double[] me = m.elements;

            double scaleX = 1 / TVector3D.SetFromMatrix4Column(m, 0).Length();
            double scaleY = 1 / TVector3D.SetFromMatrix4Column(m, 1).Length();
            double scaleZ = 1 / TVector3D.SetFromMatrix4Column(m, 2).Length();

            te[0] = me[0] * scaleX;
            te[1] = me[1] * scaleX;
            te[2] = me[2] * scaleX;
            te[3] = 0;

            te[4] = me[4] * scaleY;
            te[5] = me[5] * scaleY;
            te[6] = me[6] * scaleY;
            te[7] = 0;

            te[8] = me[8] * scaleZ;
            te[9] = me[9] * scaleZ;
            te[10] = me[10] * scaleZ;
            te[11] = 0;

            te[12] = 0;
            te[13] = 0;
            te[14] = 0;
            te[15] = 1;
            result.SetElements(te);
            return result;
        }

        public static TMatrix4 MakeRotationFromQuaternion(TQuaternion q)
        {
            return Compose(new TPoint3D(0, 0, 0), q, new TVector3D(1, 1, 1));
        }

        public static TMatrix4 LookAt(TVector3D eye, TVector3D target, TVector3D up)
        {
            var _z = TVector3D.SubVectors(eye, target);

            if (_z.LengthSq() == 0)
            {
                // eye and target are in the same position
                _z.Z = 1;
            }

            _z.Normalize();
            var _x = TVector3D.CrossProduct(up, _z);

            if (_x.LengthSq() == 0)
            {

                // up and z are parallel

                if (Math.Abs(up.Z) == 1)
                {

                    _z.X += 0.0001;

                }
                else
                {

                    _z.Z += 0.0001;

                }

                _z.Normalize();
                _x = TVector3D.CrossProduct(up, _z);

            }

            _x.Normalize();
            var _y = TVector3D.CrossProduct(_z, _x);

            TMatrix4 result = TMatrix4.I;
            double[] te = result.elements;
            te[0] = _x.X; te[4] = _y.X; te[8] = _z.X;
            te[1] = _x.Y; te[5] = _y.Y; te[9] = _z.Y;
            te[2] = _x.Z; te[6] = _y.Z; te[10] = _z.Z;
            result.SetElements(te);
            return result;
        }
        public static TMatrix4 MakePerspective(double left, double right, double top, double bottom, double near, double far, MathEnum coordinateSystem = MathEnum.WebGLCoordinateSystem)
        {

            TMatrix4 result = I;
            double[] te = result.elements;
            double x = 2 * near / (right - left);
            double y = 2 * near / (top - bottom);

            double a = (right + left) / (right - left);
            double b = (top + bottom) / (top - bottom);

            double c, d;

            if (coordinateSystem == MathEnum.WebGLCoordinateSystem)
            {

                c = -(far + near) / (far - near);
                d = (-2 * far * near) / (far - near);

            }
            else if (coordinateSystem == MathEnum.WebGPUCoordinateSystem)
            {

                c = -far / (far - near);
                d = (-far * near) / (far - near);

            }
            else
            {

                throw new Exception("THREE.Matrix4.makePerspective(): Invalid coordinate system: " + coordinateSystem);

            }

            te[0] = x; te[4] = 0; te[8] = a; te[12] = 0;
            te[1] = 0; te[5] = y; te[9] = b; te[13] = 0;
            te[2] = 0; te[6] = 0; te[10] = c; te[14] = d;
            te[3] = 0; te[7] = 0; te[11] = -1; te[15] = 0;

            result.SetElements(te);
            return result;
        }

        public static TMatrix4 MakeOrthographic(double left, double right, double top, double bottom, double near, double far, MathEnum coordinateSystem = MathEnum.WebGLCoordinateSystem)
        {
            TMatrix4 result = I;
            double[] te = result.elements;
            double w = 1.0 / (right - left);
            double h = 1.0 / (top - bottom);
            double p = 1.0 / (far - near);

            double x = (right + left) * w;
            double y = (top + bottom) * h;

            double z, zInv;

            if (coordinateSystem == MathEnum.WebGLCoordinateSystem)
            {

                z = (far + near) * p;
                zInv = -2 * p;

            }
            else if (coordinateSystem == MathEnum.WebGPUCoordinateSystem)
            {

                z = near * p;
                zInv = -1 * p;

            }
            else
            {

                throw new Exception("THREE.Matrix4.makeOrthographic(): Invalid coordinate system: " + coordinateSystem);

            }

            te[0] = 2 * w; te[4] = 0; te[8] = 0; te[12] = -x;
            te[1] = 0; te[5] = 2 * h; te[9] = 0; te[13] = -y;
            te[2] = 0; te[6] = 0; te[10] = zInv; te[14] = -z;
            te[3] = 0; te[7] = 0; te[11] = 0; te[15] = 1;

            result.SetElements(te);
            return result;

        }

        #endregion

        #region Public Methods

        public void Identity()
        {
            Set(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// 将按 行 输入的矩阵元素, 以列方式存储在 array 中
        /// </summary>
        /// <param name="n11"></param>
        /// <param name="n12"></param>
        /// <param name="n13"></param>
        /// <param name="n14"></param>
        /// <param name="n21"></param>
        /// <param name="n22"></param>
        /// <param name="n23"></param>
        /// <param name="n24"></param>
        /// <param name="n31"></param>
        /// <param name="n32"></param>
        /// <param name="n33"></param>
        /// <param name="n34"></param>
        /// <param name="n41"></param>
        /// <param name="n42"></param>
        /// <param name="n43"></param>
        /// <param name="n44"></param>
        public void Set(double n11, double n12, double n13, double n14, double n21, double n22, double n23, double n24, double n31, double n32, double n33, double n34, double n41, double n42, double n43, double n44)
        {
            _elements = new double[16];

            _elements[0] = n11; _elements[4] = n12; _elements[8] = n13; _elements[12] = n14;
            _elements[1] = n21; _elements[5] = n22; _elements[9] = n23; _elements[13] = n24;
            _elements[2] = n31; _elements[6] = n32; _elements[10] = n33; _elements[14] = n34;
            _elements[3] = n41; _elements[7] = n42; _elements[11] = n43; _elements[15] = n44;
        }

        public TMatrix4 SetElements(double[] eles)
        {
            _elements = new double[16];
            _elements[0] = eles[0];
            _elements[1] = eles[1];
            _elements[2] = eles[2];
            _elements[3] = eles[3];
            _elements[4] = eles[4];
            _elements[5] = eles[5];
            _elements[6] = eles[6];
            _elements[7] = eles[7];
            _elements[8] = eles[8];
            _elements[9] = eles[9];
            _elements[10] = eles[10];
            _elements[11] = eles[11];
            _elements[12] = eles[12];
            _elements[13] = eles[13];
            _elements[14] = eles[14];
            _elements[15] = eles[15];
            return this;
        }

        public TMatrix4 Clone()
        {
            return new TMatrix4().FromArray(_elements);
        }

        public TMatrix4 Copy(TMatrix4 m)
        {
            if (_elements == null) _elements = new double[16];
            var te = _elements;
            var me = m._elements;
            if (me == null) throw new NullReferenceException("Bad TMatrix4!");
            te[0] = me[0]; te[1] = me[1]; te[2] = me[2]; te[3] = me[3];
            te[4] = me[4]; te[5] = me[5]; te[6] = me[6]; te[7] = me[7];
            te[8] = me[8]; te[9] = me[9]; te[10] = me[10]; te[11] = me[11];
            te[12] = me[12]; te[13] = me[13]; te[14] = me[14]; te[15] = me[15];

            return this;
        }

        public TMatrix4 CopyPosition(TMatrix4 m)
        {
            var te = _elements;
            var me = m._elements;

            te[12] = me[12];
            te[13] = me[13];
            te[14] = me[14];

            return this;
        }

        public TMatrix4 SetFromMatrix3(TMatrix3 m)
        {
            var me = m.elements;
            Set(

                me[0], me[3], me[6], 0,
                me[1], me[4], me[7], 0,
                me[2], me[5], me[8], 0,
                0, 0, 0, 1

            );
            return this;
        }

        public void ExtractBasis(out TVector3D xAxis, out TVector3D yAxis, out TVector3D zAxis)
        {
            xAxis = default; yAxis = default; zAxis = default;
            xAxis = TVector3D.SetFromMatrix4Column(this, 0);
            yAxis = TVector3D.SetFromMatrix4Column(this, 1);
            zAxis = TVector3D.SetFromMatrix4Column(this, 2);

        }

        public TMatrix4 MakeBasis(TVector3D xAxis, TVector3D yAxis, TVector3D zAxis)
        {
            Set(
                xAxis.X, yAxis.X, zAxis.X, 0,
                xAxis.Y, yAxis.Y, zAxis.Y, 0,
                xAxis.Z, yAxis.Z, zAxis.Z, 0,
                0, 0, 0, 1
            );

            return this;
        }

        public TMatrix4 makeRotationFromEuler() { throw new NotImplementedException(); }

        public TMatrix4 PostMultiply(TMatrix4 m)
        {
            SetElements(MultiplyMatrices(this, m).elements);
            return this;
        }

        public TMatrix4 PreMultiply(TMatrix4 m)
        {
            SetElements(MultiplyMatrices(m, this).elements);
            return this;
        }

        public TMatrix4 MultiplyScalar(double s)
        {
            _elements[0] *= s; _elements[4] *= s; _elements[8] *= s; _elements[12] *= s;
            _elements[1] *= s; _elements[5] *= s; _elements[9] *= s; _elements[13] *= s;
            _elements[2] *= s; _elements[6] *= s; _elements[10] *= s; _elements[14] *= s;
            _elements[3] *= s; _elements[7] *= s; _elements[11] *= s; _elements[15] *= s;

            return this;
        }

        public double Determinant()
        {

            double[] te = elements;

            double n11 = te[0], n12 = te[4], n13 = te[8], n14 = te[12];
            double n21 = te[1], n22 = te[5], n23 = te[9], n24 = te[13];
            double n31 = te[2], n32 = te[6], n33 = te[10], n34 = te[14];
            double n41 = te[3], n42 = te[7], n43 = te[11], n44 = te[15];

            //TODO: make this more efficient
            //( based on http://www.euclideanspace.com/maths/algebra/matrix/functions/inverse/fourD/index.htm )

            return
                (
                n41 * (
                    +n14 * n23 * n32
                     - n13 * n24 * n32
                     - n14 * n22 * n33
                     + n12 * n24 * n33
                     + n13 * n22 * n34
                     - n12 * n23 * n34
                ) +
                n42 * (
                    +n11 * n23 * n34
                     - n11 * n24 * n33
                     + n14 * n21 * n33
                     - n13 * n21 * n34
                     + n13 * n24 * n31
                     - n14 * n23 * n31
                ) +
                n43 * (
                    +n11 * n24 * n32
                     - n11 * n22 * n34
                     - n14 * n21 * n32
                     + n12 * n21 * n34
                     + n14 * n22 * n31
                     - n12 * n24 * n31
                ) +
                n44 * (
                    -n13 * n22 * n31
                     - n11 * n23 * n32
                     + n11 * n22 * n33
                     + n13 * n21 * n32
                     - n12 * n21 * n33
                     + n12 * n23 * n31
                )
                );
        }

        public TMatrix4 Transpose()
        {
            double tmp;

            tmp = _elements[1]; _elements[1] = _elements[4]; _elements[4] = tmp;
            tmp = _elements[2]; _elements[2] = _elements[8]; _elements[8] = tmp;
            tmp = _elements[6]; _elements[6] = _elements[9]; _elements[9] = tmp;

            tmp = _elements[3]; _elements[3] = _elements[12]; _elements[12] = tmp;
            tmp = _elements[7]; _elements[7] = _elements[13]; _elements[13] = tmp;
            tmp = _elements[11]; _elements[11] = _elements[14]; _elements[14] = tmp;

            return this;
        }

        public TMatrix4 SetPosition(TPoint3D p)
        {
            _elements[12] = p.X;
            _elements[13] = p.Y;
            _elements[14] = p.Z;
            return this;
        }

        public TMatrix4 SetPosition(double x, double y, double z)
        {
            _elements[12] = x;
            _elements[13] = y;
            _elements[14] = z;
            return this;
        }

        public TMatrix4 Invert()
        {

            // based on http://www.euclideanspace.com/maths/algebra/matrix/functions/inverse/fourD/index.htm
            double[] te = elements;

            double n11 = te[0], n21 = te[1], n31 = te[2], n41 = te[3],
            n12 = te[4], n22 = te[5], n32 = te[6], n42 = te[7],
            n13 = te[8], n23 = te[9], n33 = te[10], n43 = te[11],
            n14 = te[12], n24 = te[13], n34 = te[14], n44 = te[15],

            t11 = n23 * n34 * n42 - n24 * n33 * n42 + n24 * n32 * n43 - n22 * n34 * n43 - n23 * n32 * n44 + n22 * n33 * n44,
            t12 = n14 * n33 * n42 - n13 * n34 * n42 - n14 * n32 * n43 + n12 * n34 * n43 + n13 * n32 * n44 - n12 * n33 * n44,
            t13 = n13 * n24 * n42 - n14 * n23 * n42 + n14 * n22 * n43 - n12 * n24 * n43 - n13 * n22 * n44 + n12 * n23 * n44,
            t14 = n14 * n23 * n32 - n13 * n24 * n32 - n14 * n22 * n33 + n12 * n24 * n33 + n13 * n22 * n34 - n12 * n23 * n34;

            double det = n11 * t11 + n21 * t12 + n31 * t13 + n41 * t14;

            if (det == 0) { Set(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); return this; }

            double detInv = 1 / det;

            _elements[0] = t11 * detInv;
            _elements[1] = (n24 * n33 * n41 - n23 * n34 * n41 - n24 * n31 * n43 + n21 * n34 * n43 + n23 * n31 * n44 - n21 * n33 * n44) * detInv;
            _elements[2] = (n22 * n34 * n41 - n24 * n32 * n41 + n24 * n31 * n42 - n21 * n34 * n42 - n22 * n31 * n44 + n21 * n32 * n44) * detInv;
            _elements[3] = (n23 * n32 * n41 - n22 * n33 * n41 - n23 * n31 * n42 + n21 * n33 * n42 + n22 * n31 * n43 - n21 * n32 * n43) * detInv;

            _elements[4] = t12 * detInv;
            _elements[5] = (n13 * n34 * n41 - n14 * n33 * n41 + n14 * n31 * n43 - n11 * n34 * n43 - n13 * n31 * n44 + n11 * n33 * n44) * detInv;
            _elements[6] = (n14 * n32 * n41 - n12 * n34 * n41 - n14 * n31 * n42 + n11 * n34 * n42 + n12 * n31 * n44 - n11 * n32 * n44) * detInv;
            _elements[7] = (n12 * n33 * n41 - n13 * n32 * n41 + n13 * n31 * n42 - n11 * n33 * n42 - n12 * n31 * n43 + n11 * n32 * n43) * detInv;

            _elements[8] = t13 * detInv;
            _elements[9] = (n14 * n23 * n41 - n13 * n24 * n41 - n14 * n21 * n43 + n11 * n24 * n43 + n13 * n21 * n44 - n11 * n23 * n44) * detInv;
            _elements[10] = (n12 * n24 * n41 - n14 * n22 * n41 + n14 * n21 * n42 - n11 * n24 * n42 - n12 * n21 * n44 + n11 * n22 * n44) * detInv;
            _elements[11] = (n13 * n22 * n41 - n12 * n23 * n41 - n13 * n21 * n42 + n11 * n23 * n42 + n12 * n21 * n43 - n11 * n22 * n43) * detInv;

            _elements[12] = t14 * detInv;
            _elements[13] = (n13 * n24 * n31 - n14 * n23 * n31 + n14 * n21 * n33 - n11 * n24 * n33 - n13 * n21 * n34 + n11 * n23 * n34) * detInv;
            _elements[14] = (n14 * n22 * n31 - n12 * n24 * n31 - n14 * n21 * n32 + n11 * n24 * n32 + n12 * n21 * n34 - n11 * n22 * n34) * detInv;
            _elements[15] = (n12 * n23 * n31 - n13 * n22 * n31 + n13 * n21 * n32 - n11 * n23 * n32 - n12 * n21 * n33 + n11 * n22 * n33) * detInv;

            return this;
        }

        public TMatrix4 Scale(TVector3D v)
        {
            ;
            double x = v.X, y = v.Y, z = v.Z;

            _elements[0] *= x; _elements[4] *= y; _elements[8] *= z;
            _elements[1] *= x; _elements[5] *= y; _elements[9] *= z;
            _elements[2] *= x; _elements[6] *= y; _elements[10] *= z;
            _elements[3] *= x; _elements[7] *= y; _elements[11] *= z;

            return this;
        }

        public double GetMaxScaleOnAxis()
        {

            var te = elements;

            double scaleXSq = te[0] * te[0] + te[1] * te[1] + te[2] * te[2];
            double scaleYSq = te[4] * te[4] + te[5] * te[5] + te[6] * te[6];
            double scaleZSq = te[8] * te[8] + te[9] * te[9] + te[10] * te[10];


            return Math.Sqrt(new double[] { scaleXSq, scaleYSq, scaleZSq }.Max());

        }

        public void Translate(TVector3D vector)
        {
            Set(
                _elements[0], _elements[1], _elements[2], vector.X,
                _elements[4], _elements[5], _elements[6], vector.Y,
                _elements[8], _elements[9], _elements[10], vector.Z,
                _elements[12], _elements[13], _elements[14], _elements[15]
                );
        }

        public static TMatrix4 Compose(in TPoint3D position, in TQuaternion quaternion, in TVector3D scale)
        {

            var te = new double[16];

            double x = quaternion._x, y = quaternion._y, z = quaternion._z, w = quaternion._w;
            double x2 = x + x, y2 = y + y, z2 = z + z;
            double xx = x * x2, xy = x * y2, xz = x * z2;
            double yy = y * y2, yz = y * z2, zz = z * z2;
            double wx = w * x2, wy = w * y2, wz = w * z2;

            double sx = scale.X, sy = scale.Y, sz = scale.Z;

            te[0] = (1 - (yy + zz)) * sx;
            te[1] = (xy + wz) * sx;
            te[2] = (xz - wy) * sx;
            te[3] = 0;

            te[4] = (xy - wz) * sy;
            te[5] = (1 - (xx + zz)) * sy;
            te[6] = (yz + wx) * sy;
            te[7] = 0;

            te[8] = (xz + wy) * sz;
            te[9] = (yz - wx) * sz;
            te[10] = (1 - (xx + yy)) * sz;
            te[11] = 0;

            te[12] = position.X;
            te[13] = position.Y;
            te[14] = position.Z;
            te[15] = 1;

            var result = new TMatrix4();
            result.SetElements(te);

            return result;
        }

        public TMatrix4 Decompose(out TPoint3D position, out TQuaternion quaternion, out TVector3D scale)
        {
            position = default(TPoint3D);
            quaternion = default(TQuaternion);
            scale = default(TVector3D);

            var te = this.elements;

            //let sx = _v1.set(te[0], te[1], te[2]).length();
            //const sy = _v1.set(te[4], te[5], te[6]).length();
            //const sz = _v1.set(te[8], te[9], te[10]).length();

            double sx = new TVector3D(te[0], te[1], te[2]).Length();
            double sy = new TVector3D(te[4], te[5], te[6]).Length();
            double sz = new TVector3D(te[8], te[9], te[10]).Length();


            // if determine is negative, we need to invert one scale
            double det = this.Determinant();
            if (det < 0) sx = -sx;

            position.X = te[12];
            position.Y = te[13];
            position.Z = te[14];

            // scale the rotation part
            TMatrix4 _m1 = new TMatrix4();
            _m1.Copy(this);

            double invSX = 1 / sx;
            double invSY = 1 / sy;
            double invSZ = 1 / sz;

            #region outer change
            var _m1_element = _m1.elements;

            _m1_element[0] *= invSX;
            _m1_element[1] *= invSX;
            _m1_element[2] *= invSX;

            _m1_element[4] *= invSY;
            _m1_element[5] *= invSY;
            _m1_element[6] *= invSY;

            _m1_element[8] *= invSZ;
            _m1_element[9] *= invSZ;
            _m1_element[10] *= invSZ;
            #endregion

            _m1.SetElements(_m1_element);

            quaternion = TQuaternion.SetFromRotationMatrix(_m1);

            scale.X = sx;
            scale.Y = sy;
            scale.Z = sz;

            return this;
        }

        public TMatrix4 FromArray(double[] array, int offset = 0)
        {
            if (array.Length != 16) throw new ArgumentException("Arrays Length must be 16");
            _elements = new double[16 + offset];
            for (int i = 0; i < 16; i++) _elements[i] = array[i + offset];

            return this;
        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[16 + offset];

            for (int i = 0; i < offset; i++) array[i] = double.NaN;

            var te = _elements;

            array[offset] = te[0];
            array[offset + 1] = te[1];
            array[offset + 2] = te[2];
            array[offset + 3] = te[3];

            array[offset + 4] = te[4];
            array[offset + 5] = te[5];
            array[offset + 6] = te[6];
            array[offset + 7] = te[7];

            array[offset + 8] = te[8];
            array[offset + 9] = te[9];
            array[offset + 10] = te[10];
            array[offset + 11] = te[11];

            array[offset + 12] = te[12];
            array[offset + 13] = te[13];
            array[offset + 14] = te[14];
            array[offset + 15] = te[15];

            return array;
        }

        public bool Equals(TMatrix4 other, double tolerance)
        {
            if (elements.Length != other.elements.Length)
            {

                return false;

            }

            for (int i = 0, il = elements.Length; i < il; i++)
            {

                double delta = elements[i] - other.elements[i];
                if (Math.Abs(delta) > tolerance)
                {
                    return false;
                }

            }

            return true;
        }

        public bool Equals(TMatrix4 other) => Equals(other, 0.0001);

        #endregion

        public override string ToString()
        {
            string str = "TMatrix4:";
            for (int i = 0; i < 16; ++i)
            {
                if (i % 4 == 0) { str += "\n "; }
                str += (elements[i] + " ");
            }
            return str;
        }
    }
}
