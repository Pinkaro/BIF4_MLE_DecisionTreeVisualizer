using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    class DecisionTree
    {
        private Node _rootNode;
        public Node RootNode
        {
            get { return _rootNode; }
        }

        public DecisionTree(DataSet dataSet)
        {
            int attributeIndex = dataSet.GetHighestInformationGainIndex();
            string rootName = dataSet.Data[0].Values[attributeIndex].Name;
            _rootNode = new Node(true, false, null, rootName);

            CreateDecisionTree(dataSet, _rootNode);
        }

        private static void CreateDecisionTree(DataSet dataSet, Node currentNode)
        {
            int attributeIndex = dataSet.GetHighestInformationGainIndex();
            List<SubDataSet> nodePaths = (List<SubDataSet>)dataSet.GetSubDataSets(attributeIndex);
            bool alreadyDecimated = false;
            DataSet newDataSet = null;

            //create following nodes for current node
            foreach (SubDataSet subDataSet in nodePaths)
            {
                if (subDataSet.Entropy == 0 && subDataSet.SingularDecidingExpressions()) // leaf node found
                {
                    string valueToGetThere = subDataSet.Data[0].Values[attributeIndex].Value;
                    string name = subDataSet.Data[0].Values[subDataSet.DecidingColumn].Value;
                    currentNode.FollowingNodes.Add(new Node(false, true, valueToGetThere, name));
                }
                else // normal node
                {
                    subDataSet.CalculateInformationGains();
                    int currentHighestInformationGain = subDataSet.GetHighestInformationGainIndex();

                    string valueToGetThere = subDataSet.Data[0].Values[attributeIndex].Value;
                    string nodeName = subDataSet.Data[0].Values[currentHighestInformationGain].Name;

                    Node newNode = new Node(false, false, valueToGetThere, nodeName);
                    currentNode.FollowingNodes.Add(newNode);

                    if (!alreadyDecimated)
                    {
                        newDataSet = subDataSet.GetDataSetWithoutAttribute(attributeIndex);
                        alreadyDecimated = true;
                    }
                    
                    CreateDecisionTree(newDataSet, newNode);
                }
            }
        }

        public void PrintPretty()
        {
            RootNode.PrintPretty(RootNode,"", true);
        }

        public override string ToString()
        {
            return RootNode.ToString();
        }
    }
}
