using System;

namespace tmath
{
    public struct TQuaternion
    {
        #region  Fields
        public double _x, _y, _z, _w;
        #endregion

        #region  Constructor
        public TQuaternion(double x = 0, double y = 0, double z = 0, double w = 1)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        #endregion

        #region Static Operator
        public static TQuaternion operator- (TQuaternion a, TQuaternion b)
        {
            var x = a._x - b._x;
            var y = a._y - b._y;
            var z = a._z - b._z;
            var w = a._w - b._w;
            return new TQuaternion(x,y,z,w);
        }
        #endregion

        #region Static Methods
        public static TQuaternion SetFromEuler(TEuler euler)
        {
            double x = euler.X, y = euler.Y, z = euler.Z;
            string order = euler.Order;

            // http://www.mathworks.com/matlabcentral/fileexchange/
            // 	20696-function-to-convert-between-dcm-euler-angles-quaternions-and-euler-vectors/
            //	content/SpinCalc.m

            double c1 = Math.Cos(x / 2);
            double c2 = Math.Cos(y / 2);
            double c3 = Math.Cos(z / 2);

            double s1 = Math.Sin(x / 2);
            double s2 = Math.Sin(y / 2);
            double s3 = Math.Sin(z / 2);

            double X = 0, Y = 0, Z = 0, W = 1;
            switch (order)
            {

                case "XYZ":
                    X = s1 * c2 * c3 + c1 * s2 * s3;
                    Y = c1 * s2 * c3 - s1 * c2 * s3;
                    Z = c1 * c2 * s3 + s1 * s2 * c3;
                    W = c1 * c2 * c3 - s1 * s2 * s3;
                    break;

                case "YXZ":
                    X = s1 * c2 * c3 + c1 * s2 * s3;
                    Y = c1 * s2 * c3 - s1 * c2 * s3;
                    Z = c1 * c2 * s3 - s1 * s2 * c3;
                    W = c1 * c2 * c3 + s1 * s2 * s3;
                    break;

                case "ZXY":
                    X = s1 * c2 * c3 - c1 * s2 * s3;
                    Y = c1 * s2 * c3 + s1 * c2 * s3;
                    Z = c1 * c2 * s3 + s1 * s2 * c3;
                    W = c1 * c2 * c3 - s1 * s2 * s3;
                    break;

                case "ZYX":
                    X = s1 * c2 * c3 - c1 * s2 * s3;
                    Y = c1 * s2 * c3 + s1 * c2 * s3;
                    Z = c1 * c2 * s3 - s1 * s2 * c3;
                    W = c1 * c2 * c3 + s1 * s2 * s3;
                    break;

                case "YZX":
                    X = s1 * c2 * c3 + c1 * s2 * s3;
                    Y = c1 * s2 * c3 + s1 * c2 * s3;
                    Z = c1 * c2 * s3 - s1 * s2 * c3;
                    W = c1 * c2 * c3 - s1 * s2 * s3;
                    break;

                case "XZY":
                    X = s1 * c2 * c3 - c1 * s2 * s3;
                    Y = c1 * s2 * c3 - s1 * c2 * s3;
                    Z = c1 * c2 * s3 + s1 * s2 * c3;
                    W = c1 * c2 * c3 + s1 * s2 * s3;
                    break;

                default:
                    Console.Write("THREE.Quaternion: .setFromEuler() encountered an unknown order: " + order);
                    break;

            }
            return new TQuaternion(X, Y, Z, W);
        }

        public static TQuaternion SetFromRotationMatrix(TMatrix4 m)
        {
            TQuaternion result = new TQuaternion();
            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm

            // assumes the upper 3x3 of m is a pure rotation matrix (i.e, unscaled)

            var te = m.elements;

            double  m11 = te[0], m12 = te[4], m13 = te[8],
                    m21 = te[1], m22 = te[5], m23 = te[9],
                    m31 = te[2], m32 = te[6], m33 = te[10],

                    trace = m11 + m22 + m33;

            if (trace > 0)
            {

                double s = 0.5 / Math.Sqrt(trace + 1.0);

                result._w = 0.25 / s;
                result._x = (m32 - m23) * s;
                result._y = (m13 - m31) * s;
                result._z = (m21 - m12) * s;

            }
            else if (m11 > m22 && m11 > m33)
            {

                double s = 2.0 * Math.Sqrt(1.0 + m11 - m22 - m33);

                result._w = (m32 - m23) / s;
                result._x = 0.25 * s;
                result._y = (m12 + m21) / s;
                result._z = (m13 + m31) / s;

            }
            else if (m22 > m33)
            {

                double s = 2.0 * Math.Sqrt(1.0 + m22 - m11 - m33);

                result._w = (m13 - m31) / s;
                result._x = (m12 + m21) / s;
                result._y = 0.25 * s;
                result._z = (m23 + m32) / s;

            }
            else
            {

                double s = 2.0 * Math.Sqrt(1.0 + m33 - m11 - m22);

                result._w = (m21 - m12) / s;
                result._x = (m13 + m31) / s;
                result._y = (m23 + m32) / s;
                result._z = 0.25 * s;

            }
            return result;
        }
        #endregion

        #region Public Methods
        public double Length()
        {
            return Math.Sqrt(this._x * this._x + this._y * this._y + this._z * this._z + this._w * this._w);
        }
        #endregion
    }
}
