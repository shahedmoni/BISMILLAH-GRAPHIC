using System;

namespace BismillahGraphic.DataCore
{
    public partial class SellingPaymentRecord
    {
        public int SellingPaymentRecordID { get; set; }
        public int SellingID { get; set; }
        public int ReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public double SellingPaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public string Description { get; set; }
        public DateTime SellingPaid_Date { get; set; }
        public DateTime Insert_Date { get; set; }

        public SellingPaymentReceipt SellingPaymentReceipt { get; set; }
        public virtual Selling Selling { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
