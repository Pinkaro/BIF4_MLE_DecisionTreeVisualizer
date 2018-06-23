using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    class DataSetReader
    {
        private string _dataPath;
        private string _seperator;
        private bool _header;
        private int _decidingColumn;
        private string _yesValue;

        public DataSetReader(string dataPath, string yesValue, string seperator, bool header, int decidingColumn)
        {
            _yesValue = yesValue;
            DataInfo.DecidingValueTrue = yesValue;
            _dataPath = dataPath;
            _seperator = seperator;
            _header = header;
            _decidingColumn = decidingColumn;
        }

        public DataSet GetDataSet()
        {
            if (_dataPath == string.Empty)
            {
                throw new ArgumentException("Call SetParameters method, database connection was not set");
            }

            string[] allLines = File.ReadAllLines(_dataPath);
            string[] headers = InitializeHeaders(allLines);
            string[] data = GetPureData(allLines);

            List<DataRow> dataRows = new List<DataRow>();
            double dataRowsAmount = 0.0;
            double decidingAttributeYes = 0.0;
            double decidingAttributeNo = 0.0;

            foreach(string line in data)
            {
                dataRowsAmount++;
                string[] parameters = line.Split(_seperator.ToCharArray());
                Attribute[] attributes = new Attribute[parameters.Length];

                // check deciding values
                if(parameters[_decidingColumn] == _yesValue)
                {
                    decidingAttributeYes++;
                }
                else
                {
                    decidingAttributeNo++;
                }

                // set Attributes
                for (int i = 0; i < parameters.Length; i++)
                {
                    attributes[i] = new Attribute(headers[i], parameters[i]);
                }

                dataRows.Add(new DataRow(attributes));
            }

            return new DataSet(dataRows, dataRowsAmount, decidingAttributeYes, decidingAttributeNo, _decidingColumn);
        }

        private string[] GetPureData(string[] allLines)
        {
            if (!_header)
            {
                return allLines;
            }
            else
            {
                string[] temp = new string[allLines.Length - 1];

                for(int i = 1; i < allLines.Length; i++)
                {
                    temp[i-1] = allLines[i];
                }

                return temp;
            }
        }

        public string[] InitializeHeaders(string[] fileContent)
        {
            if (_header)
            {
                string line = fileContent[0];

                return line.Split(_seperator.ToCharArray());
            }
            else
            {
                string line = fileContent[0];
                int arguments = line.Split(_seperator.ToCharArray()).Length;
                string[] headers = new string[arguments];

                for (int i = 0; i < arguments; i++)
                {
                    headers[i] = "Attribute " + (i + 1);
                }

                return headers;
            }
        }
    }
}
