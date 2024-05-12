using tmath.gs.trees;

namespace tmath_lab.algorithm_lab
{
    [TestClass]
    public class AVL_Lab
    {
        [TestMethod("测试 avlcomparable 的比较结果")]
        public void Test_Comparable()
        {
            var c1 = new AVLComparable<int>(1);
            Assert.IsTrue(c1.Key() == 1);
            var c2 = new AVLComparable<int>(2);
            Assert.IsTrue(c2.Key() == 2);
            var c3 = new AVLComparable<int>(3);
            Assert.IsTrue(c3.Key() == 3);

            Assert.IsTrue(c1.Compare(1) == cmp_t.EQ_CMP);
            Assert.IsTrue(c2.Compare(2) == cmp_t.EQ_CMP);
            Assert.IsTrue(c3.Compare(3) == cmp_t.EQ_CMP);

            Assert.IsTrue(c1.Compare(2) == cmp_t.MAX_CMP);
            Assert.IsTrue(c1.Compare(3) == cmp_t.MAX_CMP);
            Assert.IsTrue(c2.Compare(3) == cmp_t.MAX_CMP);

            Assert.IsTrue(c3.Compare(2) == cmp_t.MIN_CMP);
            Assert.IsTrue(c3.Compare(1) == cmp_t.MIN_CMP);
            Assert.IsTrue(c2.Compare(1) == cmp_t.MIN_CMP);
        }

        [TestMethod("测试 avltree 的构建")]
        public void Test_AVLTree()
        {
            AVLTree<int> int_avl_tree = new AVLTree<int>();
            var value_1 = new AVLComparable<int>(1);
            var value_2 = new AVLComparable<int>(2);
            var value_3 = new AVLComparable<int>(3);
            var value_4 = new AVLComparable<int>(4);
            var value_5 = new AVLComparable<int>(5);
            var value_6 = new AVLComparable<int>(6);
            var value_7 = new AVLComparable<int>(7);
            var value_8 = new AVLComparable<int>(8);
            var value_9 = new AVLComparable<int>(9);

            var left = AVLNode<int>.dir_t.LEFT;
            var right = AVLNode<int>.dir_t.RIGHT;

            int_avl_tree.Insert(value_1);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left) == null);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right) == null);

            int_avl_tree.Insert(value_2);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left) == null);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_2);

            int_avl_tree.Insert(value_3);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_3);

            int_avl_tree.Insert(value_4);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_3);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left) == null);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_4);

            int_avl_tree.Insert(value_5);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_4);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left).Data() == value_3);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_5);

            int_avl_tree.Insert(value_6);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_4);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(right).Data() == value_3);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_5);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left) == null);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_6);

            int_avl_tree.Insert(value_7);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_4);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(right).Data() == value_3);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_6);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left).Data() == value_5);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_7);

            int_avl_tree.Insert(value_8);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_4);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(right).Data() == value_3);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_6);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left).Data() == value_5);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_7);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Subtree(left) == null);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Subtree(right).Data() == value_8);

            int_avl_tree.Insert(value_9);
            Assert.IsTrue(int_avl_tree.Check());
            Assert.IsTrue(int_avl_tree.myRoot.Data() == value_4);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Data() == value_2);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(left).Data() == value_1);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(left).Subtree(right).Data() == value_3);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Data() == value_6);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(left).Data() == value_5);

            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Data() == value_8);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Subtree(left).Data() == value_7);
            Assert.IsTrue(int_avl_tree.myRoot.Subtree(right).Subtree(right).Subtree(right).Data() == value_9);
            Assert.IsTrue(true);
        }
    }
}
