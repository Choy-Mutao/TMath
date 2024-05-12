using System;

namespace tmath.gs.trees
{
    class Comparable<KeyType> where KeyType : IEquatable<KeyType>, IComparable<KeyType>
    {
        private KeyType myKey;

        public Comparable(KeyType key) => myKey = key;
        ~Comparable() { }

        // Compare this item against the given key & return the result;
        public virtual cmp_t Compare(KeyType key)
        {
            return key.Equals(myKey) ? cmp_t.EQ_CMP : ((key.CompareTo(myKey) < 0) ? cmp_t.MIN_CMP : cmp_t.MAX_CMP);
        }

        // Get the key-field for an item
        KeyType Key() => myKey;

    }

    public class AVLNode<KeyType> where KeyType : IEquatable<KeyType>, IComparable<KeyType>
    {
        Comparable<KeyType> myData; // Data field
        AVLNode<KeyType>[] mySubtree;

        public enum AVLEnum
        {
            MAX_SUBTREES = 2,
        }

        enum dir_t { LEFT = 0, RIGHT = 1 };

        // Constructors and Destructors;
        AVLNode(Comparable<KeyType> item = null)
        {

        }
        ~AVLNode() { }

        // Get this node's data
        Comparable<KeyType> Data() => myData;

    }

    public class AVLTree<KeyType> where KeyType : IEquatable<KeyType>, IComparable<KeyType>
    {
        public AVLNode<KeyType> myRoot;
        public AVLTree() => myRoot = null;
        ~AVLTree() { }

        // See If the tree is empty
        bool IsEmpty() => myRoot == null;

        AVLNode<KeyType> Search(KeyType key, cmp_t cmp = cmp_t.EQ_CMP)
        {
            return null;
        }
    }

    /// <summary>
    /// cmp_t is an enumeration type indicating the result of a  comparison.
    /// </summary>
    enum cmp_t
    {
        MIN_CMP = -1,   // less than
        EQ_CMP = 0,    // equal to
        MAX_CMP = 1     // greater than
    }
}
