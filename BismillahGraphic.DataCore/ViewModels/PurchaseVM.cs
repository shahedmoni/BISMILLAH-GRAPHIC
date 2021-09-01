using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public enum PurchaseStatus
    {
        Failed = 0,
        Success = 1
    }
    public class PurchaseVM
    {
        public PurchaseVM()
        {
            PurchaseCarts = new HashSet<PurchaseCart>();
        }
        public int RegistrationID { get; set; }
        public int SupplierID { get; set; }
        public int ReceiptSN { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double? PurchaseDiscountAmount { get; set; }
        public double PurchasePaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Payment_Situation { get; set; }
        public ICollection<PurchaseCart> PurchaseCarts { get; set; }
    }

    public class PurchaseCart
    {
        public int ProductID { get; set; }
        public int MeasurementUnitId { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchaseUnitPrice { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class PurchaseReceipt
    {
        public InstitutionVM InstitutionInfo { get; set; }
        public Purchase PurchaseInfo { get; set; }
        public SupplierVM SupplierInfo { get; set; }
        public string SoildBy { get; set; }
    }


    public class PurchasePaymentReceiptPrint
    {
        public PurchasePaymentReceiptPrint()
        {
            this.Invoices = new HashSet<PurchasePaidInvoiceList>();
        }
        public ICollection<PurchasePaidInvoiceList> Invoices { get; set; }
        public InstitutionVM InstitutionInfo { get; set; }
        public SupplierVM SupplierInfo { get; set; }
        public int PurchaseReceiptID { get; set; }
        public DateTime PaidDate { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public string CollectBy { get; set; }


    }


    public class PurchasePaidInvoiceList
    {
        public int PurchaseID { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchaseAmount { get; set; }
        public double PurchasePaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class PurchasePaymentReceiptList
    {
        public int PurchaseReceiptID { get; set; }
        public DateTime Date { get; set; }
        public string Supplier { get; set; }
        public int Receipt { get; set; }
        public double Amount { get; set; }
    }

    public class PurchasePaymentReceiptModel
    {
        public PurchasePaymentReceiptModel()
        {
            this.Invoices = new HashSet<PurchaseInvoicePay>();
        }
        public int PurchaseReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public int SupplierID { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime Paid_Date { get; set; }
        public ICollection<PurchaseInvoicePay> Invoices { get; set; }
    }
    public class PurchaseInvoicePay
    {
        public int PurchaseID { get; set; }
        public double PurchasePaidAmount { get; set; }
        public double PurchaseDiscountAmount { get; set; }


    }

    public class PurchaseInvoicePaySingle : InvoicePay
    {
        public int SupplierID { get; set; }
        public int RegistrationID { get; set; }
        public int ReceiptSN { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime PurchasePaid_Date { get; set; }
    }
    public class PurchaseIncomeVM
    {
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierCompanyName { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchasePaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime PurchasePaid_Date { get; set; }
        public string ReceivedBy { get; set; }
    }
    public class PurchaseRecord
    {
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierCompanyName { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchaseAmount { get; set; }
        public double PurchasePaidAmount { get; set; }
        public double PurchaseDiscountAmount { get; set; }
        public double PurchaseDueAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class PurchaseUpdateViewModel
    {
        public PurchaseUpdateViewModel()
        {
            PurchaseCarts = new HashSet<PurchaseUpdateCart>();
        }
        public SupplierVM SupplierInfo { get; set; }
        public int PurchaseID { get; set; }
        public int PurchaseSN { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double? PurchaseDiscountAmount { get; set; }
        public double PurchasePaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<PurchaseUpdateCart> PurchaseCarts { get; set; }
    }
    public class PurchaseUpdateCart
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchaseUnitPrice { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class PurchaseBillChangeViewModel
    {
        public PurchaseBillChangeViewModel()
        {
            PurchaseCarts = new HashSet<PurchaseCart>();
        }
        public int PurchaseID { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double? PurchaseDiscountAmount { get; set; }
        public ICollection<PurchaseCart> PurchaseCarts { get; set; }
    }
}
