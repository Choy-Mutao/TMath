using System;

namespace tmath
{
    public interface IVector<V> : IEquatable<V> where V : IVector<V>
    {
        //int dimn { get; set; }
        double Length();
        double LengthSq();
        void Normalize();
        double Dot(V v);
        bool IsEqualTo(V p, Tolerance tol);
        bool IsEqualTo(V p, double tol = 1e-3);

        V FromArray(double[] array, int offset = 0);
        double[] ToArray(int offset = 0);

        V Copy(V v);
        V GetNegate();

        V GetNormal();

        V Multiple(int i);
        V Multiple(double d);
        V Multiple(float f);
    }
}
