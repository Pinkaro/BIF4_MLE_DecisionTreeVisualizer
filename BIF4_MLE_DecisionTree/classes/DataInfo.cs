using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIF4_MLE_DecisionTree.classes
{
    public class DataInfo
    {
        private DataInfo _instance;
        public DataInfo Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DataInfo();
                }
                return _instance;
            }
        }

        public static string DecidingValueTrue;
    }
}
