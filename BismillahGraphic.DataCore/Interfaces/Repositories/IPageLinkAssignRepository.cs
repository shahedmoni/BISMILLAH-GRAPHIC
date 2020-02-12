using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IPageLinkAssignRepository : IRepository<PageLinkAssign>
    {
        ICollection<PageCategoryVM> SubAdminLinks(int regId);

        string AssignLink(int regId, ICollection<PageAssignVM> links);
    }
}
