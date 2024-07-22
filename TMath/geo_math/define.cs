
namespace tmath.geo_math
{
    // Error codes
    public enum TError
    {
        Enot,   // no error
        Edim,   // error: dimn of point invalid for operation
        Esum    // error: sum not affine (cooefs add to 1)
    };

    //enum boolean {FALSE=0, TRUE=1, ERROR=(-1)};
    public enum MATH_BOOLEAN
    {
        MATH_FALSE = 0,
        MATH_TRUE = 1,
        MATH_ERROR = (-1)
    };

    public enum INTER_NUM
    {
        ZERO = 0,
        ONE = 1,
        TWO = 2,
    }

    /// <summary>
    /// 应该分为 2D 几何体 和 3D 几何体
    /// </summary>
    public abstract class TGeometry<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        //public T m_minPnt { set; get; }
        //public T m_maxPnt { set; get; }

        public TBox<T> Box { get; set; }

        //public abstract void CalBox();

        public abstract void GetBox(out T min_pnt, out T max_pnt);

        //public abstract void Move(V move);  

        //public abstract void Rotate(V base_pnt, double angle);
    }

}
