using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public partial class ExpanseCategory
    {
        public ExpanseCategory()
        {
            Expanse = new HashSet<Expanse>();
        }

        public int ExpanseCategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public double? TotalExpanse { get; set; }

        public virtual ICollection<Expanse> Expanse { get; set; }
    }
}
