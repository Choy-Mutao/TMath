using System;

namespace tmath.gs.trees
{
    public class AVLComparable<KeyType> where KeyType : IComparable<KeyType>
    {
        protected KeyType myKey;

        public AVLComparable() { }
        public AVLComparable(KeyType key) { myKey = key; }
        ~AVLComparable() { }

        // Compare this item against the given key & return the result;
        public virtual cmp_t Compare(KeyType key)
        {
            return key.Equals(myKey) ? cmp_t.EQ_CMP : ((key.CompareTo(myKey) < 0) ? cmp_t.MIN_CMP : cmp_t.MAX_CMP);
        }

        // Get the key-field for an item
        public KeyType Key() => myKey;

    }

    public class AVLNode<KeyType> where KeyType : IComparable<KeyType>
    {
        private const int MAX_SUBTREES = 2;

        public enum dir_t { LEFT = 0, RIGHT = 1 };

        public static dir_t Opposite(dir_t dir) => (dir_t)((1 - (int)dir));

        // Constructors and Destructors;
        public AVLNode(AVLComparable<KeyType> item = null)
        {
            myData = item;
            //myBal = 0;
            Reset();
        }
        ~AVLNode()
        {

        }

        // Get this node's data
        public AVLComparable<KeyType> Data() => myData;

        // Get this node's  key field
        public KeyType Key() => myData.Key();

        // Query the balance factor, it will be a value between -1 .. 1
        // where:
        //     -1 => left subtree is taller than right subtree
        //      0 => left and right subtree are equal in height
        //      1 => right subtree is taller than left subtree
        public int Bal() => myBal;

        // Get the item at the top of the left/right subtree of this item (the result may be NULL if there is no such item).
        public AVLNode<KeyType> Subtree(dir_t dir) => mySubtree[(int)dir];

        public AVLNode<KeyType> Parent(AVLNode<KeyType> myRoot)
        {
            if (this == myRoot) return null;

            AVLNode<KeyType> p = myRoot;
            while (p != null)
            {
                if (this == p.Subtree(dir_t.LEFT)) return p;
                if (this == p.Subtree(dir_t.RIGHT)) return p;

                cmp_t result = p.Compare(this.Key());
                if (result == cmp_t.MIN_CMP)
                    p = p.Subtree(dir_t.LEFT);
                else
                    p = p.Subtree(dir_t.RIGHT);
            }
            return null;
        }

        // ----- Search/Insert/Delete
        //
        //   NOTE: These are all static functions instead of member functions
        //         because most of them need to modify the given tree root
        //         pointer. If these were instance member functions than
        //         that would correspond to having to modify the 'this'
        //         pointer, which is not allowed in C++. Most of the
        //         functions that are static and which take an AVL tree
        //         pointer as a parameter are static for this reason.

        // Look for the given key, return NULL if not found,
        // otherwise return the item's address.
        public static AVLNode<KeyType> Search(KeyType key, AVLNode<KeyType> root, cmp_t cmp = cmp_t.EQ_CMP)
        {
            cmp_t result;
            while (root != null && (result = root.Compare(key, cmp)) > 0)
            {
                root = root.mySubtree[(result < 0) ? (int)dir_t.LEFT : (int)dir_t.RIGHT];
            }
            return root ?? null;
        }

        // Insert the given key, return a pointer to the node if it was inserted,
        // otherwise return NULL
        public static AVLNode<KeyType> Insert(AVLComparable<KeyType> item, ref AVLNode<KeyType> root)
        {
            height_effect_t change = height_effect_t.HEIGHT_NOCHANGE;
            return Insert(item, ref root, ref change);
        }

        // Delete the given key from the tree. Return the corresponding
        // node, or return NULL if it was not found.
        public static AVLComparable<KeyType> Delete(KeyType key, ref AVLNode<KeyType> root, cmp_t cmp = cmp_t.EQ_CMP)
        {
            height_effect_t change = height_effect_t.HEIGHT_NOCHANGE;
            return Delete(key, ref root, ref change, cmp);
        }

        // Verification

        // Return the height of this tree
        public int Height()
        {
            int leftHeight = (mySubtree[(int)dir_t.LEFT]) != null ? mySubtree[(int)dir_t.LEFT].Height() : 0;
            int rightHeight = (mySubtree[(int)dir_t.RIGHT]) != null ? mySubtree[(int)dir_t.RIGHT].Height() : 0;
            return (1 + Math.Min(leftHeight, rightHeight));
        }

        // Verify this tree is a valid AVL tree, return TRUE if it is,
        // return FALSE otherwise
        public int Check()
        {
            int valid = 1;

            // First verify that subtrees are correct
            if (mySubtree[(int)dir_t.LEFT] != null) valid *= mySubtree[(int)dir_t.LEFT].Check();
            if (mySubtree[(int)dir_t.RIGHT] != null) valid *= mySubtree[(int)dir_t.RIGHT].Check();

            // Now get the height of each subtree
            int leftHeight = (mySubtree[(int)dir_t.LEFT]) != null ? mySubtree[(int)dir_t.LEFT].Height() : 0;
            int rightHeight = (mySubtree[(int)dir_t.RIGHT] != null) ? mySubtree[(int)dir_t.RIGHT].Height() : 0;

            // Verify that AVL tree property is satisfied
            int diffHeight = rightHeight - leftHeight;
            if (LEFT_IMBALANCE(diffHeight) || RIGHT_IMBALANCE(diffHeight))
            {
                valid = 0;
                Console.Error.WriteLine("Height difference is " + diffHeight
                     + " at node " + Key());
            }

            // Verify that balance-factor is correct
            if (diffHeight != myBal)
            {
                valid = 0;
                Console.Error.WriteLine("Height difference " + diffHeight
                     + " doesn't match balance-factor of " + myBal
                     + " at node " + Key());
            }

            // Verify that search-tree property is satisfied
            if ((mySubtree[(int)dir_t.LEFT] != null) && (mySubtree[(int)dir_t.LEFT].Compare(Key()) == cmp_t.MIN_CMP))
            {
                valid = 0;
                Console.Error.WriteLine("Node " + Key() + " is *smaller* than left subtree" + mySubtree[(int)dir_t.LEFT].Key());
            }
            if ((mySubtree[(int)dir_t.RIGHT] != null) && (mySubtree[(int)dir_t.RIGHT].Compare(Key()) == cmp_t.MAX_CMP))
            {
                valid = 0;
                Console.Error.WriteLine("Node " + Key() + " is *greater* than right subtree" + mySubtree[(int)dir_t.RIGHT].Key());
            }

            return valid;
        }

        // Use mnemonic constants for valid balance-factor values
        private enum balance_t { LEFT_HEAVY = -1, BALANCED = 0, RIGHT_HEAVY = 1 };

        // Use mnemonic constants for indicating a change in height
        private enum height_effect_t { HEIGHT_NOCHANGE = 0, HEIGHT_CHANGE = 1 };

        // Return true if the tree is too heavy on the left side
        private static bool LEFT_IMBALANCE(int bal)
        {
            return (bal < (int)balance_t.LEFT_HEAVY);
        }

        // Return true if the tree is too heavy on the right side
        private static bool RIGHT_IMBALANCE(int bal)
        {
            return (bal > (int)balance_t.RIGHT_HEAVY);
        }

        // ----- Private data

        AVLComparable<KeyType> myData;  // Data field
        AVLNode<KeyType>[] mySubtree = new AVLNode<KeyType>[MAX_SUBTREES];   // Pointers to subtrees
        private int myBal;   // Balance factor


        // Reset all subtrees to null and clear the balance factor
        private void Reset()
        {
            myBal = 0;
            mySubtree[(int)dir_t.LEFT] = null;
            mySubtree[(int)dir_t.RIGHT] = null;
        }

        // ----- Routines that do the *real* insertion/deletion

        // Insert the given key into the given tree. Return the node if
        // it already exists. Otherwise return NULL to indicate that
        // the key was successfully inserted.  Upon return, the "change"
        // parameter will be '1' if the tree height changed as a result
        // of the insertion (otherwise "change" will be 0).
        private static AVLNode<KeyType> Insert(AVLComparable<KeyType> item,
                   ref AVLNode<KeyType> root,
                   ref height_effect_t change)
        {
            // See if the tree is empty
            if (root == null)
            {
                // Insert new node here
                root = new AVLNode<KeyType>(item);
                change = height_effect_t.HEIGHT_CHANGE;
                return root;
            }

            // Initialize
            AVLNode<KeyType> found = null;
            int increase = 0;

            // Compare items and determine which direction to search
            cmp_t result = root.Compare(item.Key());
            dir_t dir = (result == cmp_t.MIN_CMP) ? dir_t.LEFT : dir_t.RIGHT;

            if (result != cmp_t.EQ_CMP)
            {
                // Insert into "dir" subtree
                found = Insert(item, ref root.mySubtree[(int)dir], ref change);
                if (found == null) return null;     // already here - don't insert
                increase = (int)result * (int)change;  // set balance factor increment
            }
            else
            {   // key already in tree at this node
                increase = (int)height_effect_t.HEIGHT_NOCHANGE;
                return null;
            }

            root.myBal += increase;    // update balance factor

            // ----------------------------------------------------------------------
            // re-balance if needed -- height of current tree increases only if its 
            // subtree height increases and the current tree needs no rotation.
            // ----------------------------------------------------------------------

            change = (increase != 0 && root.myBal != 0) ? (1 - ReBalance(ref root)) : height_effect_t.HEIGHT_NOCHANGE;
            return found;
        }

        // Delete the given key from the given tree. Return NULL if the
        // key is not found in the tree. Otherwise return a pointer to the
        // node that was removed from the tree.  Upon return, the "change"
        // parameter will be '1' if the tree height changed as a result
        // of the deletion (otherwise "change" will be 0).
        private static AVLComparable<KeyType> Delete(KeyType key,
                   ref AVLNode<KeyType> root,
                   ref height_effect_t change,
                   cmp_t cmp = cmp_t.EQ_CMP)
        {
            // See if the tree is empty
            if (root == null)
            {
                // Key not found
                change = height_effect_t.HEIGHT_NOCHANGE;
                return null;
            }

            // Initialize
            AVLComparable<KeyType> found = null;
            height_effect_t decrease = 0;

            // Compare items and determine which direction to search
            cmp_t result = root.Compare(key, cmp);
            dir_t dir = (result == cmp_t.MIN_CMP) ? dir_t.LEFT : dir_t.RIGHT;

            if (result != cmp_t.EQ_CMP)
            {
                // Delete from "dir" subtree
                found = Delete(key, ref root.mySubtree[(int)dir], ref change, cmp);
                if (found == null) return found;   // not found - can't delete
                decrease = (height_effect_t)((int)result * (int)change);    // set balance factor decrement
            }
            else
            {   // Found key at this node
                found = root.myData;  // set return value

                // ---------------------------------------------------------------------
                // At this point we know "result" is zero and "root" points to
                // the node that we need to delete.  There are three cases:
                //
                //    1) The node is a leaf.  Remove it and return.
                //
                //    2) The node is a branch (has only 1 child). Make "root"
                //       (the pointer to this node) point to the child.
                //
                //    3) The node has two children. We swap items with the successor
                //       of "root" (the smallest item in its right subtree) and delete
                //       the successor from the right subtree of "root".  The
                //       identifier "decrease" should be reset if the subtree height
                //       decreased due to the deletion of the successor of "root".
                // ---------------------------------------------------------------------

                if ((root.mySubtree[(int)dir_t.LEFT] == null) &&
                    (root.mySubtree[(int)dir_t.RIGHT] == null))
                {
                    // We have a leaf -- remove it
                    root = null;
                    change = height_effect_t.HEIGHT_CHANGE;    // height changed from 1 to 0
                    return found;
                }
                else if ((root.mySubtree[(int)dir_t.LEFT] == null) ||
                           (root.mySubtree[(int)dir_t.RIGHT] == null))
                {
                    // We have one child -- only child becomes new root
                    AVLNode<KeyType> toDelete = root;
                    root = root.mySubtree[(root.mySubtree[(int)dir_t.RIGHT] != null) ? (int)dir_t.RIGHT : (int)dir_t.LEFT];
                    change = height_effect_t.HEIGHT_CHANGE;    // We just shortened the subtree
                                                               // Null-out the subtree pointers so we dont recursively delete
                    toDelete.mySubtree[(int)dir_t.LEFT] = toDelete.mySubtree[(int)dir_t.RIGHT] = null;
                    return found;
                }
                else
                {
                    // We have two children -- find successor and replace our current
                    // data item with that of the successor
                    root.myData = Delete(key, ref root.mySubtree[(int)dir_t.RIGHT],
                                          ref decrease, cmp_t.MIN_CMP);
                }
            }

            root.myBal -= (int)decrease;       // update balance factor

            // ------------------------------------------------------------------------
            // Rebalance if necessary -- the height of current tree changes if one
            // of two things happens: (1) a rotation was performed which changed
            // the height of the subtree (2) the subtree height decreased and now
            // matches the height of its other subtree (so the current tree now
            // has a zero balance when it previously did not).
            // ------------------------------------------------------------------------
            //change = (decrease) ? ((root->myBal) ? balance(root) : HEIGHT_CHANGE)
            //                    : HEIGHT_NOCHANGE ;
            if (decrease > 0)
            {
                if (root.myBal > 0)
                {
                    change = ReBalance(ref root);  // rebalance and see if height changed
                }
                else
                {
                    change = height_effect_t.HEIGHT_CHANGE;   // balanced because subtree decreased
                }
            }
            else
            {
                change = height_effect_t.HEIGHT_NOCHANGE;
            }

            return found;
        }

        // Routines for rebalancing and rotating subtrees

        // Perform an XX rotation for the given direction 'X'. 
        // Return 1 if the tree height changes due to rotation,
        // otherwise return 0.
        private static height_effect_t RotateOnce(ref AVLNode<KeyType> root, dir_t dir)
        {
            dir_t otherDir = Opposite(dir);
            AVLNode<KeyType> oldRoot = root;

            // See if otherDir subtree is balanced. If it is, then this
            // rotation will *not* change the overall tree height.
            // Otherwise, this rotation will shorten the tree height.
            height_effect_t heightChange = ((root.mySubtree[(int)otherDir]?.myBal == 0)
                ? height_effect_t.HEIGHT_NOCHANGE
                : height_effect_t.HEIGHT_CHANGE);

            // assign new root
            //root = oldRoot.mySubtree[(int)otherDir];
            root = oldRoot.Subtree(otherDir);

            // new-root exchanges it's "dir" mySubtree for it's parent
            oldRoot.mySubtree[(int)otherDir] = root?.mySubtree[(int)dir];
            root.mySubtree[(int)dir] = oldRoot;

            // update balances
            oldRoot.myBal = -((dir == dir_t.LEFT) ? --(root.myBal) : ++(root.myBal));

            return heightChange;
        }


        // Perform an XY rotation for the given direction 'X'
        // Return 1 if the tree height changes due to rotation,
        // otherwise return 0.
        private static height_effect_t RotateTwice(ref AVLNode<KeyType> root, dir_t dir)
        {
            dir_t otherDir = Opposite(dir);
            AVLNode<KeyType> oldRoot = root;
            AVLNode<KeyType> oldOtherDirSubtree = root.mySubtree[(int)otherDir];

            // assign new root
            root = oldRoot.mySubtree[(int)otherDir].mySubtree[(int)dir];

            // new-root exchanges it's "dir" mySubtree for it's grandparent
            oldRoot.mySubtree[(int)otherDir] = root.mySubtree[(int)dir];
            root.mySubtree[(int)dir] = oldRoot;

            // new-root exchanges it's "other-dir" mySubtree for it's parent
            oldOtherDirSubtree.mySubtree[(int)dir] = root.mySubtree[(int)otherDir];
            root.mySubtree[(int)otherDir] = oldOtherDirSubtree;

            // update balances
            root.mySubtree[(int)dir_t.LEFT].myBal = -Math.Max(root.myBal, 0);
            root.mySubtree[(int)dir_t.RIGHT].myBal = -Math.Min(root.myBal, 0);
            root.myBal = 0;

            // A double rotation always shortens the overall height of the tree
            return height_effect_t.HEIGHT_CHANGE;
        }

        // Rebalance a (sub)tree if it has become imbalanced
        private static height_effect_t ReBalance(ref AVLNode<KeyType> root)
        {
            height_effect_t heightChange = height_effect_t.HEIGHT_NOCHANGE;

            if (LEFT_IMBALANCE(root.myBal))
            {
                // Need a right rotation
                if (root.mySubtree[(int)dir_t.LEFT]?.myBal == (int)balance_t.RIGHT_HEAVY)
                {
                    // LR rotation needed
                    heightChange = RotateTwice(ref root, dir_t.RIGHT);
                }
                else
                {
                    // LL rotation needed
                    heightChange = RotateOnce(ref root, dir_t.RIGHT);
                }
            }
            else if (RIGHT_IMBALANCE(root.myBal))
            {
                // Need a left rotation
                if (root.mySubtree[(int)dir_t.RIGHT]?.myBal == (int)balance_t.LEFT_HEAVY)
                {
                    // RL rotation needed
                    heightChange = RotateTwice(ref root, dir_t.LEFT);
                }
                else
                {
                    // RR rotation needed
                    heightChange = RotateOnce(ref root, dir_t.LEFT);
                }
            }

            return heightChange;
        }

        // Perform a comparison of the given key against the given
        // item using the given criteria (min, max, or equivalence
        // comparison). Returns:
        //   EQ_CMP if the keys are equivalent
        //   MIN_CMP if this key is less than the item's key
        //   MAX_CMP if this key is greater than item's key
        private cmp_t Compare(KeyType key, cmp_t cmp = cmp_t.EQ_CMP)
        {
            switch (cmp)
            {
                default:
                case cmp_t.EQ_CMP:  // Standard comparison
                    return myData.Compare(key);

                case cmp_t.MIN_CMP:  // Search the minimal element in this tree
                    return (mySubtree[(int)dir_t.LEFT] == null) ? cmp_t.EQ_CMP : cmp_t.MIN_CMP;

                case cmp_t.MAX_CMP:  // Search the maximal element in this tree
                    return (mySubtree[(int)dir_t.RIGHT] == null) ? cmp_t.EQ_CMP : cmp_t.MAX_CMP;
            }
        }
    }

    public class AVLTree<KeyType> where KeyType : IComparable<KeyType>
    {
        public AVLNode<KeyType> myRoot;
        public AVLTree() => myRoot = null;
        ~AVLTree() { }

        // See If the tree is empty
        bool IsEmpty() => myRoot == null;

        public AVLNode<KeyType> Search(KeyType key, cmp_t cmp = cmp_t.EQ_CMP) => AVLNode<KeyType>.Search(key, myRoot, cmp);
        public AVLNode<KeyType> Insert(AVLComparable<KeyType> item) => AVLNode<KeyType>.Insert(item, ref myRoot);
        public AVLComparable<KeyType> Delete(KeyType key, cmp_t cmp = cmp_t.EQ_CMP) => AVLNode<KeyType>.Delete(key, ref myRoot, cmp);

        // As with all binary trees, a node's in-order successor is the left-most child of its right subtree, and a node's in-order predecesor is the right-most child of its left subtree.
        public AVLNode<KeyType> Next(AVLNode<KeyType> node)
        {
            AVLNode<KeyType> q;
            AVLNode<KeyType> p = node.Subtree(AVLNode<KeyType>.dir_t.RIGHT);
            if (p != null)
            {
                while (p.Subtree(AVLNode<KeyType>.dir_t.LEFT) != null) p = p.Subtree(AVLNode<KeyType>.dir_t.LEFT);
                return p;
            }
            else // find parent, check if node is on left subtree
            {
                q = node;
                p = node.Parent(myRoot);
                while (p != null && (q == p.Subtree(AVLNode<KeyType>.dir_t.RIGHT)))
                {
                    q = p;
                    p = node.Parent(myRoot);
                }
                return p;
            }
        }

        public AVLNode<KeyType> Prev(AVLNode<KeyType> node)
        {
            AVLNode<KeyType> q;
            AVLNode<KeyType> p = node.Subtree(AVLNode<KeyType>.dir_t.LEFT);
            if (p != null)
            {
                while (p.Subtree(AVLNode<KeyType>.dir_t.RIGHT) != null) p = p.Subtree(AVLNode<KeyType>.dir_t.RIGHT);
                return p;
            }
            else
            {
                // find parent, check if node is on left subtree
                q = node;
                p = node.Parent(myRoot);
                while (p != null && (q == p.Subtree(AVLNode<KeyType>.dir_t.LEFT)))
                {
                    q = p;
                    p = p.Parent(myRoot);
                }

                return p;
            }
        }

        public bool Check() => (myRoot != null) ? myRoot.Check() == 1 :  true;
    }

    /// <summary>
    /// cmp_t is an enumeration type indicating the result of a  comparison.
    /// </summary>
    public enum cmp_t
    {
        MIN_CMP = -1,   // less than
        EQ_CMP = 0,    // equal to
        MAX_CMP = 1     // greater than
    }
}
