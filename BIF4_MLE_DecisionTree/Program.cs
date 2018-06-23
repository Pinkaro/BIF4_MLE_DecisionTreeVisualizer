using BIF4_MLE_DecisionTree.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            string partyOrNotPath = "./data/PartyOrNot.csv";
            string playOrNotPath = "./data/ToPlayOrNotToPlay.csv";

            DataSetReader dataSetReader = new DataSetReader(playOrNotPath, "yes", ",", true, 4);
            //DataSetReader dataSetReader = new DataSetReader(partyOrNotPath, "party", ";", true, 3);

            DataSet dataSet = dataSetReader.GetDataSet();

            DecisionTree decisionTree = new DecisionTree(dataSet);

            //Console.WriteLine(dataSet.ToString());

            decisionTree.PrintPretty();

            Console.ReadKey();
        }
    }
}
