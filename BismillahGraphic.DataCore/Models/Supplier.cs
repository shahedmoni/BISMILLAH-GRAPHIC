using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class Supplier
    {
        public Supplier()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int SupplierID { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierPhone { get; set; }
        public string SmsNumber { get; set; }
        public DateTime Insert_Date { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double SupplierPaid { get; set; }
        public double SupplierDue { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual ICollection<PurchasePaymentReceipt> PurchasePaymentReceipt { get; set; }
    }
}