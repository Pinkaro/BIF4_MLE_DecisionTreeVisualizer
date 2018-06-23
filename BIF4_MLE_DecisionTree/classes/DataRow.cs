using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    [Serializable]
    class DataRow
    {
        private Attribute[] _values;
        public Attribute[] Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public DataRow(Attribute[] values)
        {
            _values = values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            // get header
            foreach (Attribute attribute in Values)
            {
                sb.Append(attribute.Value + "     ");
            }

            return sb.ToString();
        }
    }
}
