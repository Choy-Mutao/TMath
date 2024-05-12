
using System;

namespace tmath.geometry
{
    // 射线
    public abstract class TRay<P, V> where P : IPoint<P, V> where V : IVector<V>
    {
        protected P _origin;
        protected V _direction; // must be normalized
        public P origin { get => _origin; set => _origin = value; }
        public V direction { get => _direction; set => _direction = value; }

        public TRay() { }

        public TRay(P origin, V direction)
        {
            _origin = origin;
            _direction = direction;
        }

        public TRay<P, V> Copy(TRay<P, V> ray)
        {
            _origin.Copy(ray._origin);
            _direction.Copy(ray._direction);
            return this;
        }

        public void Negate()
        {
            direction = direction.GetNegate();
        }
    }

    // 2D 射线
    public class TRay2d : TRay<TPoint2D, TVector2D>
    {
        public TRay2d(TPoint2D origin, TVector2D direction) : base(origin, direction) { }

        public TRay2d(TPoint2D sp, TPoint2D ep) : base(sp, (ep - sp).ToVector()) { }

        #region Methods
        public TPoint2D? IntersectWithSegment(TLineSegment2d segment2)
        {
            double x1 = origin.X;
            double y1 = origin.Y;
            double x2 = direction.X + x1;
            double y2 = direction.Y + y1;

            double x3 = segment2.SP.X;
            double y3 = segment2.SP.Y;
            double x4 = segment2.EP.X;
            double y4 = segment2.EP.Y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (denominator == 0)
            {
                // The lines are parallel, so no intersection point
                return null;
            }

            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
            double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                // Intersection point exists
                return new TPoint2D(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
            }
            else
            {
                // No intersection point within the line segments
                return null;
            }
        }

        public TPoint2D[] IntersectWith(TLineSegment2d seg)
        {
            var res = IntersectWithSegment(seg);
            if (res == null) return null;
            else return new TPoint2D[1] { (TPoint2D)res };
        }
        #endregion
    }

    // 3D 射线
    public class TRay3d : TRay<TPoint3D, TVector3D>
    {
        #region Constructor
        public TRay3d() : base(new TPoint3D(0, 0, 0), new TVector3D(0, 0, -1)) { }
        public TRay3d(TPoint3D origin, TVector3D direction) : base(origin, direction) { }
        public TRay3d(TPoint3D sp, TPoint3D ep) : base(sp, (ep - sp).ToVector()) { }
        #endregion
            
        #region Methods

        public TRay3d Set(TPoint3D origin, TVector3D direction)
        {
            _origin.Copy(origin);
            _direction.Copy(direction);
            return this;
        }

        public TRay3d Copy(TRay3d ray)
        {
            _origin.Copy(ray._origin);
            _direction.Copy(ray._direction);
            return this;
        }

        public TPoint3D At(double t, out TPoint3D target)
        {
            target = default;
            target.Copy(_origin);
            target.AddScaledVector(_direction, t);
            return target;
        }

        public TRay3d LookAt(TPoint3D v)
        {
            (_direction.FromArray(v.ToArray()) - new TVector3D(_origin.ToArray())).Normalize();
            return this;
        }

        public TRay3d Recast(double t)
        {
            _origin.Copy(At(t, out _origin));
            return this;
        }

        public TPoint3D closestPointToPoint(TPoint3D point, TPoint3D target)
        {
            target.SubVectors(point, _origin);
            var directionDistance = new TVector3D(target.ToArray()).Dot(this._direction);
            if (directionDistance < 0) { target.Copy(_origin); }
            target.Copy(_origin);
            target.AddScaledVector(_direction, directionDistance);
            return target;
        }

        public double DistanceSqToPoint(TPoint3D point)
        {
            double directionDistance = TVector3D.SubVectors(point, _origin).Dot(_direction);

            // point behind the ray

            if (directionDistance < 0) return _origin.DistanceToSquared(point);

            TPoint3D _pointOnVector = new TPoint3D(_origin);
            _pointOnVector.AddScaledVector(_direction, directionDistance);

            return _pointOnVector.DistanceToSquared(point);
        }

        public double DistanceToPoint(TPoint3D point) => Math.Sqrt(DistanceSqToPoint(point));

        //TODO: 待研究算法
        public double DistanceSqToSegment(TPoint3D v0, TPoint3D v1, ref TPoint3D optionalPointOnRay, ref TPoint3D optionalPointOnSegment)
        {
            throw new NotImplementedException();
        }

        public double DistanceToPlane(TPlane3D plane)
        {
            var denominator = plane.Normal.Dot(_direction);
            if (denominator == 0)
            {
                // line is coplanar, return origin
                if (plane.DistanceToPoint(_origin) == 0) return 0;
                // Null is preferable to undefined since undefined means.... it is undefined
                return -1;
            }

            double t = -(new TVector3D(_origin.ToArray()).Dot(plane.Normal) + plane.Constant) / denominator;

            // Return if the ray never intersects the plane

            return t >= 0 ? t : -1;
        }

        public TPoint3D IntersectPlane(TPlane3D plane, Tolerance tol = default)
        {
            TPoint3D intercet_pnt = TPoint3D.PositiveInfinity;
            double t = DistanceToPlane(plane);
            if (t < tol.EqualPoint) return intercet_pnt;
            At(t, out intercet_pnt);
            return intercet_pnt;
        }

        /// <summary>
        /// 计算射线和box的交点
        /// </summary>
        /// <param name="box"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public TPoint3D? IntersectBox(TBox3D box, out TPoint3D target)
        {
            target = default;

            double tmin = double.NaN,
                    tmax = double.NaN,
                    tymin = double.NaN,
                    tymax = double.NaN,
                    tzmin = double.NaN,
                    tzmax = double.NaN;

            double invdirx = 1 / this.direction.X,
                invdiry = 1 / this.direction.Y,
                invdirz = 1 / this.direction.Z;

            TPoint3D _origin = this.origin;

            if (invdirx >= 0)
            {

                tmin = (box.Min.X - _origin.X) * invdirx;
                tmax = (box.Max.X - _origin.X) * invdirx;

            }
            else
            {

                tmin = (box.Max.X - _origin.X) * invdirx;
                tmax = (box.Min.X - _origin.X) * invdirx;

            }

            if (invdiry >= 0)
            {

                tymin = (box.Min.Y - _origin.Y) * invdiry;
                tymax = (box.Max.Y - _origin.Y) * invdiry;

            }
            else
            {

                tymin = (box.Max.Y - _origin.Y) * invdiry;
                tymax = (box.Min.Y - _origin.Y) * invdiry;

            }

            if ((tmin > tymax) || (tymin > tmax)) return null;

            if (tymin > tmin || double.IsNaN(tmin)) tmin = tymin;

            if (tymax < tmax || double.IsNaN(tmax)) tmax = tymax;

            if (invdirz >= 0)
            {

                tzmin = (box.Min.Z - _origin.Z) * invdirz;
                tzmax = (box.Max.Z - _origin.Z) * invdirz;

            }
            else
            {

                tzmin = (box.Max.Z - _origin.Z) * invdirz;
                tzmax = (box.Min.Z - _origin.Z) * invdirz;

            }

            if ((tmin > tzmax) || (tzmin > tmax)) return null;

            if (tzmin > tmin || tmin != tmin) tmin = tzmin;

            if (tzmax < tmax || tmax != tmax) tmax = tzmax;

            //return point closest to the ray (positive side)

            if (tmax < 0) return null;

            return this.At(tmin >= 0 ? tmin : tmax, out target);
        }

        /// <summary>
        /// 仅判断 ray 是否和 box 相交
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public bool IntersectsBox(TBox3D box)
        {
            return IntersectBox(box, out _) != null;
        }

        /// <summary>
        /// 求射线和三角面的交点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="backfaceCulling"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public TPoint3D? IntersectTriangle(TPoint3D a, TPoint3D b, TPoint3D c, bool backfaceCulling, out TPoint3D target)
        {
            target = new TPoint3D();

            TVector3D _edge1 = (b - a).ToVector();
            TVector3D _edge2 = (c - a).ToVector();
            TVector3D _normal = TVector3D.CrossProduct(_edge1, _edge2);

            // Solve Q + t*D = b1*E1 + b2*E2 (Q = kDiff, D = ray direction,
            // E1 = kEdge1, E2 = kEdge2, N = Perp(E1,E2)) by
            //   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Perp(Q,E2))
            //   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Perp(E1,Q))
            //   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
            double DdN = this.direction.Dot(_normal);
            double sign;

            if (DdN > 0)
            {

                if (backfaceCulling) return null;
                sign = 1;

            }
            else if (DdN < 0)
            {

                sign = -1;
                DdN = -DdN;

            }
            else
            {

                return null;

            }

            //_diff.subVectors(this.origin, a);

            var _diff = this.origin - a;
            double DdQxE2 = sign * this.direction.Dot(TVector3D.CrossProduct(_diff.ToVector(), _edge2));

            // b1 < 0, no intersection
            if (DdQxE2 < 0)
            {

                return null;

            }

            _edge1.Perp(_diff.ToVector());
            double DdE1xQ = sign * this.direction.Dot(_edge1);

            // b2 < 0, no intersection
            if (DdE1xQ < 0)
            {

                return null;

            }

            // b1+b2 > 1, no intersection
            if (DdQxE2 + DdE1xQ > DdN)
            {

                return null;

            }

            // Line intersects triangle, check if ray does.
            double QdN = -sign * _diff.ToVector().Dot(_normal);

            // t < 0, no intersection
            if (QdN < 0)
            {

                return null;

            }

            // Ray intersects triangle.
            return this.At(QdN / DdN, out target);

        }

        public TRay3d applyMatrix4(TMatrix4 matrix4)
        {

            this.origin.ApplyMatrix4(matrix4);
            this.direction.TransformDirection(matrix4);

            return this;

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public TRay3d Clone() => new TRay3d().Copy(this);
        #endregion
    }
}
