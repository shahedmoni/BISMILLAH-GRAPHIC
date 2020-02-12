using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class Selling
    {
        public Selling()
        {
            SellingList = new HashSet<SellingList>();
            SellingPaymentRecord = new HashSet<SellingPaymentRecord>();
        }

        public int SellingID { get; set; }
        public int RegistrationID { get; set; }
        public int VendorID { get; set; }
        public int SellingSN { get; set; }
        public double SellingTotalPrice { get; set; }
        public double? SellingDiscountAmount { get; set; }
        public double? SellingDiscountPercentage { get; set; }
        public double? SellingPaidAmount { get; set; }
        public double? SellingDueAmount { get; set; }
        public string SellingPaymentStatus { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime Insert_Date { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<SellingList> SellingList { get; set; }
        public virtual ICollection<SellingPaymentRecord> SellingPaymentRecord { get; set; }
    }
}
