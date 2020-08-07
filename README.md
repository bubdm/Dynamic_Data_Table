# Dynamic Datatable


The Project is a sample implemention of datatable.js in .net core project. the columns and data will be created automatically and you don't need to specify different javascript logic for your every page. There is different types of implemention for showing checkboxes in the page and you can easily add customized columns in your datatable. For this project you need :

  - .net core 3.1
  - sql server(for saving and retrieving data to and from database)
  - Datatable.js (CDN is included in the project. but using local versions of datatable.js is recommended)

## Features

  - two way of customizing checkboxes in the datatable, including html checkbox and strings
  - easily ignoring some rows in the datatable.
  - using display name attributes instead of the fields.
  - customization using attributes instead of javascript.
  - lightweight pagination for fast ajax call.

## How To Start
## Notice 
From step 1 to step 7 is just a one time job and you do not have to do this in every search page.
From step 8 to last is the jobs you may do every time for every search.
### 1-Creating DataTable Models
First thing first, we need to create a general model to contain all the data needed for creating datatable.
```
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
```
### 2-Creating an inteface
The Initiate method will get a search endpoint for retrieving data for datatable filling.for method chaining purposes, we will return the interface itself.
```
    public interface IDataTableHelper
    {
         IDataTableHelper Initiate(string searchEndpoint);
    }
```
The second method we need is to get feild of a ViewModel and convert it to columns. It can also be chainable. the input of this method is just the type of our viewmodel. it can be any type of viewmodel.
```
    public interface IDataTableHelper
    {
        IDataTableHelper Initiate(string searchEndpoint);
        IDataTableHelper GetFeilds(Type type);

    }
```
For better customization, we need a method to add some extra column headers to datatable, here it is.
```
public interface IDataTableHelper
    {
        IDataTableHelper AddExtraHeader(string name);
        IDataTableHelper Initiate(string searchEndpoint);
        IDataTableHelper GetFeilds(Type type);

    }
```
Finally, we add a method to return the created datatable object which will be the main model in every page.
```
    public interface IDataTableHelper
    {
        IDataTableHelper AddExtraHeader(string name);
        IDataTableHelper Initiate(string searchEndpoint);
        IDataTableHelper GetFeilds(Type type);
        DatatableModel CreateDataTable();

    }
```
The inteface is now completed . moving on to next step.
### 3- Adding some customized attributes
Here is some customized attribute which will be needed after for customizing the way a field is shown in table. add these codes in a file in your attribute folders.
```
    public class HideInDataTableAttribute:Attribute
    {        
    }
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
```
Now we can use these Attibutes in every viewmodels.
### 4- Adding the logic of DatatableHelper
Here is the implemention of the inteface in step 2. We are looping through ervery feild in the given view model (Type) and add the proper header and values to datatabe model.
the last method it returning the finalized model.
```
 public class DataTableHelper : IDataTableHelper
    {
        private DatatableModel model;
        public IDataTableHelper Initiate(string searchEndpoint)
        {
            model = new DatatableModel(searchEndpoint);
            return this;
        }
        public IDataTableHelper AddExtraHeader(string name)
        {
            model.Headers.Add(name);
            return this;
        }
        public IDataTableHelper GetFeilds(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(var filed in properties)
            {
                var isHidden  = Attribute.GetCustomAttribute(filed, typeof(HideInDataTableAttribute))!=null;
                if (!isHidden)
                {
                    var displayNameAttr = ((DisplayNameAttribute)Attribute.GetCustomAttribute(filed, typeof(DisplayNameAttribute)))?.DisplayName;
                    var displayName = string.IsNullOrEmpty(displayNameAttr) ? filed.Name : displayNameAttr;
                    if (filed.PropertyType == typeof(bool))
                    {
                        var checboxField = new BooleanColumn
                        {
                            Title = filed.Name.ToLower()
                        };
                        var stringCheckboxAttr= Attribute.GetCustomAttribute(filed, typeof(StringCheckboxAttribute));
                        if (stringCheckboxAttr != null)
                        {
                            checboxField.WhenFalse = ((StringCheckboxAttribute)stringCheckboxAttr).WhenFalse;
                            checboxField.WhenTrue = ((StringCheckboxAttribute)stringCheckboxAttr).WhenTrue;
                            
                        }
                        model.CheckBoxes.Add(checboxField);

                    }
                    else
                    {
                        model.StringColumns.Add(new StringColumn() { 
                        data= filed.Name.ToLower()
                        });
                    
                    }
                    model.Headers.Add(displayName);

                }
            }
            return this;
        }
        public DatatableModel CreateDataTable()
        {
            return model;
        }
    }
```
### 5-Converting datatable.js body to class.
The request which is being sent from datatable.js will be converted to an object like this:
```
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
    public class Order
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }
        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
     public class Search
    {
        public string Value { get; set; }
    }
```
### 6- Adding a generic Response body for search response.
T here is the type of object you are returning form your different search method for different objects.
```
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
```
### 8- Adding two partial views in SharedFolder.
the first is Html partial of the datatable and the script is logic of script.
#### Html Partial
```

@{
    Layout = null;
}
@model DynamicDataTable.ViewModels.Datatable.DatatableModel
<table class="table table-bordered " id="productTable">
        <thead>
            <tr>
                @foreach(var item in Model.Headers)
                {
                    <th>@item</th>
                }
            </tr>
        </thead>
        <tbody></tbody>
    </table>
```
#### Script Partial:
```
@{
    Layout = null;
}
@model DynamicDataTable.ViewModels.Datatable.DatatableModel
<script>
    var columns = @(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model.StringColumns)));
    var targetcount = columns.length-1;
    var checkboxes = @(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model.CheckBoxes)))
    $.each(checkboxes, function (index, item) {
        targetcount++
        if (item.WhenTrue !== null) {
            columnDefs.push({
                targets: targetcount,
                data: item.Title,
                render: function (data, type, row, meta) {

                    return `${(data ? item.WhenTrue : item.WhenFalse)}`;
                }
            });
        }
        else {
            columnDefs.push({
                targets: targetcount,
                data: item.Title,
                render: function (data, type, row, meta) {

                    var checked = data ? 'checked="checked"' : ""
                    return `<input ${checked}  type="checkbox">`
                }
            });
        }

        });
</script>
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/bs4/dt-1.10.21/datatables.min.css" />
<script type="text/javascript" src="https://cdn.datatables.net/v/bs4/dt-1.10.21/datatables.min.js"></script>
<script>
    $(function () {
            var table = $('#productTable').DataTable({
                "ordering": true,
                "pageLength": @Model.PageSize,
                "bLengthChange": false,
    proccessing: true,
    serverSide: true,

    ajax: {
        url:"@Model.SearchEndPoint",
        type: 'POST'
                },
                columns: columns,
      columnDefs: columnDefs
});
        });

</script>

```

### 8- A Sample Search Function with Entity Framework Core.
This is a sample method and you can change it however you want.
but remember the response of the method is always of type : DataTableReady<T> where T is your main viewmodel.
```
    public interface IProductManager
    {
        Task<DataTableReady<ProductViewModel>> GetProductsAsync(DataTableBody body);
    }
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _db;
        public ProductManager(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<DataTableReady<ProductViewModel>> GetProductsAsync(DataTableBody body)
        {
            var query = CreateFilterAndSort(body.Search, body.Order);
            var filteredCount = await query.CountAsync();
            var totalCount = await _db.Products.CountAsync();
            query = query.Skip(body.Start).Take(body.Length);
            return new DataTableReady<ProductViewModel>
            {
                Draw = body.Draw,
                RecordsFiltered = filteredCount,
                RecordsTotal = totalCount,

                Data =await query.Select(c => new ProductViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync()
            };
        }
        private IQueryable<Product> CreateFilterAndSort(Search search,List<Order> order)
        {
           var query =  _db.Products
                .Where(c => search == null || string.IsNullOrEmpty(search.Value) || c.Name == search.Value);
            return query;
        }
```
### 9- Creating a sample controller capable of returning a page and searching.
```
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

```
### 10 - Index Views:
```
@model DynamicDataTable.ViewModels.Datatable.DatatableModel 
@(await Html.PartialAsync("_DataTableHtml",Model))

@section Scripts{
<script>
    var columnDefs = [];
    columnDefs.push({
        targets: 4,
        data: "id",
        render: function (data, type, row, meta) {
            return `
            <a data-toggle="tooltip" title="Remove" class="btn btn-danger deletezone"><i class="fa fa-trash"></i></a>`;
        }
    });
</script>
@(await Html.PartialAsync("_DataTableScript",Model))
}

```
## Warning :
in the index view, declaring the columnDefs is mandatory but adding customized buttons is optional.

