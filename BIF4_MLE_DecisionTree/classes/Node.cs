using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    class Node
    {
        private bool _isRoot;
        public bool IsRoot
        {
            get { return _isRoot; }
            private set { _isRoot = value; }
        }

        private bool _isLeaf;
        public bool IsLeaf
        {
            get { return _isLeaf; }
            private set { _isLeaf = value; }
        }

        private List<Node> _followingNodes;
        public List<Node> FollowingNodes
        {
            get { return _followingNodes; }
            set { _followingNodes = value; }
        }

        private string _valueToGetHere;
        public string ValueToGetHere
        {
            get { return _valueToGetHere; }
            set { _valueToGetHere = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Node(bool isRoot, bool isLeaf, string valueToGetHere, string name)
        {
            _isRoot = isRoot;
            _isLeaf = isLeaf;
            _valueToGetHere = valueToGetHere;
            _name = name;
            FollowingNodes = new List<Node>();

            if (IsRoot && IsLeaf)
            {
                throw new ArgumentException("A node can't be root and leaf");
            }
        }

        public void PrintPretty(Node node, string indent, bool last)
        {
            if (node.IsRoot)
            {
                Console.WriteLine(indent + "+- " + node.Name);
            }
            else
            {
                Console.WriteLine(indent + "+- " + node.ValueToGetHere + " => " + node.Name);
            }
            
            indent += last ? "   " : "|  ";

            for (int i = 0; i < node.FollowingNodes.Count; i++)
            {
                PrintPretty(node.FollowingNodes[i], indent, i == node.FollowingNodes.Count - 1);
            }
        }
    }
}
