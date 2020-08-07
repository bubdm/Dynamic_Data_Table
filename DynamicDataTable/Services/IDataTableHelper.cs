using DynamicDataTable.ViewModels.Datatable;
using System;

namespace DynamicDataTable.Services
{
    public interface IDataTableHelper
    {
        IDataTableHelper AddExtraHeader(string name);
        IDataTableHelper Initiate(string searchEndpoint);
        IDataTableHelper GetFeilds(Type type);
        DatatableModel CreateDataTable();

    }
}
