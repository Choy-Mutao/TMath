﻿using System;

namespace tmath
{
    public static class NumberUtils
    {
        /// <summary>
        /// 比较 
        /// </summary>
        /// <param abstract_name="a"> a </param>
        /// <param abstract_name="b"> b </param>
        /// <param abstract_name="tol"></param>
        /// ； 如果 a < b, 返回 -1， 如果 a = b 返回 0， 如果 a > b 返回1
        /// <returns></returns>
        public static int CompValue(double a, double b, double tol = 1e-6)
        {
            if ((a - b) < -Math.Abs(tol))
                return -1; // a < b
            else if ((a - b) > Math.Abs(tol))
                return 1; // a > b
            else
                return 0; // a = b
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadianToDegree(double radian)
        {
            return radian / Math.PI * 180;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double DegreeToRadian(double angle)
        {
            return angle * Math.PI / (double)180;
        }

        public static double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
