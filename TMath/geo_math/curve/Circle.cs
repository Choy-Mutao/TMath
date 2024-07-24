using System;

namespace tmath.geo_math.curve
{
    public abstract class Circle<T, V> : TCurve where T : IPoint<T, V> where V : IVector<V>
    {
        public T Center;
        public double Radius;

        // 对圆进行离散
        public abstract TPointCollection<T, V> Discretize(int number_of_pnts);
    }

    public class TCircle2D : Circle<TPoint2D, TVector2D>
    {
        public TCircle2D(TPoint2D center, double raidus)
        {
            Center = center;
            Radius = raidus;
        }

        public override TPointCollection<TPoint2D, TVector2D> Discretize(int number_of_pnts)
        {
            TPoint2DCollection result = new TPoint2DCollection();
            for (int i = 0; i < number_of_pnts; i++)
                result.Add(GetPointAtTheta(2 * Math.PI * i / number_of_pnts));
            return result;
        }

        public override bool Equals(TCurve other)
        {
            throw new NotImplementedException();
        }

        public TPoint2D GetPointAtTheta(double radian_angle)
        {
            double x = Center.X + Radius * Math.Cos(radian_angle);
            double y = Center.Y + Radius * Math.Sin(radian_angle);
            return new TPoint2D(x, y);
        }

        public void GetTangencyOnDir(TVector2D direcion, out TPoint2D t1, out TPoint2D t2)
        {
            var dir = direcion.GetNormal();
            t1 = Center + Radius * new TVector2D(-dir.Y, dir.X);
            t2 = Center + Radius * new TVector2D(dir.Y, -dir.X);
        }
    }

    public class TCircle3D : Circle<TPoint3D, TVector3D>
    {
        public TVector3D Normal;

        public override TPointCollection<TPoint3D, TVector3D> Discretize(int number_of_pnts)
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(TCurve other)
        {
            throw new NotImplementedException();
        }
    }
}

