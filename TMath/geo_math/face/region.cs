using Clipper2Lib;
using System;
using System.Collections.Generic;

namespace tmath.geometry
{
    public abstract class TPath<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        public TPointCollection<T, V> contour;
        public bool IsOutSide;
    }

    public class TPath2d : TPath<TPoint2D, TVector2D>
    {
        #region Constructor
        public TPath2d()
        {
            contour = new TPoint2DCollection();
            IsOutSide = true;
        }

        public TPath2d(TPoint2DCollection points)
        {
            contour = new TPoint2DCollection(points);
            //IsOutSide = CommonUtil.PntsArea((TPoint2DCollection)contour) > 0; // todo: 存在退化问题
            IsOutSide = ((TPoint2DCollection)contour).IsClockwise();
        }

        public TPath2d(TPointCollection<TPoint2D, TVector2D> points)
        {
            contour = new TPoint2DCollection(points);
            //IsOutSide = CommonUtil.PntsArea((TPoint2DCollection)contour) > 0; // todo: 存在退化问题
            IsOutSide = ((TPoint2DCollection)contour).IsClockwise();
        }

        public TPath2d(TPath2d path)
        {
            contour = new TPoint2DCollection((TPoint2DCollection)path.contour);
            IsOutSide = path.IsOutSide;
        }
        public TPath2d(TPath<TPoint2D, TVector2D> path)
        {
            contour = new TPoint2DCollection((TPoint2DCollection)path.contour);
            IsOutSide = path.IsOutSide;
        }
        #endregion
    }

    public class TPath3d : TPath<TPoint3D, TVector3D>
    {
        #region Constructor
        public TPath3d()
        {
            contour = new TPoint3DCollection();
            IsOutSide = true;
        }
        public TPath3d(TPoint3DCollection points, bool isOut = true)
        {
            contour = points.Clone();
            IsOutSide = isOut;
        }
        #endregion
    }

    // 面域
    public abstract class TRegion<T, V> : TGeometry<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        #region Fields
        public List<TPath<T, V>> paths;
        public TPlane3D m_plane; // region 所在的平面(也可能时曲面)
        #endregion

        #region Constructor
        public TRegion()
        {
            paths = new List<TPath<T, V>>();
            m_plane = TPlane3D.kXZPlane;
        }
        public TRegion(List<TPath<T, V>> paths)
        {
            this.paths = paths;
            m_plane = TPlane3D.kXZPlane;
        }

        public TRegion(List<TPath<T, V>> paths, TPlane3D plane)
        {
            this.paths = paths;
            m_plane = plane;
        }

        #endregion

        #region Methods
        public abstract TRegion<T, V> AddPath(TPath<T, V> path);
        public abstract TRegion<T, V> AddPath(TPointCollection<T, V> path);
        #endregion
    }

    public class TRegion2d : TRegion<TPoint2D, TVector2D>
    {
        #region Fields
        #endregion

        #region Constructor
        public TRegion2d() : base() { }
        public TRegion2d(TPoint2DCollection point2Ds) : base() =>
            paths.Add(new TPath2d(point2Ds));

        public TRegion2d(TRegion2d region2D) : base() => region2D.paths.ForEach(path => paths.Add(new TPath2d(path)));

        #endregion

        #region Methods
        public override TRegion<TPoint2D, TVector2D> AddPath(TPath<TPoint2D, TVector2D> path)
        {
            if (paths is null) paths = new List<TPath<TPoint2D, TVector2D>>();
            paths.Add(new TPath2d(path));
            return this;
        }

        public override TRegion<TPoint2D, TVector2D> AddPath(TPointCollection<TPoint2D, TVector2D> path) => AddPath(new TPath2d(path));

        public TRegion2d BooleanOper(TRegion2d cregion2, ClipType clipType)
        {
            ClipperD clipperD = new ClipperD();
            paths.ForEach(path =>
            {
                if (path.IsOutSide) clipperD.AddSubject(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
                else clipperD.AddClip(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
            });

            //NOTE: Here is a regular defined by experience;
            cregion2.paths.ForEach(path =>
            {
                if (!path.IsOutSide) clipperD.AddSubject(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
                else clipperD.AddClip(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
            });

            PathsD solution = new PathsD();
            clipperD.Execute(clipType, FillRule.NonZero, solution);

            paths.Clear();
            solution.ForEach(pathd => AddPath(ClipperUtil.ClipperPathDToTPoint2DCollection(pathd)));
            return this;
        }

        public static TRegion2d BooleanOper(TRegion2d r_A, TRegion2d r_B, ClipType clipType)
        {
            ClipperD clipperD = new ClipperD();
            r_A.paths.ForEach(path =>
            {
                if (path.IsOutSide) clipperD.AddSubject(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
                else clipperD.AddClip(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
            });

            //NOTE: Here is a regular defined by experience;
            r_B.paths.ForEach(path =>
            {
                if (!path.IsOutSide) clipperD.AddSubject(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
                else clipperD.AddClip(ClipperUtil.TPoint2DCollectionToClipperPathD(path.contour));
            });

            PathsD solution = new PathsD();
            clipperD.Execute(clipType, FillRule.NonZero, solution);

            TRegion2d result = new TRegion2d();
            solution.ForEach(pathd => result.AddPath(ClipperUtil.ClipperPathDToTPoint2DCollection(pathd)));
            return result;
        }

        public override void GetBox(out TPoint2D min_pnt, out TPoint2D max_pnt)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TRegion3d : TRegion<TPoint3D, TVector3D>
    {
        #region Fields
        #endregion

        #region Constructor
        public TRegion3d() : base() { }
        public TRegion3d(TPoint3DCollection point3Ds) : base(new List<TPath<TPoint3D, TVector3D>>() { new TPath3d(point3Ds) }) { }
        public TRegion3d(TRegion3d region3D) { }

        public override TRegion<TPoint3D, TVector3D> AddPath(TPath<TPoint3D, TVector3D> path)
        {
            throw new NotImplementedException();
        }

        public override TRegion<TPoint3D, TVector3D> AddPath(TPointCollection<TPoint3D, TVector3D> path)
        {
            throw new NotImplementedException();
        }

        public override void GetBox(out TPoint3D min_pnt, out TPoint3D max_pnt)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 空间区域和射线是否相交
        /// </summary>
        /// <param name="ray">指定射线</param>
        /// <param name="inter_pnt">交点</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IntersectWith(TRay3d ray, out TPoint3D inter_pnt)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
