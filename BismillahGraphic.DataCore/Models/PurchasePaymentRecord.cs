using System;

namespace BismillahGraphic.DataCore
{
    public class PurchasePaymentRecord
    {
        public int PurchasePaymentRecordID { get; set; }
        public int PurchaseID { get; set; }
        public int PurchaseReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public double PurchasePaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime PurchasePaid_Date { get; set; }
        public DateTime Insert_Date { get; set; }

        public PurchasePaymentReceipt PurchasePaymentReceipt { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual Registration Registration { get; set; }
    }
}