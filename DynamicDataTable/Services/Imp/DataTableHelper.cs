using DynamicDataTable.Attributes.Attributes;
using DynamicDataTable.ViewModels.Datatable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicDataTable.Services.Imp
{
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
}
