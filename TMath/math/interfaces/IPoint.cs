using System;

namespace tmath
{
    public interface IPoint<T, V> : IEquatable<T> where T : IPoint<T, V> where V : IVector<V>
    {
        double DistanceTo(T p);
        void Copy(T p);
        void Move(V v);
        void FromArray(double[] array, int offset = 0);
        double[] ToArray(int offset = 0);
        bool IsEqualTo(T p, Tolerance tol);
        bool IsEqualTo(T p, double tol = 1e-3);
        T ProjectOnVector(V vector);
        V ToVector();

        T Add(V v);
        T Add(T t);
        T Sub(T t);
    }

}
