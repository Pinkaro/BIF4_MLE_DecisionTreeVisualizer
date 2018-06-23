using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    [Serializable]
    class SubDataSet : DataSet
    {
        public SubDataSet(List<DataRow> data, double dataRowsAmount, double decidingAttributeYes, double decidingAttributeNo, int decidingColumn)
        {
            DataRowsAmount = dataRowsAmount;
            Data = data;
            DecidingAttributeTrue = decidingAttributeYes;
            DecidingAttributeFalse = decidingAttributeNo;
            DecidingColumn = decidingColumn;
            InformationGains = new double[Data[0].Values.Length];

            CalculateEntropy();
        }

        public bool SingularDecidingExpressions()
        {
            string differentExpression = string.Empty;
            int differences = 0;

            foreach(DataRow dataRow in Data)
            {
                if(dataRow.Values[DecidingColumn].Value != differentExpression)
                {
                    differentExpression = dataRow.Values[DecidingColumn].Value;
                    differences++;
                }
            }
            
            if(differences == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanWeStillSplit()
        {
            return true;
        }
    }
}
