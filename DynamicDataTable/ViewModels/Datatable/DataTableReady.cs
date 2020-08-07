
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DynamicDataTable.ViewModels.Datatable
{
    public class DataTableReady<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; }
        [JsonPropertyName("draw")]
        public int Draw { get; set; }
        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; set; }
        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered { get; set; }
    }





}
