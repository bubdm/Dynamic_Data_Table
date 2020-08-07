using DynamicDataTable.Attributes.Attributes;

using System.ComponentModel;

namespace Nahang.Admin.ViewModel
{
    public class ProductViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [HideInDataTable]
        public string Description { get; set; }
        [StringCheckbox("Showing In Site", "Hidden In Site")]
        [DisplayName("Activation Status")]
        public bool IsActive { get; set; }
        [DisplayName("Verify Status")]
        public bool IsVerified { get; set; }

    }

}
