using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IPageLinkRepository : IRepository<PageLink>
    {
        List<SideMenuCategory> GetSideMenuByUser(string userName);
    }
}
