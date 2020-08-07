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
### 1-Creating Models
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
There is some customized attribute which will be needed after for customizing the way a field is shown in table. add these codes in a file in your attribute folders.
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
### 6- 

