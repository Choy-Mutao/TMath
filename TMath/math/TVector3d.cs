using System;

namespace tmath
{
    public struct TVector3D : IVector<TVector3D>
    {
        #region Fields
        public static readonly TVector3D XAxis = new TVector3D(1.0, 0.0, 0.0);
        public static readonly TVector3D YAxis = new TVector3D(0.0, 1.0, 0.0);
        public static readonly TVector3D ZAxis = new TVector3D(0.0, 0.0, 1.0);

        //public int dimn { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        #endregion

        #region Constructor

        public TVector3D(double a, double b, double c)
        {
            X = a; Y = b; Z = c;
        }
        public TVector3D(TPoint3D a, TPoint3D b)
        {
            var v = b - a;
            X = v.X; Y = v.Y; Z = v.Z;
        }
        public TVector3D(TVector3D vector3)
        {
            X = vector3.X; Y = vector3.Y; Z = vector3.Z;
        }
        public TVector3D(double[] v_array)
        {
            if (v_array.Length != 3) throw new ArgumentException("v_array Length must be 3");
            X = v_array[0]; Y = v_array[1]; Z = v_array[2];
        }

        #endregion

        #region Operators

        //------------------------------------------------------------------
        //  Unary Ops
        //------------------------------------------------------------------

        // Unary minus
        public static TVector3D operator -(TVector3D u) => new TVector3D { X = -u.X, Y = -u.Y, Z = -u.Z };

        // Unary 2D perp operator
        public static TVector3D operator ~(TVector3D u) => new TVector3D { X = -u.Y, Y = u.X, Z = u.Z };

        //----------------------------------------------------------
        // Scalar Multiplication
        public static TVector3D operator *(int n, TVector3D vector3) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TVector3D operator *(double n, TVector3D vector3) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TVector3D operator *(float n, TVector3D vector3) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TVector3D operator *(TVector3D vector3, int n) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TVector3D operator *(TVector3D vector3, double n) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TVector3D operator *(TVector3D vector3, float n) => new TVector3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        // Scalar Division
        public static TVector3D operator /(TVector3D vector3, int n) => n == 0 ? throw new DivideByZeroException() : new TVector3D(vector3.X / n, vector3.Y / n, vector3.Z / n);
        public static TVector3D operator /(TVector3D vector3, double n) => n == 0 ? throw new DivideByZeroException() : new TVector3D(vector3.X / n, vector3.Y / n, vector3.Z / n);
        public static TVector3D operator /(TVector3D vector3, float n) => n == 0 ? throw new DivideByZeroException() : new TVector3D(vector3.X / n, vector3.Y / n, vector3.Z / n);

        //------------------------------------------------------------------
        //  Arithmetic Ops
        //------------------------------------------------------------------

        public static TVector3D operator +(TVector3D v1, TVector3D v2) => new TVector3D { X = v1.X + v2.X, Y = v1.Y + v2.Y, Z = v1.Z + v2.Z };
        public static TVector3D operator -(TVector3D v1, TVector3D v2) => new TVector3D { X = v1.X - v2.X, Y = v1.Y - v2.Y, Z = v1.Z - v2.Z };

        //------------------------------------------------------------------
        //  Products
        //------------------------------------------------------------------

        // Inner Dot Product
        public static double operator *(TVector3D v, TVector3D w) => (v.X * w.X + v.Y * w.Y + v.Z * w.Z);

        #endregion

        #region Methods

        // TODO: 需要测试点绕轴的旋转结果
        public TVector3D RotateByAxis(double angle, TVector3D axis)
        {
            TVector3D q = new TVector3D(0.0, 0.0, 0.0);
            double costheta, sintheta;

            axis.Normalize();
            costheta = Math.Cos(angle);
            sintheta = Math.Sin(angle);

            q.X += (costheta + (1 - costheta) * axis.X * axis.X) * X;
            q.X += ((1 - costheta) * axis.X * axis.Y - axis.Z * sintheta) * Y;
            q.X += ((1 - costheta) * axis.X * axis.Z + axis.Y * sintheta) * Z;

            q.Y += ((1 - costheta) * axis.X * axis.Y + axis.Z * sintheta) * X;
            q.Y += (costheta + (1 - costheta) * axis.Y * axis.Y) * Y;
            q.Y += ((1 - costheta) * axis.Y * axis.Z - axis.X * sintheta) * Z;

            q.Z += ((1 - costheta) * axis.X * axis.Z - axis.Y * sintheta) * X;
            q.Z += ((1 - costheta) * axis.Y * axis.Z + axis.X * sintheta) * Y;
            q.Z += (costheta + (1 - costheta) * axis.Z * axis.Z) * Z;

            return q;
        }

        public TVector3D GetNormal()
        {
            TVector3D normal = new TVector3D(this);
            normal.Normalize();
            return normal;
        }

        // 向量叉乘
        public void Perp(TVector3D vector3)
        {
            double ax = X, ay = Y, az = Z;
            double bx = vector3.X, by = vector3.Y, bz = vector3.Z;

            X = ay * bz - az * by;
            Y = az * bx - ax * bz;
            Z = ax * by - ay * bx;
        }

        public static TVector3D CrossProduct(TVector3D a, TVector3D b)
        {
            TVector3D vector3 = new TVector3D();

            double ax = a.X, ay = a.Y, az = a.Z;
            double bx = b.X, by = b.Y, bz = b.Z;

            vector3.X = ay * bz - az * by;
            vector3.Y = az * bx - ax * bz;
            vector3.Z = ax * by - ay * bx;

            return vector3;
        }

        public static TVector3D SetFromMatrix4Column(TMatrix4 m, int index)
        {
            TVector3D result = new TVector3D();
            result.FromArray(m.elements, index * 4);
            return result;
        }

        public static TVector3D SetFromMatrix3Column(TMatrix3 m, int index)
        {
            TVector3D result = new TVector3D();
            result.FromArray(m.elements, index * 3);
            return result;
        }

        public override bool Equals(object obj) => obj is TVector3D v && Equals(v);

        public bool Equals(TVector3D other) => IsEqualTo(other, 0);

        public double Length() => Math.Sqrt(LengthSq());
        public double LengthSq() => X * X + Y * Y + Z * Z;

        public void Normalize()
        {
            double l = Length(); X /= l; Y /= l; Z /= l;
        }

        public TVector3D FromArray(double[] array, int offset = 0)
        {
            X = array[offset];
            Y = array[offset + 1];
            Z = array[offset + 2];

            return this;
        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[3];

            array[offset] = X;
            array[offset + 1] = Y;
            array[offset + 2] = Z;

            return array;
        }

        public TVector3D Copy(TVector3D v)
        {
            X = v.X; Y = v.Y; Z = v.Z; return this;
        }

        public TVector3D Clone() => new TVector3D(X, Y, Z);

        public double Dot(TVector3D v) => this * v;
        public static double Dot(TVector3D a, TVector3D b) => a * b;

        public bool IsEqualTo(TVector3D p, Tolerance tol) => NumberUtils.CompValue((this - p).Length(), 0, tol.EqualVector) == 0;

        public bool IsEqualTo(TVector3D p, double tol = 0.001) => IsEqualTo(p, new Tolerance(0, tol));

        public bool IsParallelTo(TVector3D v)
        {
            return (this * v / (v.Length() * Length())) == 1;
        }

        public static bool IsTwoParallel(TVector3D v1, TVector3D v2)
        {
            return v1.IsParallelTo(v2) && v2.IsParallelTo(v1);
        }

        public bool IsSameDirectionTo(TVector3D v)
        {
            return Dot(v) > 0 && CrossProduct(this, v).Length() == 0;
        }

        public static bool IsTwoSameDirection(TVector3D v1, TVector3D v2) => v1.IsSameDirectionTo(v2) && v2.IsSameDirectionTo(v1);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public TVector3D GetNegate()
        {
            X = -X; Y = -Y; Z = -Z; return this;
        }

        public static TVector3D SubVectors(TPoint3D a, TPoint3D b)
        {
            TVector3D result = new TVector3D();
            result.X = a.X - b.X; result.Y = a.Y - b.Y; result.Z = a.Z - b.Z;
            return result;
        }

        public void ApplyMatrix4(TMatrix4 m)
        {
            double x = this.X, y = this.Y, z = this.Z;
            double[] me = m.elements;

            double w = 1 / (me[3] * x + me[7] * y + me[11] * z + me[15]);

            X = (me[0] * x + me[4] * y + me[8] * z + me[12]) * w;
            Y = (me[1] * x + me[5] * y + me[9] * z + me[13]) * w;
            Z = (me[2] * x + me[6] * y + me[10] * z + me[14]) * w;

        }

        public void TransformDirection(TMatrix4 m)
        {
            // input: THREE.Matrix4 affine matrix
            // vector interpreted as a direction

            double x = X, y = Y, z = Z;
            var e = m.elements;

            X = e[0] * x + e[4] * y + e[8] * z;
            Y = e[1] * x + e[5] * y + e[9] * z;
            Z = e[2] * x + e[6] * y + e[10] * z;

            Normalize();
        }

        public static TVector3D SubVectors(TVector3D a, TVector3D b)
        {
            double x = a.X - b.X;
            double y = a.Y - b.Y;
            double z = a.Z - b.Z;
            return new TVector3D(x, y, z);
        }

        public void Set(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public TPoint3D ToPoint() => new TPoint3D(X, Y, Z);

        // 向量乘法
        public void Multiply(TVector3D v)
        {
            X *= v.X;
            Y *= v.Y;
            Z *= v.Z;
        }

        public TVector3D Multiple(int i) => this * i;

        public TVector3D Multiple(double d) => this * d;

        public TVector3D Multiple(float f) => this * f;
        #endregion
    }
}
