namespace tmath
{
    public struct Tolerance
    {
        double _equalVector;
        double _equalPoint;
        public double EqualPoint => _equalPoint;
        public double EqualVector => _equalVector;

        public static Tolerance Global = new Tolerance(1e-6, 1e-6);
        
        public Tolerance(double equalVector, double equalPoint) 
        {
            _equalVector = equalVector;
            _equalPoint = equalPoint;
        }
    }
}
