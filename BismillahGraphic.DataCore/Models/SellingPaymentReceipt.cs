using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class SellingPaymentReceipt
    {
        public SellingPaymentReceipt()
        {
            this.SellingPaymentRecord = new HashSet<SellingPaymentRecord>();
        }
        public int ReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public int VendorID { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime Paid_Date { get; set; }
        public DateTime Insert_Date { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Registration Registration { get; set; }
        public ICollection<SellingPaymentRecord> SellingPaymentRecord { get; set; }
    }
}
