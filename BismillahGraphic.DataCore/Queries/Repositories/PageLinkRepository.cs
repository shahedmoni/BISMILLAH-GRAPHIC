using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    internal class PageLinkRepository : Repository<PageLink>, IPageLinkRepository
    {
        public PageLinkRepository(DataContext context) : base(context)
        {

        }

        public List<SideMenuCategory> GetSideMenuByUser(string userName)
        {

            if (string.IsNullOrEmpty(userName)) return null;

            var reg = Context.Registration.FirstOrDefault(r => r.UserName == userName);
            if (reg.Type == "Admin")
            {
                var menu = (from p in Context.PageLinkCategory
                            orderby p.SN
                            select new SideMenuCategory
                            {
                                LinkCategoryID = p.LinkCategoryID,
                                Category = p.Category,
                                IconClass = p.IconClass,
                                SN = p.SN,
                                links = p.PageLink.Select(l => new SideMenuLink
                                {
                                    LinkID = l.LinkID,
                                    SN = l.SN,
                                    Action = l.Action,
                                    Controller = l.Controller,
                                    IconClass = l.IconClass,
                                    Title = l.Title
                                }).OrderBy(l => l.SN).ToList()
                            }).ToList();
                return menu;
            }
            else
            {
                var menu = (from p in Context.PageLinkAssign
                            join c in Context.PageLinkCategory
                            on p.PageLink.LinkCategory.LinkCategoryID equals c.LinkCategoryID
                            where p.RegistrationID == reg.RegistrationID
                            orderby p.PageLink.LinkCategory.SN
                            select new SideMenuCategory
                            {
                                LinkCategoryID = c.LinkCategoryID,
                                Category = c.Category,
                                IconClass = c.IconClass,
                                SN = c.SN
                            }).Distinct().ToList();

                foreach (var item in menu)
                {
                    item.links = Context.PageLinkAssign.Where(l => l.PageLink.LinkCategoryID == item.LinkCategoryID && l.RegistrationID == reg.RegistrationID).Select(l => new SideMenuLink
                    {
                        LinkID = l.PageLink.LinkID,
                        SN = l.PageLink.SN,
                        Action = l.PageLink.Action,
                        Controller = l.PageLink.Controller,
                        IconClass = l.PageLink.IconClass,
                        Title = l.PageLink.Title
                    }).ToList();
                }

                return menu;
            }

        }
    }
}