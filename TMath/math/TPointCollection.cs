using RTree;
using System;
using System.Collections.Generic;
using System.Linq;
using tmath.geo_math;
using tmath.geo_math.curve;
using tmath.geometry;
using tmath.pga;

namespace tmath
{
    public abstract class TPointCollection<T, V> : List<T> where T : IPoint<T, V> where V : IVector<V>
    {
        public abstract void GetBox(out T LB, out T RT);

        public virtual bool IsClosed()
        {
            if (Count == 0) return false;
            return this.First().IsEqualTo(this.Last());
        }

        public virtual void MakeClosed()
        {
            if (!IsClosed()) Add(this.First()); return;
        }

        public virtual void MakeOpened()
        {
            if (IsClosed()) RemoveAt(Count - 1);
        }

        public virtual bool IsSimple() { throw new NotImplementedException(); }

        public abstract TPointCollection<T, V> Clone();

        public void ReduceVertices() => ReduceVertices(TConstant.Epsilon);

        /// <summary>
        /// 会发生向前的 reduction， 产生模糊的结果， 有可以优化精度的方法
        /// </summary>
        /// <param name="tolerance"></param>
        public void ReduceVertices(double tolerance)
        {
            int start_index = 0;
            int redundant_number = 0;
            for (int i = 1; i < Count; i++)
            {
                var start_vertex = this[start_index];
                if (this[i].IsEqualTo(start_vertex, tolerance)) { redundant_number++; continue; }
                else { this[++start_index] = this[i]; }
            }
            RemoveRange(start_index + 1, redundant_number);
        }

        private double dot(V u, V v) => u.Dot(v);

        private double norm2(V v) => dot(v, v);

        private double d2(T u, T v) => norm2(u.Sub(v).ToVector());

        private double norm(V v) => Math.Sqrt(norm2(v));

        private double d(T u, T v) => Math.Sqrt(d2(u, v));

        /// <summary>
        /// Decimate remove vertices to get a smaller approximate polygon
        /// </summary>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public void Decimate(double tolerance)
        {
            TPointCollection<T, V> @this = this;

            int i, k, m, pv;             // misc counters
            double tol2 = tolerance * tolerance;        // tolerance squared
            T[] vt = new T[Count];       // vertex buffer
            //int mk = new int[Count] = { 0};   // marker  buffer
            int[] mk = new int[Count];
            for (int mk_i = 0; mk_i < Count; mk_i++) mk[mk_i] = 0;

            // STAGE 1.  Vertex Reduction within tolerance of  prior vertex cluster
            vt[0] = this[0];               // start at the beginning
            for (i = k = 1, pv = 0; i < Count; i++)
            {
                if (d2(this[i], this[pv]) < tol2) continue;
                vt[k++] = this[i];
                pv = i;
            }
            if (pv < Count - 1)
                vt[k++] = this[Count - 1];       // finish at the end

            // STAGE 2.  Douglas-Peucker polyline reduction
            mk[0] = mk[k - 1] = 1;       //  mark the first and last vertexes
            DecimateDP(tolerance, 0, k - 1, ref mk);

            // copy marked vertices to the reduced polyline
            for (i = m = 0; i < k; i++)
            {
                if (mk[i] != 0)
                    @this[m++] = vt[i];
            }
            @this.RemoveRange(m, Count - m);
        }

        /// <summary>
        /// This is the Douglas-Peucker recursive reduction routine
        /// It marks vertices that are part of the reduced polyline
        /// for approximating the polyline subchain v[j] to v[k].
        /// 
        /// Note: the collection must be simple
        /// </summary>
        /// <param name="tolerance"></param>
        private void DecimateDP(double tolerance, int j, int k, ref int[] mk)
        {
            if (k <= j + 1) return; // there is nothing to decimate

            // check for adequate approximation by segment S from v[j] to v[k]
            int maxi = j;
            double maxd2 = 0;
            double tol2 = tolerance * tolerance;
            TSegment<T, V> S = new TSegment<T, V>(this[j], this[k]);
            var u = S.Vector();

            //double cu = u.Dot(u);
            double cu = dot(u, u);

            V w;
            T Pb;
            double b, cw, dv2;

            for (int i = j + 1; i < k; i++)
            {
                // compute distance squared
                w = this[i].Sub(S.SP).ToVector();

                //cw = w.Dot(u);
                cw = dot(w, u);
                if (cw <= 0)
                    dv2 = d2(this[i], S.SP);
                else if (cu <= cw)
                    dv2 = d2(this[i], S.EP);
                else
                {
                    b = (double)cw / (double)cu;
                    Pb = S.SP.Add(u.Multiple(b));
                    dv2 = d2(this[i], Pb);
                }
                // test with max distance squared
                if (dv2 <= maxd2)
                    continue;
                // v[i] is a new max vertex
                maxi = i;
                maxd2 = dv2;
            }
            if (maxd2 > tol2)         // error is worse than the tolerance
            {
                // split the polyline at the farthest  vertex from S
                mk[maxi] = 1;       // mark v[maxi] for the reduced polyline
                                    // recursively decimate the two subpolylines at v[maxi]
                DecimateDP(tolerance, j, maxi, ref mk);  // polyline v[j] to v[maxi]
                DecimateDP(tolerance, maxi, k, ref mk);  // polyline v[maxi] to v[k]
            }
            // else the approximation is OK, so ignore intermediate vertexes
            return;
        }
    }

    public class TPoint2DCollection : TPointCollection<TPoint2D, TVector2D>
    {
        public TPoint2DCollection() : base() { }
        public TPoint2DCollection(TPoint2DCollection pnts)
        {
            pnts.ForEach(pnt => Add(pnt));
        }
        public TPoint2DCollection(TPointCollection<TPoint2D, TVector2D> pnts)
        {
            pnts.ForEach(pnt => Add(pnt));
        }

        public TPoint2DCollection(double[][] array2d)
        {
            int points_num = array2d.Length;
            for (int i = 0; i < points_num; i++)
            {
                var item = array2d[i];
                if (item.Length != 2) throw new DataMisalignedException("二维点集的数据项长度错误!");
                Add(new TPoint2D(item[0], item[1]));
            }
        }

        public double Area()
        {
            double area = 0;
            int n = Count;
            for (int p = n - 1, q = 0; q < n; p = q++)
                area += this[p].X * this[q].Y - this[q].X * this[p].Y;
            return area * 0.5;
        }

        public override TPointCollection<TPoint2D, TVector2D> Clone() => new TPoint2DCollection(this);

        public override void GetBox(out TPoint2D LB, out TPoint2D RT)
        {
            double xmin = double.MaxValue, xmax = double.MinValue, ymin = double.MaxValue, ymax = double.MinValue;
            ForEach(item =>
            {
                if (item.X < xmin) xmin = item.X;
                if (item.X > xmax) xmax = item.X;
                if (item.Y < ymin) ymin = item.Y;
                if (item.Y > ymax) ymax = item.Y;
            });
            LB = new TPoint2D(xmin, ymin); RT = new TPoint2D(xmax, ymax);
        }

        public static TPoint2DCollection Random(int number, TBox2D box)
        {
            var res = new TPoint2DCollection();

            double min_x = box.Min.X, min_y = box.Min.Y;
            double length = box.Max.X - box.Min.X, width = box.Max.Y - box.Min.Y;

            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                double x = min_x + random.NextDouble() * length;
                double y = min_y + random.NextDouble() * width;
                res.Add(new TPoint2D(x, y));
            }
            return res;
        }

        public TPoint2DCollection ApplyMtrix3(TMatrix3 m3)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var item = this[i]; item.ApplyMatrix3(m3);
                this[i] = item;
            }
            return this;
        }

        public void XYOrder()
        {
            Sort((a, b) =>
            {
                // test the x-coord first
                if (a.X > b.X) return 1;
                if (a.X < b.X) return (-1);
                // and test the y-coord second
                if (a.Y > b.Y) return 1;
                if (a.Y < b.Y) return (-1);
                // when you exclude all other possibilities, what remains  is...
                return 0;  // they are the same point 
            });
        }

        public bool IsClockwise()
        {
            // 
            double sum = 0;
            if (Count < 3) throw new ArgumentException("Points number less than 3");
            MakeClosed();
            for (int i = 0; i <  Count; i++)
            {
                var o = this[i];
                var a = this[(i + 1) % Count];
                var b = this[(i + 2) % Count];
                sum += (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
            }
            return sum < 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Obsolete("warning: the mount of points can not be more than 100")]
        public TPoint2DCollection ConvexHull()
        {
            int num = ConvexHullApproximation.NearHull_2D(this, Count, out TPoint2DCollection result);
            return result;
        }

        public double ClosestPointTo(TPoint2DCollection poly_2, out TPoint2D pnt_self, out TPoint2D pnt_other) => ClosestPointTo(this, poly_2, out pnt_self, out pnt_other);

        public static double ClosestPointTo(TPoint2DCollection poly_1, TPoint2DCollection poly_2, out TPoint2D pon1, out TPoint2D pon2)
        {
            pon1 = new TPoint2D();
            pon2 = new TPoint2D();
            double min_dist = double.PositiveInfinity;


            RTree<TLineSegment2d> rtree_1 = new RTree<TLineSegment2d>();
            TPoint2DCollection poly_1_copy = new TPoint2DCollection(poly_1); poly_1_copy.MakeClosed();
            for (int i = 0; i < poly_1_copy.Count - 1; i++)
            {
                TLineSegment2d segment1 = new TLineSegment2d(poly_1_copy[i], poly_1_copy[i + 1]);
                rtree_1.Add(segment1, segment1);
            }

            RTree<TLineSegment2d> rtree_2 = new RTree<TLineSegment2d>();
            TPoint2DCollection poly_2_copy = new TPoint2DCollection(poly_2); poly_2_copy.MakeClosed();
            for (int i = 0; i < poly_2_copy.Count - 1; i++)
            {
                TLineSegment2d segment2 = new TLineSegment2d(poly_2_copy[i], poly_2_copy[i + 1]);
                rtree_2.Add(segment2, segment2);
            }

            poly_2_copy.GetBox(out TPoint2D lb2, out TPoint2D rt2);
            var inte1 = rtree_1.Intersects(new Rectangle(lb2.X, lb2.Y, 0, rt2.X, rt2.Y, 0));

            poly_1_copy.GetBox(out TPoint2D lb1, out TPoint2D rt1);
            var inte2 = rtree_2.Intersects(new Rectangle(lb1.X, lb1.Y, 0, rt1.X, rt1.Y, 0));

            for (int i = 0; i < inte1.Count; i++)
            {
                for (int j = 0; j < inte2.Count; j++)
                {
                    var a = inte1[i]; var b = inte2[j];
                    var dist = a.ClosestPointTo(b, out TPoint2D p1, out TPoint2D p2);
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        pon1 = p1;
                        pon2 = p2;
                    }
                }
            }

            return min_dist;
        }

        public void RotateByPoint(double angle, TPoint2D ptBase)
        {
            for (int i = 0; i < Count; i++)
            {
                var pt = this[i].RotateByPoint(angle, ptBase);
                this[i] = pt;
            }
        }
    }

    public class TPoint3DCollection : TPointCollection<TPoint3D, TVector3D>
    {
        public TPoint3DCollection() : base() { }
        public TPoint3DCollection(TPoint3DCollection pnts)
        {
            pnts.ForEach(pnt => Add(pnt));
        }
        public TPoint3DCollection(TPointCollection<TPoint3D, TVector3D> pnts)
        {
            pnts.ForEach(pnt => Add(pnt));
        }

        /// <summary>
        /// 根据集合中存储的点集顺序, 根据右手规则, 计算出点集的法向;
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TVector3D GetNormal()
        {
            throw new NotImplementedException();
        }

        [Obsolete("一般意义上的3D点的几何，在没有空间几何依附的情况下 不具备面积的含义")]
        public double Area()
        {
            MakeClosed();

            TVector3D N = GetNormal();

            double area = 0;
            double an, ax, ay, az; // abs value of normal and its coords
            int coord;           // coord to ignore: 1=X, 2=Y, 3=Z
            int i, j, k;         // loop indices

            int n = Count;
            if (n < 3) return 0;  // a degenerate polygon

            // select largest abs coordinate to ignore for projection
            ax = (N.X > 0 ? N.X : -N.X);    // abs X-coord
            ay = (N.Y > 0 ? N.Y : -N.Y);    // abs Y-coord
            az = (N.Z > 0 ? N.Z : -N.Z);    // abs Z-coord

            coord = 3;                    // ignore Z-coord
            if (ax > ay)
            {
                if (ax > az) coord = 1;   // ignore X-coord
            }
            else if (ay > az) coord = 2;  // ignore Y-coord

            // compute area of the 2D projection
            switch (coord)
            {
                case 1:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (this[i].Y * (this[j].Z - this[k].Z));
                    break;
                case 2:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (this[i].Z * (this[j].X - this[k].X));
                    break;
                case 3:
                    for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                        area += (this[i].X * (this[j].Y - this[k].Y));
                    break;
            }
            switch (coord)
            {    // wrap-around term
                case 1:
                    area += (this[n].Y * (this[1].Z - this[n - 1].Z));
                    break;
                case 2:
                    area += (this[n].Z * (this[1].X - this[n - 1].X));
                    break;
                case 3:
                    area += (this[n].X * (this[1].Y - this[n - 1].Y));
                    break;
            }

            // scale to get area before projection
            an = Math.Sqrt(ax * ax + ay * ay + az * az); // length of normal vector
            switch (coord)
            {
                case 1:
                    area *= (an / (2 * N.X));
                    break;
                case 2:
                    area *= (an / (2 * N.Y));
                    break;
                case 3:
                    area *= (an / (2 * N.Z));
                    break;
            }
            return area;
        }

        public override TPointCollection<TPoint3D, TVector3D> Clone() => new TPoint3DCollection(this);

        public override void GetBox(out TPoint3D LB, out TPoint3D RT)
        {
            double xmin = double.MaxValue, xmax = double.MinValue, ymin = double.MaxValue, ymax = double.MinValue, zmin = double.MaxValue, zmax = double.MinValue;
            ForEach(item =>
            {
                if (item.X < xmin) xmin = item.X;
                if (item.X > xmax) xmax = item.X;
                if (item.Y < ymin) ymin = item.Y;
                if (item.Y > ymax) ymax = item.Y;
                if (item.Z < zmin) zmin = item.Z;
                if (item.Z > zmax) zmax = item.Z;
            });
            LB = new TPoint3D(xmin, ymin, zmin); RT = new TPoint3D(xmax, ymax, zmax);
        }

    }
}
