using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>, IAddCustom<ProductCategoryVM>
    {
        ICollection<DDL> ddl();
        // void AddCustom(ProductCategoryVM model);
        void UpdateCustom(ProductCategoryVM model);
        ProductCategoryVM FindCustom(int? id);
        bool RemoveCustom(int id);

    }
}
