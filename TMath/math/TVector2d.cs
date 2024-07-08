using System;

namespace tmath
{
    [Serializable]
    public struct TVector2D : IVector<TVector2D>
    {
        #region
        public static readonly TVector2D XAxis = new TVector2D(1.0, 0.0);
        public static readonly TVector2D YAxis = new TVector2D(0.0, 1.0);

        public double X { get; set; }
        public double Y { get; set; }
        #endregion

        #region Constructor

        public TVector2D(double a, double b)
        {
            X = a; Y = b;
        }

        public TVector2D(double[] array)
        {
            if (array.Length != 2) throw new ArgumentException();
            X = array[0]; Y = array[1];
        }

        public TVector2D(TPoint2D a, TPoint2D b)
        {
            var v = b - a;
            this.X = v.X;
            this.Y = v.Y;

        }

        public TVector2D(TVector2D src)
        {
            X = src.X; Y = src.Y;
        }

        public TVector2D(TPoint2D P)
        {
            X = P.X; Y = P.Y;
        }
        #endregion

        #region Operators

        //----------------------------------------------------------
        // Add Operator
        public static TVector2D operator +(TVector2D v1, TVector2D v2) => new TVector2D(v1.X + v2.X, v1.Y + v2.Y);

        // Minu Operator
        public static TVector2D operator -(TVector2D v1, TVector2D v2) => new TVector2D(v1.X - v2.X, v1.Y - v2.Y);

        // Nevigate Operator
        public static TVector2D operator -(TVector2D v) => new TVector2D(-v.X, -v.Y);

        // Scalar Multiplication
        public static TVector2D operator *(int n, TVector2D vector2) => new TVector2D(vector2.X * n, vector2.Y * n);
        public static TVector2D operator *(double n, TVector2D vector2) => new TVector2D(vector2.X * n, vector2.Y * n);
        public static TVector2D operator *(float n, TVector2D vector2) => new TVector2D(vector2.X * n, vector2.Y * n);
        public static TVector2D operator *(TVector2D vector2, int n) => new TVector2D(vector2.X * n, vector2.Y * n);
        public static TVector2D operator *(TVector2D vector2, double n) => new TVector2D(vector2.X * n, vector2.Y * n);
        public static TVector2D operator *(TVector2D vector2, float n) => new TVector2D(vector2.X * n, vector2.Y * n);
        // Scalar Division
        public static TVector2D operator /(TVector2D vector2, int n) => n == 0 ? throw new DivideByZeroException() : new TVector2D(vector2.X / n, vector2.Y / n);
        public static TVector2D operator /(TVector2D vector2, double n) => n == 0 ? throw new DivideByZeroException() : new TVector2D(vector2.X / n, vector2.Y / n);
        public static TVector2D operator /(TVector2D vector2, float n) => n == 0 ? throw new DivideByZeroException() : new TVector2D(vector2.X / n, vector2.Y / n);
        //------------------------------------------------------------------
        //  Products
        //------------------------------------------------------------------

        // Inner Dot Product
        public static double operator *(TVector2D v, TVector2D w) => (v.X * w.X + v.Y * w.Y);
        #endregion

        #region Static Methods
        public static bool Up(TVector2D u, TVector2D v) => u.Dot(v) > 0;
        public static bool Down(TVector2D u, TVector2D v) => u.Dot(v) < 0;
        #endregion

        #region Methods
        public TPoint2D ToTPoint2D() => new TPoint2D(X, Y);

        public TVector2D RotateByPoint(double angle, TPoint2D point)
        {
            TVector2D rotated = new TVector2D(1, 0);

            double costheta = Math.Cos(angle);
            double sintheta = Math.Sin(angle);

            double x = X - point.X;
            double y = Y - point.Y;

            rotated.X = x * costheta - y * sintheta + point.X;
            rotated.Y = x * sintheta + y * costheta + point.Y;

            return rotated;
        }

        public double Perp(TVector2D vector2) => X * vector2.Y - Y * vector2.X;
        public static double Cross(TVector2D a, TVector2D b) => a.X * b.Y - a.Y * b.X;
        public bool IsEqualTo(TVector2D vector2, double tol = 1e-3)
        {
            throw new NotImplementedException();
        }

        public bool Equals(TVector2D other)
        {
            throw new NotImplementedException();
        }

        public double Length() => Math.Sqrt(LengthSq());

        public double LengthSq() => X * X + Y * Y;

        public void Normalize()
        {
            double l = Length(); X /= l; Y /= l;
        }

        public void SetLength(double D)
        {
            Normalize();
            double unity = (X * X + Y * Y);
            if (unity == 0) throw new DivideByZeroException("This vector is zero");

            int sign = Math.Sign(D);

            double d = Math.Sqrt(D * D / unity);
            X *= d * sign;
            Y *= d * sign;
        }

        public TVector2D GetNormal()
        {
            TVector2D normal = new TVector2D(X, Y);
            normal.Normalize();
            return normal;
        }

        public TVector2D FromArray(double[] array, int offset = 0)
        {
            X = array[0 + offset];
            Y = array[1 + offset];
            return this;
        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[2];
            array[offset] = X;
            array[offset + 1] = Y;

            return array;
        }

        public TVector2D Copy(TVector2D v)
        {
            X = v.X; Y = v.Y; return this;
        }

        public double Dot(TVector2D v) => this * v;

        public bool IsEqualTo(TVector2D p, Tolerance tol)
        {
            throw new NotImplementedException();
        }

        public TVector2D GetNegate()
        {
            X = -X; Y = -Y;
            return this;
        }

        public double Angle2DTo(TVector2D base_v)
        {
            TVector2D self = this;
            self.Normalize();
            base_v.Normalize();
            double dValue = self * base_v;
            if (dValue > 1) { dValue = 1.0; }
            if (dValue < -1) { dValue = -1.0; }
            double angle = Math.Acos(dValue);

            TPoint2D origin = new TPoint2D(0, 0);
            if (GeometryUtil.CommonUtil.IsLeft(origin, base_v.ToTPoint2D(), self.ToTPoint2D()) == 1) return angle;
            else if (GeometryUtil.CommonUtil.IsLeft(origin, base_v.ToTPoint2D(), self.ToTPoint2D()) == -1) return -angle;
            return angle;
        }

        public double Angle() => Angle2DTo(new TVector2D(1.0, 0.0));

        public TVector2D Multiple(int i) => this * i;

        public TVector2D Multiple(double d) => this * d;

        public TVector2D Multiple(float f) => this * f;
        #endregion
    }
}
