using System;

namespace tmath
{
    [Serializable]
    public struct TPoint2D : IPoint<TPoint2D, TVector2D>
    {
        #region  Fields
        public static TPoint2D ORIGIN_2D = new TPoint2D(0, 0);
        public static TPoint2D NULL = NegativeInfinity;
        public static TPoint2D PositiveInifinity = new TPoint2D(double.PositiveInfinity, double.PositiveInfinity);
        public static TPoint2D NegativeInfinity = new TPoint2D(double.NegativeInfinity, double.NegativeInfinity);

        public double X { get; set; }
        public double Y { get; set; }
        #endregion

        #region  Constructor
        public TPoint2D(double x, double y)
        {
            X = x; Y = y;
        }
        public TPoint2D(double[] array)
        {
            if (array.Length != 2) throw new ArgumentException("array Length must be 2");
            X = array[0]; Y = array[1];
        }

        public TPoint2D(TPoint2D src)
        {
            X = src.X; Y = src.Y;
        }

        public TPoint2D(TVector2D V)
        {
            X = V.X; Y = V.Y;
        }
        #endregion

        #region Operator
        /// <summary>
        /// 严格相等
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static bool operator ==(TPoint2D p0, TPoint2D p1) => (p0.X == p1.X && p0.Y == p1.Y);

        public static bool operator !=(TPoint2D p0, TPoint2D p1) => (p0.X != p1.X || p0.Y != p1.Y);

        public static TPoint2D operator -(TPoint2D p1, TPoint2D p2) => new TPoint2D(p1.X - p2.X, p1.Y - p2.Y);

        public static TPoint2D operator -(TPoint2D p, TVector2D vec2d) => new TPoint2D(p.X - vec2d.X, p.Y - vec2d.Y);

        public static TPoint2D operator +(TPoint2D p, TVector2D vec2d) => new TPoint2D(p.X + vec2d.X, p.Y + vec2d.Y);

        public static TPoint2D operator +(TPoint2D p1, TPoint2D p2) => new TPoint2D(p1.X + p2.X, p1.Y + p2.Y);

        public static TPoint2D operator /(TPoint2D p, double n)
        {
            if (n == 0) throw new DivideByZeroException("TVector2D can not be divided by zero!");
            return new TPoint2D(p.X / n, p.Y / n);
        }

        public static TPoint2D operator *(TPoint2D p, double n) => new TPoint2D(p.X * n, p.Y * n);



        #endregion

        #region Static Methods
        // xyorder(): determines the xy lexicographical order of two points
        //      returns: (+1) if p1 > p2; (-1) if p1 < p2; and  0 if equal 
        public static int XYOrder(TPoint2D p1, TPoint2D p2)
        {
            // test the x-coord first
            if (p1.X > p2.X) return 1;
            if (p1.X < p2.X) return (-1);
            // and test the y-coord second
            if (p1.Y > p2.Y) return 1;
            if (p1.Y < p2.Y) return (-1);
            // when you exclude all other possibilities, what remains  is...
            return 0;  // they are the same point 
        }

        /// <summary>
        /// tests if point P2 is Left|On|Right of the line P0 to P1.
        /// </summary> 
        /// <param name="P0"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns> >0 for left, 0 for on, and <0 for  right of the line.</returns>
        public static int IsLeft(TPoint2D P0, TPoint2D P1, TPoint2D P2)
        {
            return Math.Sign((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
        }

        /// <summary>
        /// 判断 在 U 方向上， Vi 在 Vj 的前面
        /// </summary>
        /// <param name="u"></param>
        /// <param name="Vi"></param>
        /// <param name="Vj"></param>
        /// <returns></returns>
        public static bool Above(TVector2D u, TPoint2D Vi, TPoint2D Vj) => u.Dot((Vi - Vj).ToVector()) > 0;

        /// <summary>
        /// 判断 在 U 方向上， Vi 在 Vj 的后面
        /// </summary>
        /// <param name="u"></param>
        /// <param name="Vi"></param>
        /// <param name="Vj"></param>
        /// <returns></returns>
        public static bool Below(TVector2D u, TPoint2D Vi, TPoint2D Vj) => u.Dot((Vi - Vj).ToVector()) < 0;

        [Obsolete("在循环中没有真正循环")]
        public static TPoint2D Random(double minx = 0, double maxx = 1.0, double miny = 0, double maxy = 1.0)
        {
            var random = new Random();
            double x = random.NextDouble() * (maxx - minx) * (random.Next(2) * 2 - 1);
            double y = random.NextDouble() * (maxy - miny) * (random.Next(2) * 2 - 1);
            return new TPoint2D(x, y);
        }
        #endregion

        #region Methods
        public TPoint2D RotateByPoint(double angle, TPoint2D point)
        {
            TPoint2D rotated = new TPoint2D(0, 0);

            double costheta = Math.Cos(angle);
            double sintheta = Math.Sin(angle);

            double x = X - point.X;
            double y = Y - point.Y;

            rotated.X = x * costheta - y * sintheta + point.X;
            rotated.Y = x * sintheta + y * costheta + point.Y;

            X = rotated.X;
            Y = rotated.Y;

            return rotated;
        }

        public TVector2D ToVector() => new TVector2D(X, Y);

        public bool IsEqualTo(TPoint2D other, Tolerance tolerance) => NumberUtils.CompValue(DistanceTo(other), 0, tolerance.EqualPoint) == 0;

        public bool IsEqualTo(TPoint2D P, double tol = 1e-3) => IsEqualTo(P, new Tolerance(0, tol));

        public bool Equals(TPoint2D other) => IsEqualTo(other, 0);

        public double DistanceTo(TPoint2D Q) => (this - Q).ToVector().Length();

        public void ApplyMatrix3(TMatrix3 m)
        {
            double x = X, y = Y;
            var e = m.row_elements;
            X = e[0] * x + e[1] * y + e[2];
            Y = e[3] * x + e[4] * y + e[5];
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void Copy(TPoint2D p)
        {
            X = p.X; Y = p.Y;
        }

        public void Move(TVector2D v)
        {
            X += v.X; Y += v.Y;
        }

        public void FromArray(double[] array, int offset = 0)
        {
            X = array[0 + offset];
            Y = array[1 + offset];

        }

        public double[] ToArray(int offset = 0)
        {
            double[] array = new double[2];
            array[offset] = X;
            array[offset + 1] = Y;

            return array;
        }

        public TPoint2D ProjectOnVector(TVector2D vector)
        {
            double denominator = vector.LengthSq();
            if (denominator == 0) new TPoint2D(0, 0);
            double scalar = (X * vector.X + Y * vector.Y) / denominator;
            return new TPoint2D(vector.X * scalar, vector.Y * scalar);
        }

        public TPoint2D Add(TPoint2D t) => this + t;

        public TPoint2D Sub(TPoint2D t) => this - t;

        public TPoint2D Add(TVector2D v) => this + v;

        #endregion
    }
}
