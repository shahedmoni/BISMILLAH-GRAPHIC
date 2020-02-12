using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class SideMenuCategory
    {
        public SideMenuCategory()
        {
            this.links = new HashSet<SideMenuLink>();
        }
        public int LinkCategoryID { get; set; }
        public int? SN { get; set; }
        public string Category { get; set; }
        public string IconClass { get; set; }
        public ICollection<SideMenuLink> links { get; set; }

    }
    public class SideMenuLink
    {
        public int LinkID { get; set; }
        public int? SN { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }

    }
}
