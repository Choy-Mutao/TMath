using System;

namespace tmath.geo_math
{
    public class GeometryException : Exception
    {
        TError error;
        string message;

        public GeometryException(TError error, string message) : base(message) { }
    }

    public class GeometryOperationException : Exception { }
}
