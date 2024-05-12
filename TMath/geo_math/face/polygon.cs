namespace tmath.geometry
{
    // 多边形, 首尾相连
    public abstract class APolygon<S> where S : ISegment
    {
        //public List<TPoint> V { get; SetByRowElements; }
        //public int n { get; SetByRowElements; }
        //public APolygon()
        //{
        //    V = new List<TPoint>();
        //    n = V.Count;
        //}
        //public APolygon(List<TPoint> v)
        //{
        //    V = v;
        //    this.n = v.Count;
        //    CalBox();
        //}

        //public override void CalBox()
        //{
        //    TPoint min = new TPoint(), max = new TPoint();
        //    GeometryUtil.CommonUtil.Cal2dAABBox(V, ref min, ref max);
        //    m_minPnt = min;
        //    m_maxPnt = max;
        //}

        //public override void Move(TVector move)
        //{
        //    //throw new NotImplementedException();
        //    V.ForEach(item => item.Move(move));
        //}

        //public override void Rotate(TPoint base_pnt, double angle)
        //{
        //    V.ForEach(item => item.Rotate(base_pnt, angle));
        //}

        //public void SetPoints(List<TPoint> ptArr, bool bNormal = false)
        //{
        //    V = new List<TPoint>(ptArr);
        //    if (bNormal) { Normalize(); }
        //    CalBox();
        //}

        //public List<TPoint> GetPoints() { return new List<TPoint>(V); }

        //public double area
        //{
        //    get { return GeometryUtil.CommonUtil.PntsArea(V); }
        //}

        //public void Normalize()
        //{
        //    if (V.Count < 1) { return; }
        //    if (GeometryUtil.NumberUtil.CompValue(area, 0.05) == -1) return;
        //    // 去除重复点

        //    for (int i = 0; i + 1 < V.Count; i++)
        //    {
        //        if (V[i].DistanceTo(V[i + 1]) < 0.001)//精度从0.01修改为0.001
        //        {
        //            V.RemoveAt(i + 1);
        //            i--;
        //        }
        //    }
        //    if (V.First().IsEqualTo(V.Last(), 1e-5) && V.Count > 3)
        //        V.RemoveAt(V.Count - 1);
        //    if (GeometryUtil.CommonUtil.isClockwise(V))
        //        V.Reverse();
        //}

        //public TVector GetTagentAt(int vid)
        //{
        //    if (V.Count < 2) { return null; }
        //    else if (V.Count == 2) { return V[1] - V[0]; }
        //    else
        //    {
        //        int pid = vid - 1 < 0 ? V.Count - 1 : vid - 1;
        //        int nid = (vid + 1) % V.Count;
        //        //TODO: 如果有重复点怎么办
        //        TVector a = V[vid] - V[pid];
        //        TVector b = V[nid] - V[vid];
        //        a.Normalize(); b.Normalize();
        //        return a + b;
        //    }
        //}

        /// <summary>
        /// 多边形的面积
        /// </summary>
        /// <returns></returns>
        public abstract double Area();
    }

    public class TPolygon2D : APolygon<TLineSegment2d>
    {
        public override double Area()
        {
            throw new System.NotImplementedException();
        }
    }

    public class TPolygon3D : APolygon<TLineSegment3d>
    {
        public override double Area()
        {
            throw new System.NotImplementedException();
        }
    }

}
