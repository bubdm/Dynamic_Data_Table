
using System.Text.Json.Serialization;

namespace DynamicDataTable.ViewModels.Datatable
{
    public class Order
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }
        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
}
