using Emojis;

namespace MyCollections
{
    public static class MyBinaryTree<T> where T : IInit, IComparable<T>, new()
    {
        static T GetInfo()
        {
            T data = new T();
            data.RandomInit();
            Console.WriteLine($"The element {data} is adding...");
            return data;
        }
        public static int Height(TreePoint<T> p)
        {
            return HeightRecursive(p) + 1;
        }
        public static int HeightRecursive(TreePoint<T> p)
        {
            if (p == null)
                return -1;
            int leftHeight = HeightRecursive(p.left);
            int rightHeight = HeightRecursive(p.right);
            return Math.Max(leftHeight, rightHeight) + 1;
        }
        public static TreePoint<T> IdealTree(int size, TreePoint<T> p)
        {
            if (size == 0)
                return null;
            int nleft = size / 2;
            int nright = size - nleft - 1;
            T data = GetInfo();
            TreePoint<T> result = new(data);
            result.left = IdealTree(nleft, result.left);
            result.right = IdealTree(nright, result.right);
            return result;
        }
        public static TreePoint<T> ToSearchTree(TreePoint<T> root)
        {
            TreePoint<T> searchTreeRoot = null; 
            searchTreeRoot =  ToSearchTreeHelper(root, searchTreeRoot); 
            return searchTreeRoot;
        }
        private static TreePoint<T> ToSearchTreeHelper(TreePoint<T> node, TreePoint<T> searchTreeRoot)
        {
            if (node == null)
                return searchTreeRoot;

            searchTreeRoot = AddToSearchTree(searchTreeRoot, node.data); 
            searchTreeRoot = ToSearchTreeHelper(node.left, searchTreeRoot);   
            searchTreeRoot = ToSearchTreeHelper(node.right, searchTreeRoot);  
            return searchTreeRoot;
        }
        static TreePoint<T> AddToSearchTree(TreePoint<T> root, T data)
        {
            if (root == null)
                return MakePoint(data);
            TreePoint<T> currentNode = root;
            TreePoint<T> parentNode = null;
            bool found = false;

            while (currentNode != null && !found)
            {
                parentNode = currentNode;
                int comparisonResult = data.CompareTo(currentNode.data);
                if (comparisonResult == 0)
                {
                    found = true;
                    return root;
                }
                else if (comparisonResult < 0)
                    currentNode = currentNode.left; 
                else
                    currentNode = currentNode.right;
            }
            TreePoint<T> newNode = MakePoint(data);
            if (data.CompareTo(parentNode.data) < 0)
                parentNode.left = newNode;
            else
                parentNode.right = newNode;
            return root; 
        }
        public static TreePoint<T> MakePoint(T data)
        {
            return new TreePoint<T>(data);
        }
        public static TreePoint<T> DeleteFromSearchTree(T data, TreePoint<T> root)
        {
            TreePoint<T> nodeToDelete = FindNodeToDelete(data, root, out TreePoint<T> parentNode);
            if (nodeToDelete == null)
                return root;

            if (nodeToDelete.left == null && nodeToDelete.right == null) 
                DeleteLeafNode(parentNode, nodeToDelete, ref root);
            else if (nodeToDelete.left == null || nodeToDelete.right == null) 
                DeleteNodeWithOneChild(parentNode, nodeToDelete, ref root);
            else 
                DeleteNodeWithTwoChildren(nodeToDelete, ref root);
            return root;
        }
        private static TreePoint<T> FindNodeToDelete(T data, TreePoint<T> root, out TreePoint<T> parentNode)
        {
            TreePoint<T> currentNode = root;
            parentNode = null;
            while (currentNode != null)
            {
                int comparisonResult = data.CompareTo(currentNode.data);
                if (comparisonResult == 0)
                    return currentNode;
                parentNode = currentNode;
                currentNode = (comparisonResult < 0) ? currentNode.left : currentNode.right;
            }
            return null;
        }
        private static void DeleteLeafNode(TreePoint<T> parentNode, TreePoint<T> nodeToDelete, ref TreePoint<T> root)
        {
            if (parentNode == null)
            {
                root = null;
            }
            else if (nodeToDelete == parentNode.left)
            {
                parentNode.left = null;
            }
            else
            {
                parentNode.right = null;
            }
        }
        private static void DeleteNodeWithOneChild(TreePoint<T> parentNode, TreePoint<T> nodeToDelete, ref TreePoint<T> root)
        {
            TreePoint<T> child = (nodeToDelete.left != null) ? nodeToDelete.left : nodeToDelete.right;

            if (parentNode == null)
                root = child;
            else if (nodeToDelete == parentNode.left)
                parentNode.left = child;
            else
                parentNode.right = child;
        }

        // Удаляет узел с двумя потомками
        private static void DeleteNodeWithTwoChildren(TreePoint<T> nodeToDelete, ref TreePoint<T> root)
        {
            TreePoint<T> successor = GetMinimum(nodeToDelete.right);
            nodeToDelete.data = successor.data;
            nodeToDelete.right = DeleteFromSearchTree(successor.data, nodeToDelete.right);
        }

        private static TreePoint<T> GetMinimum(TreePoint<T> node)
        {
            while (node.left != null)
                node = node.left;
            return node;
        }
        public static void ShowTree(TreePoint<T> p, int levelsCount)
        {
            const int indent = 5;
            if (p == null)
                return;
            ShowTree(p.right, levelsCount + indent);
            for (int i = 0; i < levelsCount; i++)
                Console.Write(" ");
            Console.WriteLine(p.data);
            ShowTree(p.left, levelsCount + indent);
        }

        public static void DeleteTree(TreePoint<T> p)
        {
            p.left = null;
            p.right = null;
            p = null;
        }
    }
}