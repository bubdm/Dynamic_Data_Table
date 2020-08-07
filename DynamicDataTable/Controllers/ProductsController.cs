using System.Linq;
using System.Threading.Tasks;
using DynamicDataTable.Services;
using DynamicDataTable.ViewModels.Datatable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using Nahang.Admin.ViewModel;


namespace DynamicDataTable.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDataTableHelper _dataTableHelper;
        private readonly IProductManager _productManager;

        public ProductsController(IDataTableHelper dataTableHelper, IProductManager productManager)
        {
            _dataTableHelper = dataTableHelper;
            _productManager = productManager;

        }
        [HttpGet]
        public IActionResult Index()
        {
            var datatableModel = _dataTableHelper.
                Initiate(Url.ActionLink("Search", "Products", null))
                .GetFeilds(typeof(ProductViewModel))
                .AddExtraHeader("Actions")
                .CreateDataTable();
            return View(datatableModel);
        }
        [HttpPost]
        public async Task<JsonResult> Search(DataTableBody body)
        {
            return Json(await _productManager.GetProductsAsync(body));
        }

    }
}
