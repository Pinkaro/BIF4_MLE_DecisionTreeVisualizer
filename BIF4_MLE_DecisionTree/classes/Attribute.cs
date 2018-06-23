using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    [Serializable]
    class Attribute
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Attribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public Attribute(string name)
        {
            _name = name;
        }
    }
}
