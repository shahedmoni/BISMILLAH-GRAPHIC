using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class Product
    {
        public Product()
        {
            SellingList = new HashSet<SellingList>();
            PurchaseList = new HashSet<PurchaseList>();
        }

        public int ProductID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double Stock { get; set; }
        public DateTime Insert_Date { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<SellingList> SellingList { get; set; }
        public virtual ICollection<PurchaseList> PurchaseList { get; set; }
    }
}
