using System.Threading.Tasks;
using Nahang.Admin.ViewModel;
using DynamicDataTable.ViewModels.Datatable;

namespace DynamicDataTable.Services
{
    public interface IProductManager
    {
        Task<DataTableReady<ProductViewModel>> GetProductsAsync(DataTableBody body);

    }
}
