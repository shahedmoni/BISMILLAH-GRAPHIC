using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
        }

        public ICollection<ProductListCategoryWise> ToListCustom()
        {
            var product = Context.ProductCategory.Include(c => c.Product).Where(c => c.Product.Any()).Select(c => new ProductListCategoryWise
            {
                ProductCategoryID = c.ProductCategoryID,
                ProductCategoryName = c.ProductCategoryName,
                products = c.Product.Select(p => new ProductList
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice
                }).ToList()

            }).ToList();

            return product;
        }

        public async Task<ICollection<ProductListCategoryWise>> ToListCustomAsync()
        {
            var product = await Context.ProductCategory.Include(c => c.Product).Where(c => c.Product.Any()).Select(c =>
                new ProductListCategoryWise
                {
                    ProductCategoryID = c.ProductCategoryID,
                    ProductCategoryName = c.ProductCategoryName,
                    products = c.Product.Select(p => new ProductList
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        ProductPrice = p.ProductPrice
                    }).ToList()

                }).ToListAsync();

            return product;
        }

        public void AddCustom(ProductVM model)
        {
            Add(new Product
            {
                ProductID = model.ProductID,
                ProductCategoryID = model.ProductCategoryID,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice
            });
        }

        public void UpdateCustom(ProductVM model)
        {
            var product = Find(model.ProductID);
            product.ProductName = model.ProductName;
            product.ProductCategoryID = model.ProductCategoryID;
            if (product.ProductPrice != model.ProductPrice)
                product.ProductPrice = model.ProductPrice;
            Update(product);
        }

        public ProductVM FindCustom(int? id)
        {
            var p = Find(id.GetValueOrDefault());

            if (p == null) return null;
            return new ProductVM
            {
                ProductID = p.ProductID,
                ProductCategoryID = p.ProductCategoryID,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice
            };
        }

        public bool RemoveCustom(int id)
        {
            if (Context.SellingList.Any(s => s.ProductID == id)) return false;
            Remove(Find(id));
            return true;
        }

        public async Task<ICollection<ProductVM>> SearchAsync(string key)
        {
            return await Context.Product.Where(p => p.ProductName.Contains(key)).Select(p =>
                new ProductVM
                {
                    ProductID = p.ProductID,
                    ProductCategoryID = p.ProductCategoryID,
                    ProductName = p.ProductName,
                    ProductCategoryName = p.ProductCategory.ProductCategoryName,
                    ProductPrice = p.ProductPrice
                }).Take(5).ToListAsync();
        }

        public ICollection<ProductSold> SoldReport(DateTime? fDateTime, DateTime? tDateTime)
        {

            var fDate = fDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var tDate = tDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var report = Context.Product.Include(p => p.SellingList).Select(p => new ProductSold
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                SquareInch = p.SellingList.Where(l => l.Selling.SellingDate >= fDate && l.Selling.SellingDate <= tDate).Sum(l => l.SellingQuantity)
            });
            return report.ToList();
        }
    }
}