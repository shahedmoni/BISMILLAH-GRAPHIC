using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class Purchase
    {
        public Purchase()
        {
            PurchaseList = new HashSet<PurchaseList>();
            PurchasePaymentRecord = new HashSet<PurchasePaymentRecord>();
        }

        public int PurchaseID { get; set; }
        public int RegistrationID { get; set; }
        public int SupplierID { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double? PurchaseDiscountAmount { get; set; }
        public double? PurchaseDiscountPercentage { get; set; }
        public double? PurchasePaidAmount { get; set; }
        public double? PurchaseDueAmount { get; set; }
        public string PurchasePaymentStatus { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime Insert_Date { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseList> PurchaseList { get; set; }
        public virtual ICollection<PurchasePaymentRecord> PurchasePaymentRecord { get; set; }
    }
}