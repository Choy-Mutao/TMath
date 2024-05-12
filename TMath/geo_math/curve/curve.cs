

using System;

namespace tmath.geo_math.curve
{
    abstract class TCurve : IEquatable<TCurve>
    {
        public abstract bool Equals(TCurve other);
    }
}
