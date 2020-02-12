using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class PageLinkAssignRepository : Repository<PageLinkAssign>, IPageLinkAssignRepository
    {
        public PageLinkAssignRepository(DataContext context) : base(context)
        {
        }

        public string AssignLink(int regId, ICollection<PageAssignVM> links)
        {
            var pAssigns = links.Select(l => new PageLinkAssign
            {
                LinkID = l.LinkID,
                RegistrationID = regId
            }).ToList();
            var pDelete = Context.PageLinkAssign.Where(p => p.RegistrationID == regId).ToList();
            Context.PageLinkAssign.RemoveRange(pDelete);
            Context.PageLinkAssign.AddRange(pAssigns);
            return Context.Registration.Find(regId).UserName;
        }

        public ICollection<PageCategoryVM> SubAdminLinks(int regId)
        {

            var userDll = (from c in Context.PageLinkCategory
                           select new PageCategoryVM
                           {
                               Category = c.Category,
                               Links = (from l in Context.PageLink
                                        join r in Context.AspNetRoles
                                            on l.RoleId equals r.Id
                                        where l.LinkCategoryID == c.LinkCategoryID
                                        select new PageLinkVM
                                        {
                                            LinkID = l.LinkID,
                                            Title = l.Title,
                                            RoleName = r.Name,
                                            IsAssign = (from a in Context.PageLinkAssign where a.LinkID == l.LinkID && a.RegistrationID == regId select a.LinkAssignID).Any()
                                        }).ToList()
                           }).ToList();
            return userDll;
        }
    }
}
