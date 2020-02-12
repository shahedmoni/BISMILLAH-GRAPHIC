using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class PageLink
    {
        public PageLink()
        {
            PageLinkAssign = new HashSet<PageLinkAssign>();
        }

        public int LinkID { get; set; }
        public int LinkCategoryID { get; set; }
        public string RoleId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public int? SN { get; set; }

        public virtual PageLinkCategory LinkCategory { get; set; }
        public virtual AspNetRoles Role { get; set; }
        public virtual ICollection<PageLinkAssign> PageLinkAssign { get; set; }
    }
}
