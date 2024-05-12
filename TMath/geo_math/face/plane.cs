using System;

/// <summary>
/// 不特意定义二维平面
/// </summary>
namespace tmath.geometry
{
    /// <summary>
    /// 不特意定义二维平面
    /// </summary>
    public class TPlane3D : IEquatable<TPlane3D>
    {

        #region Fields
        public static TPlane3D kXYPlane = new TPlane3D(new TVector3D(0, 0, 1));
        public static TPlane3D kXZPlane = new TPlane3D(new TVector3D(0, 1, 0));
        public static TPlane3D kYZPlane = new TPlane3D(new TVector3D(0, 0, 1));

        protected TVector3D _normal = new TVector3D(0, 0, 1);
        protected double _constant = 0.0;

        public TVector3D Normal { get { return _normal; } set => _normal = value; }
        public double Constant { get { return _constant; } set => _constant = value; }
        public TPoint3D Origin { get => GetPlanerOrigin(_normal, _constant); }
        #endregion

        #region Constructor
        public TPlane3D()
        {
            _normal = new TVector3D(0, 0, 1);
            _constant = 0.0;
        }

        public TPlane3D(TVector3D n)
        {
            _normal = new TVector3D(n);
            if (_normal.Length() == 0) throw new DivideByZeroException("Plane normal is 0 Length");
            _constant = 0.0;
        }

        public TPlane3D(double c)
        {
            _constant = c;
            _normal = new TVector3D(0, 0, 1);
        }

        public TPlane3D(in TVector3D n, double c)
        {
            _normal = new TVector3D(n);
            _constant = c;
        }

        public TPlane3D(in TVector3D n, in TPoint3D o)
        {
            _normal = new TVector3D(n);
            if (_normal.Length() == 0) throw new DivideByZeroException("Plane normal is 0 Length");
            TPoint3D P = new TPoint3D(0, 0, 0);
            double sn = _normal * (P - o).ToVector();
            double sd = _normal * _normal;
            if (sd == 0) throw new DivideByZeroException("Divided by Zero");
            _constant = sn / sd;
        }

        /// <summary>
        /// 点的方向控制平面的法线方向
        /// </summary>
        /// <param name="_p1"></param>
        /// <param name="_p2"></param>
        /// <param name="_p3"></param>
        public TPlane3D(TPoint3D _p1, TPoint3D _p2, TPoint3D _p3) { Set(_p1, _p2, _p3); }
        #endregion

        #region Methods

        public void Set(TPoint3D p1, TPoint3D p2, TPoint3D p3)
        {
            #region 生成 normal 和 constant
            TVector3D v1 = (p2 - p1).ToVector();
            TVector3D v2 = (p3 - p2).ToVector();
            _normal = TVector3D.CrossProduct(v1, v2).GetNormal();
            if (_normal.Length() == 0) throw new DivideByZeroException("Three-point collinear");
            TPoint3D P = new TPoint3D(0, 0, 0);
            double sn = _normal * ((P - p1).ToVector());
            double sd = _normal * _normal;
            _constant = sn / sd;
            #endregion
        }

        public void Set(TVector3D normal, double constant)
        {
            _normal = normal;
            _constant = constant;
        }

        public TPlane3D SetComponents(double x, double y, double z, double w)
        {
            _normal = new TVector3D(x, y, z);
            _constant = w;
            return this;
        }

        public TPlane3D SetFromNormalAndCoplanarPoint(TVector3D normal, TPoint3D point)
        {

            _normal.Copy(normal);
            _constant = (-new TVector3D(point.ToArray())).Dot(_normal);

            return this;

        }

        public TPlane3D SetFromCoplanarPoints(TPoint3D a, TPoint3D b, TPoint3D c)
        {
            TVector3D normal =  TVector3D.SubVectors(c, b);
            normal.Perp(TVector3D.SubVectors(a, b));
            normal.Normalize();

            // Q: should an error be thrown if normal is zero (e.g. degenerate plane)?

            SetFromNormalAndCoplanarPoint(normal, a);

            return this;
        }

        public TPlane3D Copy(TPlane3D plane)
        {
            _normal = new TVector3D(plane.Normal);
            _constant = plane.Constant;
            return this;
        }

        public void Normalize()
        {
            if (_normal.Length() == 0) throw new DivideByZeroException("Plane normal is 0 Length");
            double inverseNormalLength = 1.0 / _normal.Length();
            _normal *= inverseNormalLength;
            _constant *= inverseNormalLength;
        }

        public TPlane3D Negate()
        {
            _constant *= -1;
            _normal.GetNegate();
            return this;
        }

        public double DistanceToPoint(TPoint3D point)
        {
            return _normal * new TVector3D(point.ToArray()) + _constant;
        }
        public double DistanceToSphere(object shpere)
        {
            throw new NotImplementedException();
        }

        public TPoint3D ProjectPoint(in TPoint3D point)
        {
            TPoint3D target = default;
            target.Copy(point);
            target.AddScaledVector(_normal, -DistanceToPoint(point));
            return target;
        }

        public bool IntersectLine(TLine3D line, out TPoint3D target)
        {
            TVector3D direction = line.Delta();
            target = new TPoint3D();
            double denominator = Normal.Dot(direction);
            if (denominator == 0)
            {
                // line is coplanar return origin;
                if (DistanceToPoint(line.P1) == 0)
                {
                    target.Copy(line.P1);
                    return true;
                }
                return false;
            }

            double t = -(new TVector3D(line.P1.ToArray()) * Normal + Constant) / denominator;

            if (t < 0 || t > 1)
                return false;

            target.Copy(line.P1);
            target.AddScaledVector(direction, t);
            return true;
        }

        public void IntersectsLine() { throw new NotImplementedException(); }

        public bool IntersectsBox(TBox3D box) { throw new NotImplementedException(); }

        public bool IntersectsSphere(TSphere sphere) { throw new NotImplementedException(); }

        public TPoint3D CoplanarPoint() { throw new NotImplementedException(); }

        public void ApplyMatrix4(TMatrix4 matrix) { throw new NotImplementedException(); }

        public void Translate(TVector3D move_vector) { throw new NotImplementedException(); }

        public bool Equals(TPlane3D plane)
        {
            return plane.Normal.Equals(_normal) && plane.Constant == _constant;
        }

        public bool Equals(TPlane3D plane, double tol)
        {
            throw new NotImplementedException();
        }

        public TPlane3D Clone()
        {
            TPlane3D plane = new TPlane3D();
            plane.Copy(this);
            return plane;
        }

        public bool IsOn(TPoint3D p) => 0 == NumberUtils.CompValue(DistanceToPoint(p), 0);

        public static TPoint3D GetPlanerOrigin(TVector3D normal, double constant)
        {
            // 选择一个任意点，例如 (0, 0, 0)
            TPoint3D arbitraryPoint = new TPoint3D(0, 0, 0);

            // 计算平面上一点的坐标
            double t = -(TVector3D.Dot(normal, new TVector3D(arbitraryPoint.ToArray())) + constant) / TVector3D.Dot(normal, normal);

            // 获取平面原点
            TPoint3D planeOrigin = arbitraryPoint + t * normal;
            return planeOrigin;
        }


        #endregion
    }
}
