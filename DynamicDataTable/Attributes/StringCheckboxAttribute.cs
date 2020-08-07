using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicDataTable.Attributes.Attributes
{
    public class StringCheckboxAttribute : Attribute
    {
        public  string WhenTrue;
        public  string WhenFalse;
        public StringCheckboxAttribute(string whenFalse,string whenTrue)
        {
            WhenFalse = whenFalse;
            WhenTrue = whenTrue;
        }

    }
}
