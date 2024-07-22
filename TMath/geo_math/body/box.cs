using System;
using System.Linq;

namespace tmath.geo_math
{
    [Serializable]
    public abstract class TBox<T>
    {
        public T Min;
        public T Max;

        public TBox() { }
        public TBox(T _min, T _max) { Min = _min; Max = _max; }

        public TBox<T> Copy(TBox<T> _src) { Min = _src.Min; Max = _src.Max; return this; }
        public abstract TBox<T> Clone();
    }

    [Serializable]
    public class TBox2D : TBox<TPoint2D>
    {
        #region Constructor
        public TBox2D()
        {
            Min = new TPoint2D(double.PositiveInfinity, double.PositiveInfinity);
            Max = new TPoint2D(double.NegativeInfinity, double.NegativeInfinity);
        }
        public TBox2D(TPoint2D Min, TPoint2D Max) : base(Min, Max)
        {
        }
        public TBox2D(double min_x, double min_y, double width, double height) : base(new TPoint2D(min_x, min_y), new TPoint2D(min_x + width, min_y + height)) { }

        #endregion

        public override TBox<TPoint2D> Clone() => new TBox2D().Copy(this);
    }

    [Serializable]
    public class TBox3D : TBox<TPoint3D>
    {
        public TBox3D()
        {
            Min = new TPoint3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            Max = new TPoint3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        }

        public TBox3D(TPoint3D Min, TPoint3D Max) : base(Min, Max)
        {
        }

        public TBox3D(double min_x, double min_y, double min_z, double max_x, double max_y, double max_z)
            : base(new TPoint3D(min_x, min_y, min_z), new TPoint3D(max_x, max_y, max_z))
        { }


        #region Private Methods
        private bool SatForAxes(double[] axes, TVector3D v0, TVector3D v1, TVector3D v2, TVector3D extents)
        {

            for (int i = 0, j = axes.Length - 3; i <= j; i += 3)
            {

                //_testAxis.fromArray(axes, i);
                var _testAxis = new TVector3D().FromArray(axes, i);
                // project the aabb onto the separating axis
                double r = extents.X * Math.Abs(_testAxis.X) + extents.Y * Math.Abs(_testAxis.Y) + extents.Z * Math.Abs(_testAxis.Z);
                // project all 3 vertices of the triangle onto the separating axis
                double p0 = v0.Dot(_testAxis);
                double p1 = v1.Dot(_testAxis);
                double p2 = v2.Dot(_testAxis);
                // actual test, basically see if either of the most extreme of the triangle points intersects r
                //if (Math.Max(-Math.Max(p0, p1, p2), Math.Min(p0, p1, p2)) > r)
                if (Math.Max(-new double[3] { p0, p1, p2 }.Max(), new double[3] { p0, p1, p2 }.Min()) > r)
                {

                    // points of the projected triangle are outside the projected half-length of the aabb
                    // the axis is separating and we can exit
                    return false;

                }

            }

            return true;
        }
        #endregion

        /// <summary>
        /// 设置包围盒的距离
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        public TBox3D Set(TPoint3D Min, TPoint3D Max)
        {
            this.Min = Min;
            this.Max = Max;

            return this;
        }

        public override TBox<TPoint3D> Clone() => new TBox3D().Copy(this);

        public bool IsEmpty()
        {
            // this is a more robust check for empty than ( volume <= 0 ) because volume can get positive with two negative axes

            return (this.Max.X < this.Min.X) || (this.Max.Y < this.Min.Y) || (this.Max.Z < this.Min.Z);

        }

        /// <summary>
        /// 获得包围盒的中心点
        /// </summary>
        /// <returns></returns>
        public TPoint3D GetCenter()
        {
            if (IsEmpty()) return new TPoint3D(0, 0, 0);
            else return (Min + Max) * 0.5;
        }


        /// <summary>
        /// 判断剖分三角是否和box相交
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IntersectsTriangle(TTriangle triangle)
        {
            if (IsEmpty()) return false;

            // compute box center and extents
            //this.getCenter(_center);
            var _center = GetCenter();
            //_extents.subVectors(this.Max, _center);
            var _extents = Max - _center;

            // translate triangle to aabb origin
            //_v0.subVectors(triangle.a, _center);
            var _v0 = triangle.a - _center;
            //_v1.subVectors(triangle.b, _center);
            var _v1 = triangle.b - _center;
            //_v2.subVectors(triangle.c, _center);
            var _v2 = triangle.c - _center;

            // compute edge vectors for triangle
            //_f0.subVectors(_v1, _v0);
            var _f0 = _v1 - _v0;
            //_f1.subVectors(_v2, _v1);
            var _f1 = _v2 - _v1;
            //_f2.subVectors(_v0, _v2);
            var _f2 = _v0 - _v2;

            // test against axes that are given by cross product combinations of the edges of the triangle and the edges of the aabb
            // make an axis testing of each of the 3 sides of the aabb against each of the 3 sides of the triangle = 9 axis of separation
            // axis_ij = u_i X f_j (u0, u1, u2 = face normals of aabb = X,Y,Z axes vectors since aabb is axis aligned)
            double[] axes = new double[] {0,
                -_f0.Z,
                _f0.Y,
                0,
                -_f1.Z,
                _f1.Y,
                0,
                -_f2.Z,
                _f2.Y,
                _f0.Z,
                0,
                -_f0.X,
                _f1.Z,
                0,
                -_f1.X,
                _f2.Z,
                0,
                -_f2.X,
                -_f0.Y,
                _f0.X,
                0,
                -_f1.Y,
                _f1.X,
                0,
                -_f2.Y,
                _f2.X,
                0};
            if (!SatForAxes(axes, _v0.ToVector(), _v1.ToVector(), _v2.ToVector(), _extents.ToVector()))
            {

                return false;

            }

            // test 3 face normals from the aabb
            axes = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
            if (!SatForAxes(axes, _v0.ToVector(), _v1.ToVector(), _v2.ToVector(), _extents.ToVector()))
            {

                return false;

            }

            // finally testing the face normal of the triangle
            // use already existing triangle edge vectors here
            var _triangleNormal = TVector3D.CrossProduct(_f0.ToVector(), _f1.ToVector());
            axes = new double[3] { _triangleNormal.X, _triangleNormal.Y, _triangleNormal.Z };

            return SatForAxes(axes, _v0.ToVector(), _v1.ToVector(), _v2.ToVector(), _extents.ToVector());

        }
    }
}
