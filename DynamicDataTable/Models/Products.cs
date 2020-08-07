using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DynamicDataTable.ViewModels
{
   public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }


    }
}
