namespace tmath.geo_math.bounding
{
    public class Ball<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        public T center;
        public double radius;
        public Ball(T center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}
