using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class PurchasePaymentReceipt
    {
        public PurchasePaymentReceipt()
        {
            this.PurchasePaymentRecord = new HashSet<PurchasePaymentRecord>();
        }
        public int PurchaseReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public int SupplierID { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public string Description { get; set; }

        public DateTime Paid_Date { get; set; }
        public DateTime Insert_Date { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Registration Registration { get; set; }
        public ICollection<PurchasePaymentRecord> PurchasePaymentRecord { get; set; }
    }
}