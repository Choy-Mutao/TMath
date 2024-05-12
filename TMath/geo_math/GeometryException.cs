using System;

namespace tmath.geometry
{
    public class GeometryException : Exception
    {
        TError error;
        string message;

        public GeometryException(TError error, string message) : base(message) { }
    }

    public class GeometryOperationException : Exception { }
}
