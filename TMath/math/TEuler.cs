using System;

namespace tmath
{
    public struct TEuler
    {
        #region  Fields
        private double _x, _y, _z;
        private string _order;

        public const string DEFAULT_ORDER = "XYZ";

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public double Z { get => _z; set => _z = value; }

        public string Order { get => _order; set => _order = value; }
        #endregion

        #region  Constructor
        public TEuler(double x = 0, double y = 0, double z = 0, string order = "XYZ")
        {
            _x = x;
            _y = y;
            _z = z;
            _order = order;
        }
        #endregion

        #region Methods
        public void Set(double x, double y, double z, string order)
        {
            _x = x;
            _y = y;
            _z = z;
            _order = order;
        }

        public TEuler Clone() => new TEuler(_x,_y, _z, _order);

        public void Copy(in TEuler euler)
        {
            _x = euler.X;
            _y = euler.Y;
            _z = euler.Z;
            _order = euler.Order;
        }

        public static TEuler SetFromRotationMatrix(TMatrix4 m, string order = DEFAULT_ORDER)
        {

            // assumes the upper 3x3 of m is a pure rotation matrix (i.e, unscaled)

            var te = m.elements;
            double m11 = te[0], m12 = te[4], m13 = te[8];
            double m21 = te[1], m22 = te[5], m23 = te[9];
            double m31 = te[2], m32 = te[6], m33 = te[10];

            double _x = 0, _y = 0, _z = 0;
            string _order;
            switch (order)
            {

                case "XYZ":

                    _y = Math.Asin(NumberUtils.Clamp(m13, -1, 1));

                    if (Math.Abs(m13) < 0.9999999)
                    {

                        _x = Math.Atan2(-m23, m33);
                        _z = Math.Atan2(-m12, m11);

                    }
                    else
                    {

                        _x = Math.Atan2(m32, m22);
                        _z = 0;

                    }

                    break;

                case "YXZ":

                    _x = Math.Asin(-NumberUtils.Clamp(m23, -1, 1));

                    if (Math.Abs(m23) < 0.9999999)
                    {

                        _y = Math.Atan2(m13, m33);
                        _z = Math.Atan2(m21, m22);

                    }
                    else
                    {

                        _y = Math.Atan2(-m31, m11);
                        _z = 0;

                    }

                    break;

                case "ZXY":

                    _x = Math.Asin(NumberUtils.Clamp(m32, -1, 1));

                    if (Math.Abs(m32) < 0.9999999)
                    {

                        _y = Math.Atan2(-m31, m33);
                        _z = Math.Atan2(-m12, m22);

                    }
                    else
                    {

                        _y = 0;
                        _z = Math.Atan2(m21, m11);

                    }

                    break;

                case "ZYX":

                    _y = Math.Asin(-NumberUtils.Clamp(m31, -1, 1));

                    if (Math.Abs(m31) < 0.9999999)
                    {

                        _x = Math.Atan2(m32, m33);
                        _z = Math.Atan2(m21, m11);

                    }
                    else
                    {

                        _x = 0;
                        _z = Math.Atan2(-m12, m22);

                    }

                    break;

                case "YZX":

                    _z = Math.Asin(NumberUtils.Clamp(m21, -1, 1));

                    if (Math.Abs(m21) < 0.9999999)
                    {

                        _x = Math.Atan2(-m23, m22);
                        _y = Math.Atan2(-m31, m11);

                    }
                    else
                    {

                        _x = 0;
                        _y = Math.Atan2(m13, m33);

                    }

                    break;

                case "XZY":

                    _z = Math.Asin(-NumberUtils.Clamp(m12, -1, 1));

                    if (Math.Abs(m12) < 0.9999999)
                    {

                        _x = Math.Atan2(m32, m22);
                        _y = Math.Atan2(m13, m11);

                    }
                    else
                    {

                        _x = Math.Atan2(-m23, m33);
                        _y = 0;

                    }

                    break;
                default:
                    Console.Write("THREE.TEuler: .setFromRotationMatrix() encountered an unknown order: " + order);
                    break;

            }

            _order = order;
            return new TEuler(_x, _y, _z, order);
        }

        public static TEuler SetFromQuaternion(TQuaternion q, string order = TEuler.DEFAULT_ORDER)
        {
            TMatrix4 _matrix = TMatrix4.MakeRotationFromQuaternion(q);
            return TEuler.SetFromRotationMatrix(_matrix, order);
        }


        public void SetFromVector3(TVector3D v, string order)
        {
            Set(v.X, v.Y, v.Z, order);
        }


        public void Reorder(string newOrder)
        {

            // WARNING: this discards revolution information -bhouston
            TQuaternion _quaternion = TQuaternion.SetFromEuler(this);
            SetFromQuaternion(_quaternion, newOrder);
        }

        public bool Equals(TEuler euler, double tolerance = 1e-3)
        {
            double diff = Math.Abs(_x - euler.X) + Math.Abs(_y - euler.Y) + Math.Abs(_z - euler.Z);
            return (diff < tolerance);
        }

        public bool Equals(TEuler euler)
        {
            return (euler.X == _x) && (euler.Y == _y) && (euler.Z == _z) && (euler.Order == _order);

        }

        public void FromTuple( Tuple<double, double, double, string> array )
        {
            _x = array.Item1;
            _y = array.Item2;
            _z = array.Item3;
            if (array.Item4 != null) _order = array.Item4;
        }

        public Tuple<double, double, double, string> ToTuple()
        {
            return new Tuple<double, double, double, string>(_x, _y, _z, _order);
        }
        #endregion
    }
}
