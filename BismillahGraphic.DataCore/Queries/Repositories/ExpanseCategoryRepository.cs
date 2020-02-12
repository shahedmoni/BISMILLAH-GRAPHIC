using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class ExpanseCategoryRepository : Repository<ExpanseCategory>, IExpanseCategoryRepository
    {
        public ExpanseCategoryRepository(DataContext context) : base(context)
        {
        }

        public ICollection<DDL> ddl()
        {
            return Context.ExpanseCategory.Select(e => new DDL
            {
                value = e.ExpanseCategoryID,
                label = e.CategoryName
            }).ToList();
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Expanse.Any(e => e.ExpanseCategoryID == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}
