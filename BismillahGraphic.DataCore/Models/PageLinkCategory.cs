using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class PageLinkCategory
    {
        public PageLinkCategory()
        {
            PageLink = new HashSet<PageLink>();
        }

        public int LinkCategoryID { get; set; }
        public string Category { get; set; }
        public string IconClass { get; set; }
        public int? SN { get; set; }

        public virtual ICollection<PageLink> PageLink { get; set; }
    }
}
