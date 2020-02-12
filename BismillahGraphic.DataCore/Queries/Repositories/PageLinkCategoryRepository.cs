using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class PageLinkCategoryRepository : Repository<PageLinkCategory>, IPageLinkCategoryRepository
    {
        public PageLinkCategoryRepository(DataContext context) : base(context)
        {

        }

        public ICollection<PageLinkCategory> GetCategoryWithLink()
        {
            return Context.PageLinkCategory.Include(p => p.PageLink).OrderBy(p => p.SN).ToList();
        }

        public PageLink LinkRoleUpdate(int linkId, string roleId)
        {
            var linkage = Context.PageLink.Find(linkId);
            linkage.RoleId = roleId;
            return linkage;
        }

        public ICollection<DDL> ddl()
        {
            return Context.PageLinkCategory.Select(c => new DDL
            {
                value = c.LinkCategoryID,
                label = c.Category
            }).ToList();
        }
    }
}