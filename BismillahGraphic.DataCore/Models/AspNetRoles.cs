using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            PageLink = new HashSet<PageLink>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<PageLink> PageLink { get; set; }
    }
}
