using System;

namespace tmath.geo_math.curve
{
    public class TEllipse2D
    {
        // center of ellipse
        public TPoint2D Center;

        // 规定主轴最长
        public double MajorAxis;
        public double MinorAxis;

        // 椭圆的标准构造函数
        public TEllipse2D(TPoint2D center, double majoraxis, double minoraxis)
        {
            Center = center; MajorAxis = majoraxis; MinorAxis = minoraxis;
        }


        // 计算椭圆面积
        public double CalculateArea()
        {
            return Math.PI * MajorAxis * MinorAxis;
        }

        // 计算椭圆周长，使用近似公式
        public double CalculateCircumference()
        {
            return 2 * Math.PI * Math.Sqrt((Math.Pow(MajorAxis, 2) + Math.Pow(MinorAxis, 2)) / 2);
        }

        // 计算给定角度上椭圆点的坐标
        public TPoint2D CalculatePointAtAngle(double angle)
        {
            double x = Center.X + MajorAxis * Math.Cos(angle);
            double y = Center.Y + MinorAxis * Math.Sin(angle);
            return new TPoint2D(x, y);
        }

        public TPoint2DCollection Discretize(int number_of_pnts)
        {
            TPoint2DCollection result = new TPoint2DCollection();
            for (int i = 0; i < number_of_pnts; i++)
            {
                double angle = 2 * Math.PI * i / number_of_pnts;
                result.Add(CalculatePointAtAngle(angle));
            }
            return result;

        }

        // 计算椭圆的两个焦点
        public void CalculateFoci(out TPoint2D f1, out TPoint2D f2)
        {
            double c = Math.Sqrt(MajorAxis * MajorAxis - MinorAxis * MinorAxis);
            f1 = new TPoint2D(Center.X - c, Center.Y);
            f2 = new TPoint2D(Center.X + c, Center.Y);
        }
    }
}
