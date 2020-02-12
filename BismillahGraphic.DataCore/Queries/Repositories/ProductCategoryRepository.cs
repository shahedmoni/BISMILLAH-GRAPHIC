using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    internal class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DataContext context) : base(context)
        {

        }
        public ICollection<DDL> ddl()
        {
            return Context.ProductCategory.Select(c => new DDL
            {
                value = c.ProductCategoryID,
                label = c.ProductCategoryName
            }).ToList();
        }

        public void AddCustom(ProductCategoryVM model)
        {
            Add(new ProductCategory
            {
                ProductCategoryName = model.ProductCategoryName
            });
        }

        public void UpdateCustom(ProductCategoryVM model)
        {
            var pCategory = Find(model.ProductCategoryID);
            pCategory.ProductCategoryName = model.ProductCategoryName;
            Update(pCategory);
        }

        public ProductCategoryVM FindCustom(int? id)
        {
            var pCategory = Find(id.GetValueOrDefault());

            if (pCategory == null) return null;
            return new ProductCategoryVM
            {
                ProductCategoryID = pCategory.ProductCategoryID,
                ProductCategoryName = pCategory.ProductCategoryName
            };
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Product.Any(p => p.ProductCategoryID == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}