using tmath;
using tmath.geometry;


namespace tmath_lab.Geo_Lab
{
    [TestClass]
    public class Segment_Lab
    {
        [TestMethod("判断点是否在线段上")]
        public void Test_PointOnSegment()
        {
            TLineSegment2d segment2d = new TLineSegment2d(new TPoint2D(0, 0), new TPoint2D(10, 10));
            Assert.IsTrue(segment2d.IsPointOn(new TPoint2D(5, 5)));
            Assert.IsFalse(segment2d.IsPointOn(new TPoint2D(3, 4)));
        }

        [TestMethod("测试两个线段的相交")]
        public void Test_SegmentIntersectWithSegment()
        {
            TLineSegment2d segment1, segment2;
            TPoint2D p1, p2;
            //案例 1: 两线段相交
            //线段1: (1, 1) 到(4, 4)
            //线段2: (1, 4) 到(4, 1)
            //预期结果: 线段相交于点(2.5, 2.5)
            segment1 = new TLineSegment2d(1, 1, 4, 4);
            segment2 = new TLineSegment2d(1, 4, 4, 1);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ONE);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(2.5, 2.5)));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 2: 两线段端点相交
            //线段1: (0, 0) 到(3, 3)
            //线段2: (3, 3) 到(5, 5)
            //预期结果: 线段相交于点(3, 3)
            segment1 = new TLineSegment2d(0, 0, 3, 3);
            segment2 = new TLineSegment2d(3, 3, 5, 5);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ONE);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(3, 3)));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 3: 两线段不相交
            //线段1: (0, 0) 到(2, 2)
            //线段2: (3, 3) 到(4, 4)
            //预期结果: 线段不相交
            segment1 = new TLineSegment2d(0, 0, 2, 2);
            segment2 = new TLineSegment2d(3, 3, 4, 4);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ZERO);
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 4: 两线段平行不相交
            //线段1: (0, 0) 到(5, 5)
            //线段2: (0, 1) 到(5, 6)
            //预期结果: 线段平行，不相交
            segment1 = new TLineSegment2d(0, 0, 5, 5);
            segment2 = new TLineSegment2d(0, 1, 5, 6);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ZERO);
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 5: 两线段重合（部分重叠）
            //线段1: (2, 2) 到(5, 5)
            //线段2: (4, 4) 到(6, 6)
            //预期结果: 线段重合
            segment1 = new TLineSegment2d(2, 2, 5, 5);
            segment2 = new TLineSegment2d(4, 4, 6, 6);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.TWO);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(4, 4)));
            Assert.IsTrue(p2.IsEqualTo(new TPoint2D(5, 5)));

            //案例 6: 两线段垂直相交
            //线段1: (2, 2) 到(2, 5)
            //线段2: (0, 3) 到(4, 3)
            //预期结果: 线段相交于点(2, 3)
            segment1 = new TLineSegment2d(2, 2, 2, 5);
            segment2 = new TLineSegment2d(0, 3, 4, 3);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ONE);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(2, 3)));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 7: 线段交于端点
            //线段1: (0, 0) 到(4, 0)
            //线段2: (4, 0) 到(4, 4)
            //预期结果: 线段相交于点(4, 0)
            segment1 = new TLineSegment2d(0, 0, 4, 0);
            segment2 = new TLineSegment2d(4, 0, 4, 4);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.ONE);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(4, 0)));
            Assert.IsTrue(p2.IsEqualTo(TPoint2D.NULL));

            //案例 8: 线段完全重合
            //线段1: (1, 1) 到(3, 3)
            //线段2: (1, 1) 到(3, 3)
            //预期结果: 线段完全重合
            segment1 = new TLineSegment2d(1, 1, 3, 3);
            segment2 = new TLineSegment2d(1, 1, 3, 3);
            Assert.IsTrue(segment1.IntersectWith(segment2, out p1, out p2) == INTER_NUM.TWO);
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(1, 1)));
            Assert.IsTrue(p2.IsEqualTo(new TPoint2D(3, 3)));
        }


        [TestMethod("测试线段和直线的相交")]
        public void Test_SegmentIntersectWithLine()
        {
            TLine2D line; TLineSegment2d segment;
            TPoint2D p0, p1;
            //案例 1: 线段与直线相交
            //直线方程: y = 2x + 1 
            //选择的两点: A(0,1)，B(1, 3)
            //线段: (1, 5) 到(6, 9)
            //预期结果: 线段与直线相交于点(2.67, 6.33)
            line = new TLine2D(0, 1, 1, 3);
            segment = new TLineSegment2d(1, 5, 6, 9);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ONE);
            Assert.IsTrue(p0.IsEqualTo(new TPoint2D(2.67, 6.33), 0.01));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));

            //案例 2: 线段端点在直线上
            //直线方程: y = x + 2
            //选择的两点: A(0,2)，B(1, 3)
            //线段: (2, 4) 到(3, 6)
            //预期结果: 线段的一个端点(2, 4) 在直线上
            line = new TLine2D(0, 2, 1, 3);
            segment = new TLineSegment2d(2, 4, 3, 6);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ONE);
            Assert.IsTrue(p0.IsEqualTo(new TPoint2D(2, 4)));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));

            //案例 3: 线段与直线平行（不重合）
            //直线方程: y = 3x + 4
            //选择的两点: A(0,4)，B(1, 7)
            //线段: (2, 7) 到(3, 10)
            //预期结果: 线段与直线平行，不相交
            line = new TLine2D(0, 4, 1, 7);
            segment = new TLineSegment2d(2, 7, 3, 10);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ZERO);
            Assert.IsTrue(p0.IsEqualTo(TPoint2D.NULL));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));


            //案例 4: 线段完全在直线上(重合)
            //直线方程: y = 0.5x + 1
            //选择的两点: A(0,1)，B(2, 2)
            //线段: (2, 2) 到(4, 3)
            //预期结果: 线段完全位于直线上
            line = new TLine2D(0, 1, 2, 2);
            segment = new TLineSegment2d(2, 2, 4, 3);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.TWO);
            Assert.IsTrue(p0.IsEqualTo(new TPoint2D(2, 2)));
            Assert.IsTrue(p1.IsEqualTo(new TPoint2D(4, 3)));

            //案例 5: 线段与直线相交，交点非端点
            //直线方程: y =−x + 4
            //选择的两点: A(0,4)，B(2, 2)
            //线段: (1, 1) 到(4, 4)
            //预期结果: 线段与直线相交于点(2, 2)
            line = new TLine2D(0, 4, 2, 2);
            segment = new TLineSegment2d(1, 1, 4, 4);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ONE);
            Assert.IsTrue(p0.IsEqualTo(new TPoint2D(2, 2)));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));

            //案例 6: 线段横跨直线但不相交（端点均在直线同侧）
            //直线方程: y = 2x - 1y = 2x−1
            //选择的两点: A(0,-1)，B(1, 1)
            //线段: (0, 3) 到(1, 4)
            //预期结果: 虽然线段在直线的上方，但由于直线斜率的关系，线段与直线不相交
            line = new TLine2D(0, -1, 1, 1);
            segment = new TLineSegment2d(0, 3, 1, 4);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ZERO);
            Assert.IsTrue(p0.IsEqualTo(TPoint2D.NULL));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));

            //案例 7: 线段与垂直直线相交
            //直线方程: x = 3x = 3
            //选择的两点: A(3,1)，B(3, 3)
            //线段: (2, 2) 到(4, 4)
            //预期结果: 线段与垂直直线相交于点(3, 3)
            line = new TLine2D(3, 1, 3, 3);
            segment = new TLineSegment2d(2, 2, 4, 4);
            Assert.IsTrue(segment.IntersectWith(line, out p0, out p1) == INTER_NUM.ONE);
            Assert.IsTrue(p0.IsEqualTo(new TPoint2D(3, 3)));
            Assert.IsTrue(p1.IsEqualTo(TPoint2D.NULL));

        }
    }

    [TestClass]
    public class Line_Lab
    {
        [TestMethod]
        public void Test_Line2d_IsPonitOn()
        {
            TPoint2D on_pnt, non_pnt;
            // 水平
            TLine2D horizontal_line = new TLine2D(new TPoint2D(2, 5), new TPoint2D(6, 5));
            on_pnt = new TPoint2D(3, 5);
            non_pnt = new TPoint2D(-2, 4);
            Assert.IsTrue(horizontal_line.IsPointOn(on_pnt));
            Assert.IsFalse(horizontal_line.IsPointOn(non_pnt));

            // 垂直
            TLine2D vertical_line = new TLine2D(new TPoint2D(4, 2), new TPoint2D(4, 6));
            on_pnt = new TPoint2D(4, -1);
            non_pnt = new TPoint2D(5, 5);
            Assert.IsTrue(vertical_line.IsPointOn(on_pnt));
            Assert.IsFalse(vertical_line.IsPointOn(non_pnt));

            // 任意
            TLine2D arbitrary_line = new TLine2D(new TPoint2D(1, 2), new TPoint2D(3, 3));
            on_pnt = new TPoint2D(2, 2.5);
            non_pnt = new TPoint2D(1, 1);
            Assert.IsTrue(arbitrary_line.IsPointOn(on_pnt));
            Assert.IsFalse(arbitrary_line.IsPointOn(non_pnt));

        }

        [TestMethod]
        public void Test_LineIntersect()
        {

            // 案例 1: 两条直线相交
            // 直线1: 通过点(0, 0) 和(2, 2)
            // 直线2: 通过点(0, 2) 和(2, 0)
            // 预期结果: 返回 true, inter_pnt 应该是(1, 1)
            TLine2D line1 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(2, 2));
            TLine2D line2 = new TLine2D(new TPoint2D(0, 2), new TPoint2D(2, 0));
            TPoint2D intersection_point;

            var isintersect = line1.IntersectWith(line2, out intersection_point);
            Assert.IsTrue(isintersect == INTER_NUM.ONE);
            Assert.IsTrue(intersection_point.IsEqualTo(new TPoint2D(1, 1)));
            // 案例 2: 两条直线平行不相交
            // 直线1: 通过点(0, 0) 和(2, 2)
            // 直线2: 通过点(0, 1) 和(2, 3)
            // 预期结果: 返回 false
            line1 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(2, 2));
            line2 = new TLine2D(new TPoint2D(0, 1), new TPoint2D(2, 3));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ZERO);
            Assert.IsTrue(intersection_point == TPoint2D.NULL);

            // 案例 3: 两条直线重合
            // 直线1: 通过点(0, 0) 和(3, 3)
            // 直线2: 通过点(1, 1) 和(2, 2)
            // 预期结果: 返回 true, inter_pnt 可以是任何在这两条直线上的点，但具体取决于算法设计

            line1 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(3, 3));
            line2 = new TLine2D(new TPoint2D(1, 1), new TPoint2D(2, 2));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ZERO);
            Assert.IsTrue(intersection_point == TPoint2D.NULL);
            // 案例 4: 两条直线垂直相交
            // 直线1: 通过点(0, 0) 和(0, 4)
            // 直线2: 通过点(0, 2) 和(2, 2)
            // 预期结果: 返回 true, inter_pnt 应该是(0, 2)

            line1 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(0, 4));
            line2 = new TLine2D(new TPoint2D(0, 2), new TPoint2D(2, 2));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ONE);
            Assert.IsTrue(intersection_point.IsEqualTo(new TPoint2D(0, 2)));
            // 案例 5: 两条直线有相同的斜率但不重合
            // 直线1: 通过点(0, 0) 和(2, 2)
            // 直线2: 通过点(0, 1) 和(2, 3)
            // 预期结果: 返回 false

            line1 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(2, 2));
            line2 = new TLine2D(new TPoint2D(0, 1), new TPoint2D(2, 3));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ZERO);
            Assert.IsTrue(intersection_point == TPoint2D.NULL);
            // 案例 6: 其中一条直线垂直
            // 直线1: 通过点(2, 0) 和(2, 4)
            // 直线2: 通过点(0, 0) 和(4, 4)
            // 预期结果: 返回 true, inter_pnt 应该是(2, 2)

            line1 = new TLine2D(new TPoint2D(2, 0), new TPoint2D(2, 4));
            line2 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(4, 4));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ONE);
            Assert.IsTrue(intersection_point.IsEqualTo(new TPoint2D(2, 2)));
            // 案例 7: 两条直线相交于原点
            // 直线1: 通过点(-1, 1) 和(1, -1)
            // 直线2: 通过点(-1, -1) 和(1, 1)
            // 预期结果: 返回 true, inter_pnt 应该是(0, 0)

            line1 = new TLine2D(new TPoint2D(-1, 1), new TPoint2D(1, -1));
            line2 = new TLine2D(new TPoint2D(-1, -1), new TPoint2D(1, 1));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ONE);
            Assert.IsTrue(intersection_point.IsEqualTo(new TPoint2D(0, 0)));

            // 案例 8: 两条直线交叉
            // 直线 1: 通过点(0,1)，(1,3)
            // 直线 2: 通过点(1,5), (4,10)
            // 预期结果: 返回 true, inter_pnt 应该是 (2.67, 6.33)
            line1 = new TLine2D(new TPoint2D(0, 1), new TPoint2D(1, 3));
            line2 = new TLine2D(new TPoint2D(1, 5), new TPoint2D(4, 10));
            Assert.IsTrue(line1.IntersectWith(line2, out intersection_point) == INTER_NUM.ONE);
            Assert.IsTrue(intersection_point.IsEqualTo(new TPoint2D(7, 15), 0.01));


            // 特殊案例: 考虑浮点数精度问题
            // 直线1: 通过点(0.000001, 0.000001) 和(2.000001, 2.000001)
            // 直线2: 通过点(0, 0) 和(2, 2)
            // 预期结果: 根据实现细节，这可能返回 true 或 false。如果算法考虑了浮点数精度问题，则可能认为这两条直线重合。

            line1 = new TLine2D(new TPoint2D(0.000001, 0.000001), new TPoint2D(2.000001, 2.000001));
            line2 = new TLine2D(new TPoint2D(0, 0), new TPoint2D(2, 2));

        }
    }
}
