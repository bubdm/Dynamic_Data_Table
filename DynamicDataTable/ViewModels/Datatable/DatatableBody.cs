
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DynamicDataTable.ViewModels.Datatable
{
    public class DataTableBody
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }
        [JsonPropertyName("start")]
        public int Start { get; set; }
        [JsonPropertyName("length")]
        public int Length { get; set; }
        [JsonPropertyName("search")]
        public Search Search { get; set; }
        [JsonPropertyName("order")]
        public List<Order> Order { get; set; }
    }
}
