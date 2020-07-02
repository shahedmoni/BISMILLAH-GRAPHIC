using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public partial class Vendor
    {
        public Vendor()
        {
            Selling = new HashSet<Selling>();
        }

        public int VendorID { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorPhone { get; set; }
        public string SmsNumber { get; set; }
        public DateTime Insert_Date { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double VendorPaid { get; set; }
        public double VendorDue { get; set; }
        public virtual ICollection<Selling> Selling { get; set; }
        public virtual ICollection<SellingPaymentReceipt> SellingPaymentReceipt { get; set; }
    }
}
