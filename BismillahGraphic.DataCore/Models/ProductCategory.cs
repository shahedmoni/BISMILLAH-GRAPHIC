using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public DateTime Insert_Date { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
