using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    [Serializable]
    class DataSet
    {
        private int _decidingColumn;
        public int DecidingColumn
        {
            get { return _decidingColumn; }
            protected set { _decidingColumn = value; }
        }

        private double[] _informationGains;
        protected double[] InformationGains
        {
            get { return _informationGains; }
            set { _informationGains = value; }
        }

        private double _decidingAttributeTrue;
        protected double DecidingAttributeTrue
        {
            get { return _decidingAttributeTrue; }
            set { _decidingAttributeTrue = value; }
        }

        private double _decidingAttributeFalse;
        protected double DecidingAttributeFalse
        {
            get { return _decidingAttributeFalse; }
            set { _decidingAttributeFalse = value; }
        }

        private List<DataRow> _data;
        public List<DataRow> Data
        {
            get { return _data; }
            protected set { _data = value; }
        }

        private double _entropy;
        public double Entropy
        {
            get { return _entropy; }
            protected set { _entropy = value; }
        }

        private double _dataRowsAmount;
        public double DataRowsAmount
        {
            get { return _dataRowsAmount; }
            protected set { _dataRowsAmount = value; }
        }

        // This constructor is called when the next node has already been decided or we are in root
        public DataSet(List<DataRow> data, double dataRowsAmount, double decidingAttributeYes, double decidingAttributeNo, int decidingColumn)
        {
            _dataRowsAmount = dataRowsAmount;
            _data = data;
            _decidingAttributeTrue = decidingAttributeYes;
            _decidingAttributeFalse = decidingAttributeNo;
            _decidingColumn = decidingColumn;
            _informationGains = new double[data[0].Values.Length];

            CalculateEntropy();
            CalculateInformationGains();
        }

        protected DataSet() { }

        protected void CalculateEntropy()
        {
            if (_decidingAttributeFalse == 0)
            {
                _entropy = (-(_decidingAttributeTrue / _dataRowsAmount) * Math.Log((_decidingAttributeTrue / _dataRowsAmount), 2));
            }
            else if (_decidingAttributeTrue == 0)
            {
                _entropy = (-(_decidingAttributeFalse / _dataRowsAmount) * Math.Log((_decidingAttributeFalse / _dataRowsAmount), 2));
            }
            else
            {
                _entropy = (-(_decidingAttributeTrue / _dataRowsAmount) * Math.Log((_decidingAttributeTrue / _dataRowsAmount), 2))
                       + (-(_decidingAttributeFalse / _dataRowsAmount) * Math.Log((_decidingAttributeFalse / _dataRowsAmount), 2));
            }
        }

        public void CalculateInformationGains()
        {
            // For every Attribute create a sorted Dataset after its value levels
            // cycle through the amount of attributes because we need datasets for each value level of attributes
            for (int i = 0; i < _informationGains.Length; i++)
            {
                if (i != _decidingColumn)
                {
                    double informationGain = 0.0;

                    // Get sub datasets for each attribute through index number
                    IEnumerable<DataSet> attributeDatasets = GetSubDataSets(i);

                    foreach (DataSet subDataSet in attributeDatasets)
                    {
                        informationGain -= ((subDataSet.DataRowsAmount / _dataRowsAmount) * subDataSet.Entropy);
                    }
                    _informationGains[i] = _entropy + informationGain;
                }
            }
        }

        public int GetHighestInformationGainIndex()
        {
            int highestInformationGainIndex = 0;
            double highestInformationGain = 0.0;
            for (int i = 0; i < _informationGains.Length; i++)
            {
                if (_informationGains[i] > highestInformationGain)
                {
                    highestInformationGain = _informationGains[i];
                    highestInformationGainIndex = i;
                }
            }
            return highestInformationGainIndex;
        }

        public IEnumerable<SubDataSet> GetSubDataSets(int attributeIndex)
        {
            // find attribute levels
            HashSet<string> attributeLevels = GetAttributeLevels(attributeIndex);
            List<SubDataSet> subDataSets = new List<SubDataSet>();

            // create sub datasets
            foreach (string attributeLevel in attributeLevels)
            {
                subDataSets.Add(GetSubDataSet(attributeIndex, attributeLevel));
            }

            return subDataSets;
        }

        private SubDataSet GetSubDataSet(int attributeIndex, string attributeLevel)
        {
            // instantiate required parameters for each sub-dataset
            double subDataRowsAmount = 0.0;
            double subDecidingAttributeYes = 0.0;
            double subDecidingAttributeNo = 0.0;
            List<DataRow> subData = new List<DataRow>();

            foreach (DataRow dataRow in _data)
            {
                string attributeValue = dataRow.Values[attributeIndex].Value;
                if (attributeValue.Equals(attributeLevel))
                {
                    subDataRowsAmount++;
                    subData.Add(dataRow);

                    // check for deciding value
                    if (dataRow.Values[_decidingColumn].Value.Equals(DataInfo.DecidingValueTrue))
                    {
                        subDecidingAttributeYes++;
                    }
                    else
                    {
                        subDecidingAttributeNo++;
                    }
                }
            }
            return new SubDataSet(subData, subDataRowsAmount, subDecidingAttributeYes, subDecidingAttributeNo, _decidingColumn);
        }

        private HashSet<string> GetAttributeLevels(int attributeIndex)
        {
            HashSet<string> attributeLevels = new HashSet<string>();

            foreach (DataRow dataRow in _data)
            {
                string attributeValue = dataRow.Values[attributeIndex].Value;
                if (!attributeLevels.Contains(attributeValue))
                {
                    attributeLevels.Add(attributeValue);
                }
            }

            return attributeLevels;
        }

        public DataSet GetDataSetWithoutAttribute(int attributeIndex)
        {
            List<DataRow> newData = new List<DataRow>();

            foreach (DataRow dataRow in Data)
            {
                Attribute[] newAttributes = new Attribute[dataRow.Values.Length - 1];

                for (int i = 0; i < dataRow.Values.Length; i++)
                {
                    if (i < attributeIndex)
                    {
                        newAttributes[i] = dataRow.Values[i];
                    }

                    if (i > attributeIndex)
                    {
                        newAttributes[i - 1] = dataRow.Values[i];
                    }
                }
                newData.Add(new DataRow(newAttributes));
            }

            double[] newInformationGains = new double[_informationGains.Length - 1];
            for(int i = 0; i < _informationGains.Length; i++)
            {
                if(i < attributeIndex)
                {
                    newInformationGains[i] = _informationGains[i];
                }

                if(i > attributeIndex)
                {
                    newInformationGains[i - 1] = _informationGains[i];
                }
            }

            DataSet newDataSet = DeepClone<DataSet>(this);
            newDataSet.InformationGains = newInformationGains;
            newDataSet.Data = newData;
            newDataSet.DecidingColumn--;
            return newDataSet;
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[DATAROW]");
            sb.AppendLine("total: " + _dataRowsAmount);
            sb.AppendLine("true: " + _decidingAttributeTrue);
            sb.AppendLine("false: " + _decidingAttributeFalse);
            sb.AppendLine("entropy: " + _entropy);
            sb.AppendLine();

            sb.AppendLine("Informationgains: ");

            for (int i = 0; i < _informationGains.Length; i++)
            {
                sb.AppendLine("[" + i + "] " + _informationGains[i]);
            }

            sb.AppendLine();

            // get headers
            foreach (Attribute attribute in Data[0].Values)
            {
                sb.Append(attribute.Name + "     ");
            }

            foreach (DataRow dataRow in Data)
            {
                sb.AppendLine(dataRow.ToString());
            }

            return sb.ToString();
        }
    }
}
