using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IExpanseCategoryRepository : IRepository<ExpanseCategory>
    {
        ICollection<DDL> ddl();
        bool RemoveCustom(int id);
    }
}
