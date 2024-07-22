

using System;

namespace tmath.geo_math.curve
{
    public abstract class TCurve : IEquatable<TCurve>
    {
        public abstract bool Equals(TCurve other);
    }
}
