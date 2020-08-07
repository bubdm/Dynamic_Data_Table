using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DynamicDataTable.ViewModels.Datatable
{
    public class DatatableModel
    {
        
        public DatatableModel(string searchEndPoint)
        {
            StringColumns = new List<StringColumn>();
            CheckBoxes = new List<BooleanColumn>();
            Headers = new List<string>();
            SearchEndPoint = searchEndPoint;
        }
        public int PageSize { get; set; } = 10;
        public string SearchEndPoint { get; set; }
        public List<StringColumn> StringColumns { get; set; }
        public List<BooleanColumn> CheckBoxes { get; set; }
        public List<string> Headers { get; set; }
    }
    public class ExtraRows
    {
        public string Header { get; set; }
        public string Html { get; set; }
    }
    public class StringColumn
    {
#pragma warning disable IDE1006 // Naming Styles
        public string data { get; set; }
       
#pragma warning restore IDE1006 // Naming Styles
    }
    public class BooleanColumn
    {
        public string Title { get; set; }
        public string WhenTrue { get; set; }
        public string WhenFalse { get; set; }

    }
}
