namespace tmath.geo_math
{
    public class TSphere
    {
        #region  Fields
        private TPoint3D _center = new TPoint3D();
        private double _radius = -1;
        #endregion

        #region  Constructor
        public TSphere(TPoint3D center, double radius)
        {
            _center = center;
            _radius = radius;
        }
        ~TSphere() { }
        #endregion

        #region Methods
        #endregion
    }
}
