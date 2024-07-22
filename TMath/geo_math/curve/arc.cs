using System;

namespace tmath.geo_math.curve
{
    public enum ARC_DIR
    {
        CW = -1,
        CCW = 1,
    }

    public abstract class Arc<T, V> : Circle<T, V> where T : IPoint<T, V> where V : IVector<V>
    {
        private double m_startradian;
        // range; [0, 2Pi]
        public double start_radian
        {
            get => m_startradian;
            set => m_startradian = (value + 2 * Math.PI) % (2 * Math.PI);
        }

        public abstract TPoint2D StartPoint { get; }
        public abstract TPoint2D EndPoint { get; }

        // radian to end
        public double center_radian;

        // cw or ccw
        public ARC_DIR dir;
    }

    public class TArc2D : Arc<TPoint2D, TVector2D>
    {

        public override TPoint2D StartPoint
        {
            get
            {
                double startX = Center.X + Radius * Math.Cos(start_radian);
                double startY = Center.Y + Radius * Math.Sin(start_radian);
                return new TPoint2D(startX, startY);
            }
        }

        public override TPoint2D EndPoint
        {
            get
            {
                double endAngle;
                if (dir == ARC_DIR.CW)
                    endAngle = start_radian - center_radian;
                else
                    endAngle = start_radian + center_radian;

                // Normalize the endAngle to be within the range [0, 2π)
                endAngle = endAngle % (2 * Math.PI);
                if (endAngle < 0)
                {
                    endAngle += 2 * Math.PI;
                }

                double endX = Center.X + Radius * Math.Cos(endAngle);
                double endY = Center.Y + Radius * Math.Sin(endAngle);
                return new TPoint2D(endX, endY);
            }
        }


        public TArc2D(TPoint2D center, double radius, double s_r/*range; [0, 2Pi]*/, double c_r/*range; [0, 2Pi]*/, ARC_DIR _dir = ARC_DIR.CCW)
        {
            Center = center;
            Radius = radius;
            start_radian = s_r;
            center_radian = c_r;
            dir = _dir;
        }

        public override TPointCollection<TPoint2D, TVector2D> Discretize(int number_of_pnts)
        {
            TPoint2DCollection result = new TPoint2DCollection();

            double diff = center_radian / number_of_pnts;

            for (int i = 0; i <= number_of_pnts; i++)
            {
                double angle = start_radian + diff * i * (int)dir;
                double x = Center.X + Radius * Math.Cos(angle);
                double y = Center.Y + Radius * Math.Sin(angle);
                result.Add(new TPoint2D(x, y));
            }
            return result;
        }

        public double DistanceToPoint(TPoint2D point, out TPoint2D p2)
        {
            double dx = point.X - Center.X;
            double dy = point.Y - Center.Y;
            double distanceToCenter = point.DistanceTo(Center);
            double angleToPoint = Math.Atan2(dy, dx);

            // Normalize the angleToPoint to be within the range [0, 2pi]
            angleToPoint = angleToPoint % (2 * Math.PI);
            if (angleToPoint < 0)
                angleToPoint += 2 * Math.PI;

            double startAngleNormalized = start_radian % (2 * Math.PI);
            if (startAngleNormalized < 0)
            {
                startAngleNormalized += 2 * Math.PI;
            }

            double endAngle;
            if (dir == ARC_DIR.CW)
            {
                endAngle = startAngleNormalized - center_radian;
            }
            else
            {
                endAngle = startAngleNormalized + center_radian;
            }

            // Normalize the endAngle to be within the range [0, 2π)
            endAngle = endAngle % (2 * Math.PI);
            if (endAngle < 0)
            {
                endAngle += 2 * Math.PI;
            }

            bool isWithinArc = false;
            if (dir == ARC_DIR.CW)
            {
                if (startAngleNormalized > endAngle)
                {
                    isWithinArc = angleToPoint >= endAngle && angleToPoint <= startAngleNormalized;
                }
                else
                {
                    isWithinArc = angleToPoint >= endAngle || angleToPoint <= startAngleNormalized;
                }
            }
            else
            {
                if (startAngleNormalized < endAngle)
                {
                    isWithinArc = angleToPoint >= startAngleNormalized && angleToPoint <= endAngle;
                }
                else
                {
                    isWithinArc = angleToPoint >= startAngleNormalized || angleToPoint <= endAngle;
                }
            }

            if (isWithinArc)
            {
                var p2x = Center.X + Radius * Math.Cos(angleToPoint);
                var p2y = Center.Y + Radius * Math.Sin(angleToPoint);
                p2 = new TPoint2D(p2x, p2y);

                return (double)Math.Abs(distanceToCenter - Radius);
            }
            else
            {
                double distanceToStart = Math.Sqrt((point.X - StartPoint.X) * (point.X - StartPoint.X) + (point.Y - StartPoint.Y) * (point.Y - StartPoint.Y));
                double distanceToEnd = Math.Sqrt((point.X - EndPoint.X) * (point.X - EndPoint.X) + (point.Y - EndPoint.Y) * (point.Y - EndPoint.Y));

                if (distanceToStart < distanceToEnd)
                    p2 = StartPoint;
                else
                    p2 = EndPoint;

                return Math.Min(distanceToStart, distanceToEnd);
            }

        }

        public double DistanceToSegment(TLineSegment2d segment, out TPoint2D pOnArc, out TPoint2D pOnSegment)
        {
            // Initialize output points
            pOnArc = new TPoint2D();
            pOnSegment = new TPoint2D();

            double minDistance = double.MaxValue;

            // Calculate distances to the segment's start and end points
            double distanceToStart = DistanceToPoint(segment.SP, out TPoint2D arcStart);
            double distanceToEnd = DistanceToPoint(segment.EP, out TPoint2D arcEnd);

            // Check if the minimum distance is to the start or end of the segment
            if (distanceToStart < minDistance)
            {
                minDistance = distanceToStart;
                pOnArc = arcStart;
                pOnSegment = segment.SP;
            }

            if (distanceToEnd < minDistance)
            {
                minDistance = distanceToEnd;
                pOnArc = arcEnd;
                pOnSegment = segment.EP;
            }

            // Check the perpendicular distance from the segment to the arc
            double dx = segment.EP.X - segment.SP.X;
            double dy = segment.EP.Y - segment.SP.Y;
            double lengthSquared = dx * dx + dy * dy;

            if (lengthSquared != 0)
            {
                double t = ((Center.X - segment.SP.X) * dx + (Center.Y - segment.SP.Y) * dy) / lengthSquared;
                t = Math.Max(0, Math.Min(1, t));
                double projX = segment.SP.X + t * dx;
                double projY = segment.SP.Y + t * dy;

                double distanceToProj = DistanceToPoint(new TPoint2D(projX, projY), out TPoint2D arcProj);

                if (distanceToProj < minDistance)
                {
                    minDistance = distanceToProj;
                    pOnArc = new TPoint2D(arcProj.X, arcProj.Y);
                    pOnSegment = new TPoint2D(projX, projY);
                }
            }

            return minDistance;
        }

        public bool IsPointOn(in TPoint2D point) => IsPointOn(point, Tolerance.Global);

        public bool IsPointOn(in TPoint2D point, Tolerance tol)
        {
            var tmp_sr = start_radian;
            if (dir == ARC_DIR.CW) tmp_sr = start_radian - center_radian;
            tmp_sr = (tmp_sr + 2 * Math.PI) % (2 * Math.PI);
            double x = Center.X + Radius * Math.Cos(tmp_sr);
            double y = Center.Y + Radius * Math.Sin(tmp_sr);
            var tmp_sp = new TPoint2D(x, y);

            var i = TPoint2D.IsLeft(Center, tmp_sp, point);
            var angle = Math.Acos((tmp_sp - Center).ToVector().GetNormal().Dot((point - Center).ToVector().GetNormal()));
            angle = (i >= 0 ? angle : 2 * Math.PI - angle);

            return NumberUtils.CompValue(point.DistanceTo(Center), Radius, tol.EqualPoint) == 0 &&
                angle <= center_radian;
        }

        public INTER_NUM IntersectWith(TLine2D line, out TPoint2D ip1, out TPoint2D ip2)
        {
            INTER_NUM i = INTER_NUM.ZERO;
            ip1 = TPoint2D.NULL;
            ip2 = TPoint2D.NULL;

            double r = Radius;
            double x0 = Center.X, y0 = Center.Y;
            double x1 = line.P0.X, x2 = line.P1.X;
            double y1 = line.P0.Y, y2 = line.P1.Y;

            double dx = x2 - x1;
            double dy = y2 - y1;

            var a = dx * dx + dy * dy;
            var b = 2 * (dx * (x1 - x0) + dy * (y1 - y0));
            var c = (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0) - r * r;


            double[] t_values;
            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0) // 无解
            {
                t_values = new double[0];
            }
            else if (discriminant == 0)
            {
                t_values = new double[1];
                t_values[0] = -b / 2 * a;
            }
            else
            {
                double sqrt_d = Math.Sqrt(discriminant);
                t_values = new double[2];
                t_values[0] = (-b + sqrt_d) / (2 * a);
                t_values[1] = (-b - sqrt_d) / (2 * a);
            }

            int count = 0;
            if (t_values.Length == 0) { ip1 = TPoint2D.NULL; ip2 = TPoint2D.NULL; }
            else if (t_values.Length == 1)
            {
                var t = t_values[0];
                if (t >= 0 && t <= 1)
                {
                    var x = x1 + t * dx;
                    var y = y1 + t * dy;

                    ip1 = new TPoint2D(x, y);
                    i = INTER_NUM.ONE;

                    if (!IsPointOn(ip1))
                        ip1 = TPoint2D.NULL;
                    else count++;
                }
            }
            else if (t_values.Length == 2)
            {
                double t1 = t_values[0];
                if (t1 >= 0 && t1 <= 1)
                {
                    var x = x1 + t1 * dx;
                    var y = y1 + t1 * dy;
                    ip1 = new TPoint2D(x, y);
                    if (!IsPointOn(ip1))
                        ip1 = TPoint2D.NULL;
                    else count++;
                }

                double t2 = t_values[1];
                if (t2 >= 0 && t2 <= 1)
                {
                    double x = x1 + t2 * dx;
                    double y = y1 + t2 * dy;
                    ip2 = new TPoint2D(x, y);
                    if (!IsPointOn(ip2))
                        ip2 = TPoint2D.NULL;
                    else count++;
                }
            }

            switch (count)
            {
                case 0:
                    i = INTER_NUM.ZERO; break;
                case 1:
                    i = INTER_NUM.ONE; break;
                case 2:
                    i = INTER_NUM.TWO; break;
                default:
                    break;
            }
            return i;
        }
    }

    public class TArc3D : Arc<TPoint3D, TVector3D>
    {
        public override TPoint2D StartPoint => throw new NotImplementedException();

        public override TPoint2D EndPoint => throw new NotImplementedException();

        public override TPointCollection<TPoint3D, TVector3D> Discretize(int number_of_pnts)
        {
            throw new System.NotImplementedException();
        }
    }
}
