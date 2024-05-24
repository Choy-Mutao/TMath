using System;

namespace tmath
{
    [Serializable]
    public struct TPoint3D : IPoint<TPoint3D, TVector3D>
    {
        #region  Fields
        public double X;
        public double Y;
        public double Z;
        public static TPoint3D PositiveInfinity = new TPoint3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public static TPoint3D NegativeInfinity = new TPoint3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        #endregion

        #region Static Fields
        public static TPoint3D O = new TPoint3D(0, 0, 0);
        public static TPoint3D I = new TPoint3D(1, 1, 1);
        public static TPoint3D II = new TPoint3D(2, 2, 2);
        public static TPoint3D NULL = NegativeInfinity;
        #endregion

        #region  Constructor
        public TPoint3D(double a, double b, double c)
        {
            X = a; Y = b; Z = c;
        }

        public TPoint3D(double[] array)
        {
            if (array.Length != 3) throw new ArgumentException("array Length must be 3");
            X = array[0]; Y = array[1]; Z = array[2];
        }

        public TPoint3D(TPoint3D src)
        {
            X = src.X; Y = src.Y; Z = src.Z;
        }
        #endregion

        #region Operators
        public static bool operator ==(TPoint3D p0, TPoint3D p1) => (p0.X == p1.X && p0.Y == p1.Y && p0.Z == p1.Z);

        public static bool operator !=(TPoint3D p0, TPoint3D p1) => (p0.X != p1.X || p0.Y != p1.Y || p0.Z != p1.Z);

        public static TPoint3D operator -(TPoint3D p1, TPoint3D p2) => new TPoint3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        public static TPoint3D operator +(TPoint3D p, TVector3D vec3d) => new TPoint3D(p.X + vec3d.X, p.Y + vec3d.Y, p.Z + vec3d.Z);
        public static TPoint3D operator +(TPoint3D a, TPoint3D b) => new TPoint3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        //----------------------------------------------------------
        // Scalar Multiplication
        public static TPoint3D operator *(int n, TPoint3D vector3) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TPoint3D operator *(double n, TPoint3D vector3) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TPoint3D operator *(float n, TPoint3D vector3) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TPoint3D operator *(TPoint3D vector3, int n) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TPoint3D operator *(TPoint3D vector3, double n) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        public static TPoint3D operator *(TPoint3D vector3, float n) => new TPoint3D(vector3.X * n, vector3.Y * n, vector3.Z * n);
        // Scalar Division
        public static TPoint3D operator /(TPoint3D vector3, int n) => n == 0 ? throw new DivideByZeroException() : new TPoint3D(vector3.X / n, vector3.Y / n, vector3.Z / n);
        public static TPoint3D operator /(TPoint3D vector3, double n) => n == 0 ? throw new DivideByZeroException() : new TPoint3D(vector3.X / n, vector3.Y / n, vector3.Z / n);
        public static TPoint3D operator /(TPoint3D vector3, float n) => n == 0 ? throw new DivideByZeroException() : new TPoint3D(vector3.X / n, vector3.Y / n, vector3.Z / n);

        // Random
        [Obsolete("在循环中没有真正循环")]
        public static TPoint3D Random(double minx = 0, double maxx = 1.0, double miny = 0, double maxy = 1.0, double minz = 0, double maxz = 1.0)
        {
            var random = new Random();
            double x = random.NextDouble() * (maxx - minx) * (random.Next(2) * 2 - 1);
            double y = random.NextDouble() * (maxy - miny) * (random.Next(2) * 2 - 1);
            double z = random.NextDouble() * (maxz - minz) * (random.Next(2) * 2 - 1);
            return new TPoint3D(x, y, z);
        }
        #endregion

        #region Methods
        public TPoint3D Add(TPoint3D t) => this + t;

        public TPoint3D Sub(TPoint3D s) => this - s;

        public bool IsEqualTo(TPoint3D point3, Tolerance tol)
        {
            return NumberUtils.CompValue(DistanceTo(point3), 0, tol.EqualPoint) == 0;
        }

        public bool IsEqualTo(TPoint3D point3, double tol = 1e-3)
        {
            return IsEqualTo(point3, new Tolerance(0, tol));
        }

        public override bool Equals(object obj)
        {
            if (obj is TPoint3D other) { return Equals(other); }
            return false;
        }

        public bool Equals(TPoint3D other) => X == other.X && Y == other.Y && Z == other.Z;

        public void ApplyMatrix2D(TMatrix3 m)
        {
            double x = X, y = Y, z = Z;
            double[] e = m.row_elements;

            X = e[0] * x + e[3] * y + e[6] * z;
            Y = e[1] * x + e[4] * y + e[7] * z;
            Z = e[2] * x + e[5] * y + e[8] * z;
        }

        public void ApplyMatrix4(TMatrix4 m)
        {
            double x = X, y = Y, z = Z;
            double[] e = m.elements;

            double w = 1 / (e[3] * x + e[7] * y + e[11] * z + e[15]);

            X = (e[0] * x + e[4] * y + e[8] * z + e[12]) * w;
            Y = (e[1] * x + e[5] * y + e[9] * z + e[13]) * w;
            Z = (e[2] * x + e[6] * y + e[10] * z + e[14]) * w;
        }

        public void TransformBy(TMatrix4 m) => ApplyMatrix4(m);

        public double DistanceToSquared(TPoint3D point)
        {
            double dx = X - point.X, dy = Y - point.Y, dz = Z - point.Z;
            return dx * dx + dy * dy + dz * dz;
        }
        public double DistanceTo(TPoint3D Q) => Math.Sqrt(DistanceToSquared(Q));

        public void Copy(TPoint3D p)
        {
            X = p.X; Y = p.Y; Z = p.Z;
        }

        public TPoint3D Clone() => new TPoint3D(X, Y, Z);

        public void Move(TVector3D v)
        {
            X += v.X; Y += v.Y; Z += v.Z;
        }

        public void FromArray(double[] array, int offset = 0)
        {
            X = array[offset]; Y = array[offset + 1]; Z = array[offset + 2];
        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[3];
            array[offset] = X;
            array[offset + 1] = Y;
            array[offset + 2] = Z;
            return array;
        }

        public void AddScaledVector(TVector3D v, double s)
        {
            X += v.X * s; Y += v.Y * s; Z += v.Z * s;
        }

        internal void SubVectors(TPoint3D a, TPoint3D b)
        {
            X = a.X - b.X;
            Y = a.Y - b.Y;
            Z = a.Z - b.Z;
        }

        public TPoint3D ProjectOnVector(TVector3D v)
        {
            double denominator = v.LengthSq();

            if (denominator == 0) new TPoint3D(0, 0, 0);

            double scalar = v.Dot(new TVector3D(ToArray())) / denominator;
            TPoint3D proj = new TPoint3D(v.ToArray());
            proj.MultiplyScalar(scalar);
            return proj;
        }

        private void MultiplyScalar(double scalar)
        {
            X *= scalar; Y *= scalar; Z *= scalar;
        }

        public void Set(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public override int GetHashCode()
        {
            int hashCode = 1276749221;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public TVector3D ToVector() => new TVector3D(X, Y, Z);

        public TPoint3D Add(TVector3D v) => this + v;
        #endregion
    }
}
