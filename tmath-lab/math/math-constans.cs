using tmath;

namespace tmath_lab.math
{
    public static class math_constans
    {
        #region  Fields

        public const double x = 2;
        public const double y = 2;
        public const double z = 2;
        public const double w = 2;

        public static TPoint2D pnt_negInf2 = new TPoint2D(double.NegativeInfinity, double.NegativeInfinity);
        public static TPoint2D pnt_posInf2 = new TPoint2D(double.PositiveInfinity, double.PositiveInfinity);


        public static TPoint2D pnt_negOne2 = new TPoint2D(-1, -1);

        public static TPoint2D pnt_zero2 = new TPoint2D();
        public static TPoint2D pnt_one2 = new TPoint2D(1, 1);
        public static TPoint2D pnt_two2 = new TPoint2D(2, 2);


        public static TVector2D vec_negInf2 = new TVector2D(double.NegativeInfinity, double.NegativeInfinity);
        public static TVector2D vec_posInf2 = new TVector2D(double.PositiveInfinity, double.PositiveInfinity);


        public static TVector2D vec_negOne2 = new TVector2D(-1, -1);

        public static TVector2D vec_zero2 = new TVector2D();
        public static TVector2D vec_one2 = new TVector2D(1, 1);
        public static TVector2D vec_two2 = new TVector2D(2, 2);


        //public static TPoint3D pnt_negInf3 = new TPoint3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        //public static TPoint3D pnt_posInf3 = new TPoint3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public static TPoint3D pnt_negInf3 = TPoint3D.NegativeInfinity;
        public static TPoint3D pnt_posInf3 = TPoint3D.PositiveInfinity;

        public static TPoint3D pnt_zero3 = new TPoint3D();
        public static TPoint3D pnt_one3 = new TPoint3D(1, 1, 1);
        public static TPoint3D pnt_two3 = new TPoint3D(2, 2, 2);

        public static TVector3D vec_negInf3 = new TVector3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        public static TVector3D vec_posInf3 = new TVector3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

        public static TVector3D vec_zero3 = new TVector3D();
        public static TVector3D vec_one3 = new TVector3D(1, 1, 1);
        public static TVector3D vec_two3 = new TVector3D(2, 2, 2);

        public const double eps = 0.0001;

        #endregion
    }
}
