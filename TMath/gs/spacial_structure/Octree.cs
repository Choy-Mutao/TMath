using System.Collections.Generic;
using System.Linq;
using tmath.geometry;

namespace tmath.gs.spacial_structure
{
    public class Octree
    {
        #region  Fields
        public TBox3D box;
        public TBox3D bounds;
        public List<Octree> subTrees;
        public Stack<TTriangle> triangles;
        #endregion

        #region  Constructor
        public Octree(TBox3D _box)
        {
            box = _box;
            bounds = new TBox3D();

            // initial
            subTrees = new List<Octree>();
            triangles = new Stack<TTriangle>();
        }
        ~Octree() { }
        #endregion

        #region Methods
        /// <summary>
        /// 在 Octree 中增加三角面片
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public Octree AddTriangle(TTriangle triangle)
        {
            TPoint3D bounds_min = bounds.Min, bounds_max = bounds.Max;

            bounds_min.X = new double[4] { bounds_min.X, triangle.a.X, triangle.b.X, triangle.c.X }.Min();
            bounds_min.Y = new double[4] { bounds_min.Y, triangle.a.Y, triangle.b.Y, triangle.c.Y }.Min();
            bounds_min.Z = new double[4] { bounds_min.Z, triangle.a.Z, triangle.b.Z, triangle.c.Z }.Min();

            bounds_max.X = new double[4] { bounds_max.X, triangle.a.X, triangle.b.X, triangle.c.X }.Max();
            bounds_max.Y = new double[4] { bounds_max.Y, triangle.a.Y, triangle.b.Y, triangle.c.Y }.Max();
            bounds_max.Z = new double[4] { bounds_max.Z, triangle.a.Z, triangle.b.Z, triangle.c.Z }.Max();

            bounds.Min = bounds_min;
            bounds.Max = bounds_max;

            triangles.Push(triangle);

            return this;
        }

        public Octree CalcBox()
        {

            box = (TBox3D)bounds.Clone();

            // offset small amount to account for regular grid
            var box_min = box.Min;
            box_min.X -= 0.01;
            box_min.Y -= 0.01;
            box_min.Z -= 0.01;

            box.Min = box_min;

            return this;
        }

        public Octree Split(int level)
        {

            if (box is null) return this;

            // initial data
            List<Octree> _sub_Trees = new List<Octree>();
            TVector3D halfsize = (box.Max - box.Min).ToVector();
            halfsize *= 0.5;

            // 构造四个box
            for (int X = 0; X < 2; X++)
            {
                for (int Y = 0; Y < 2; Y++)
                {
                    for (int Z = 0; Z < 2; Z++)
                    {
                        TVector3D v = new TVector3D(X, Y, Z);

                        TPoint3D box_min = box.Min;
                        v.Multiply(halfsize);
                        box_min += v;
                        TPoint3D box_max = box_min;
                        box_max += halfsize;

                        TBox3D _box = new TBox3D(box_min, box_max);
                        _sub_Trees.Add(new Octree(_box));
                    }
                }
            }


            while (triangles.Count > 0)
            {

                TTriangle triangle = triangles.Pop();

                for (int i = 0; i < _sub_Trees.Count; i++)
                {
                    if (_sub_Trees[i].box.IntersectsTriangle(triangle))
                    {
                        _sub_Trees[i].triangles.Push(triangle);
                    }
                }
            }

            for (int i = 0; i < _sub_Trees.Count; i++)
            {
                int len = _sub_Trees[i].triangles.Count;
                if (len > 8 && level < 16)
                {
                    _sub_Trees[i].Split(level + 1);
                }
                if (len != 0)
                {
                    this.subTrees.Add(_sub_Trees[i]);
                }
            }
            return this;
        }

        public Octree Build()
        {
            CalcBox();
            Split(0);

            return this;
        }

        /// <summary>
        /// 获得和 射线 ray 相交的 所有 triangle
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="triangles"></param>
        public void GetRayTriangles(TRay3d ray, ref List<TTriangle> triangles)
        {
            
            for (int i = 0; i < subTrees.Count; i++)
            {
                var subTree = subTrees[i];
                if (ray.IntersectsBox(subTree.box) == false) continue;
                if (subTree.triangles.Count > 0)
                {
                    for (int j = 0; j < subTree.triangles.Count; j++)
                    {
                        var el_triangle = subTree.triangles.ElementAt(j);
                        if (triangles.IndexOf(el_triangle) == -1) triangles.Add(el_triangle);
                    }
                }
                else
                {
                    subTree.GetRayTriangles(ray, ref triangles);
                }
            }
        }

        /// <summary>
        /// 计算是否相交
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="distance"></param>
        /// <param name="triangle"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool RayIntersect(TRay3d ray, out double distance, out TTriangle triangle, out TPoint3D position)
        {
            distance = 1e100;
            triangle = null;
            position = new TPoint3D();

            if (ray.direction.Length() == 0) return false;
            List<TTriangle> triangles = new List<TTriangle>();
            GetRayTriangles(ray, ref triangles);

            for (int i = 0; i < triangles.Count; i++)
            {
                TPoint3D? result = ray.IntersectTriangle(triangles[i].a, triangles[i].b, triangles[i].c, true, out _);

                if (result != null)
                {

                    double newdistance = result.Value.Sub(ray.origin).ToVector().Length();

                    if (distance > newdistance)
                    {

                        position = (result.Value.Clone() + ray.origin);
                        distance = newdistance;
                        triangle = triangles[i];

                    }

                }
            }
            return distance < 1e100 ? true : false;

        }

        #endregion
    }
}
