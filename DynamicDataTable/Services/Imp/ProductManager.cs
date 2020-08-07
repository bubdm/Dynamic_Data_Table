using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Nahang.Admin.ViewModel;
using System.Collections.Generic;
using DynamicDataTable.Models;
using DynamicDataTable.ViewModels.Datatable;
using DynamicDataTable.ViewModels;

namespace DynamicDataTable.Services.Imp
{
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

    }

}
