using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public class ProductCategoryVM
    {
        public int ProductCategoryID { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string ProductCategoryName { get; set; }
    }

    public class ProductVM
    {
        public int ProductID { get; set; }
        public int ProductCategoryID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        public double ProductPrice { get; set; }
        public double Stock { get; set; }
    }


    public class ProductListCategoryWise
    {
        public ProductListCategoryWise()
        {
            products = new HashSet<ProductList>();
        }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public ICollection<ProductList> products { get; set; }

    }

    public class ProductList
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }

    public class ProductSold
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double SquareInch { get; set; }
    }
}
