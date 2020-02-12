using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IPageLinkCategoryRepository : IRepository<PageLinkCategory>
    {
        ICollection<PageLinkCategory> GetCategoryWithLink();

        PageLink LinkRoleUpdate(int linkId, string roleId);
        ICollection<DDL> ddl();
    }
}
