using System;

namespace tmath.geo_math.curve
{
    public enum ARC_DIR
    {
        CW = -1,
        CCW = 1,
    }

    public abstract class Arc<T, V> : Circle<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        // range; [0, 2Pi]
        public double start_radian;

        // radian to end
        public double center_radian;

        // cw or ccw
        public ARC_DIR dir;
    }

    public class TArc2D : Arc<TPoint2D, TVector2D>
    {
        public TArc2D(TPoint2D center, double radius, double s_r/*range; [0, 2Pi]*/, double c_r/*range; [0, 2Pi]*/, ARC_DIR _dir = ARC_DIR.CCW)
        {
            Center = center;
            Radius = radius;
            start_radian = s_r;
            center_radian = c_r;
            dir = _dir;
        }

        public override TPointCollection<TPoint2D, TVector2D> Discretize(int number_of_pnts)
        {
            TPoint2DCollection result = new TPoint2DCollection();

            double diff = center_radian / number_of_pnts;

            for (int i = 0; i <= number_of_pnts; i++)
            {
                double angle = start_radian + diff * i * (int)dir;
                double x = Center.X + Radius * Math.Cos(angle);
                double y = Center.Y + Radius * Math.Sin(angle);
                result.Add(new TPoint2D(x, y));
            }
            return result;
        }
    }

    public class TArc3D : Arc<TPoint3D, TVector3D>
    {
        public override TPointCollection<TPoint3D, TVector3D> Discretize(int number_of_pnts)
        {
            throw new System.NotImplementedException();
        }
    }
}
