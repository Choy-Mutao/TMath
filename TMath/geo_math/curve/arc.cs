using RTree;
using System;
using tmath.geometry;

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
            set
            {
                if (value < 0 || value >= 2 * Math.PI)
                    throw new ArgumentOutOfRangeException(nameof(start_radian), "Start Angle(Radian) must be in the range [0, 2Π]");
                m_startradian = value;
            }
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
            double angleToPoint = Math.Atan2(dx, dy);

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
                pOnSegment = segment.SP;
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
