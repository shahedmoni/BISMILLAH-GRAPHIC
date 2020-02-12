using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class Registration
    {
        public Registration()
        {
            PageLinkAssign = new HashSet<PageLinkAssign>();
        }

        public int RegistrationID { get; set; }
        public string UserName { get; set; }
        public bool Validation { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public string DateofBirth { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string PS { get; set; }
        public virtual ICollection<PageLinkAssign> PageLinkAssign { get; set; }
        public virtual ICollection<SellingPaymentRecord> SellingPaymentRecord { get; set; }
        public virtual ICollection<SellingPaymentReceipt> SellingPaymentReceipt { get; set; }
    }
}
