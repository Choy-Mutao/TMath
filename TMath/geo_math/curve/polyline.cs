namespace tmath.geo_math.curve
{
    // 折线, 首尾不相连
    public abstract class APolyline<S, T, V> : TPointCollection<T, V> where S : ISegment where T : IPoint<T, V> where V : IVector<V>
    {
        public void SetPoints(TPointCollection<T, V> pnts) { Clear(); AddRange(pnts); }
    }


    public class TPolyLine2D : APolyline<TLineSegment2d, TPoint2D, TVector2D>
    {
        public TPolyLine2D(TPoint2DCollection pntsollection)
        {
            Clear();
            AddRange(pntsollection);
        }

        public override TPointCollection<TPoint2D, TVector2D> Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void GetBox(out TPoint2D LB, out TPoint2D RT)
        {
            ToTPoint2DCollection().GetBox(out LB, out RT);
        }

        public TPoint2DCollection ToTPoint2DCollection()
        {
            return new TPoint2DCollection(this);
        }

        public void RotateByPoint(double angle, TPoint2D ptBase)
        {
            for (int i = 0; i < Count; i++)
            {
                var pnt = this[i];
                this[i] = pnt.RotateByPoint(angle, ptBase);
            }
        }
    }

    public class TPolyLine3D : APolyline<TLineSegment3d, TPoint3D, TVector3D>
    {
        public TPolyLine3D(TPoint3DCollection pntscollection)
        {
            Clear();
            AddRange(pntscollection);
        }
        public override TPointCollection<TPoint3D, TVector3D> Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void GetBox(out TPoint3D LB, out TPoint3D RT)
        {
            ToTPoint3DCollection().GetBox(out LB, out RT);
        }

        public TPoint3DCollection ToTPoint3DCollection()
        {
            return new TPoint3DCollection(this);
        }
    }
}
