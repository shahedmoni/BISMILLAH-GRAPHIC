using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public enum SellingStatus
    {
        Failed = 0,
        Success = 1
    }
    public class SellingVM
    {
        public SellingVM()
        {
            SellingCarts = new HashSet<SellingCart>();
        }
        public int RegistrationID { get; set; }
        public int VendorID { get; set; }
        public int ReceiptSN { get; set; }
        public int SellingSN { get; set; }
        public double SellingTotalPrice { get; set; }
        public double? SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public string Payment_Situation { get; set; }
        public ICollection<SellingCart> SellingCarts { get; set; }
    }

    public class SellingCart
    {
        public int ProductID { get; set; }
        public double SellingQuantity { get; set; }
        public double SellingUnitPrice { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class SellingReceipt
    {
        public InstitutionVM InstitutionInfo { get; set; }
        public Selling SellingInfo { get; set; }
        public VendorVM VendorInfo { get; set; }
        public string SoildBy { get; set; }
    }


    public class PaymentReceiptPrint
    {
        public PaymentReceiptPrint()
        {
            this.Invoices = new HashSet<PaidInvoiceList>();
        }
        public ICollection<PaidInvoiceList> Invoices { get; set; }
        public InstitutionVM InstitutionInfo { get; set; }
        public VendorVM VendorInfo { get; set; }
        public int ReceiptID { get; set; }
        public DateTime PaidDate { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public string CollectBy { get; set; }


    }


    public class PaidInvoiceList
    {
        public int SellingID { get; set; }
        public int SellingSN { get; set; }
        public double SellingAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public DateTime SellingDate { get; set; }
    }

    public class PaymentReceiptList
    {
        public int ReceiptID { get; set; }
        public DateTime Date { get; set; }
        public string Vendor { get; set; }
        public int Receipt { get; set; }
        public double Amount { get; set; }
    }

    public class PaymentReceipt
    {
        public PaymentReceipt()
        {
            this.Invoices = new HashSet<InvoicePay>();
        }
        public int ReceiptID { get; set; }
        public int RegistrationID { get; set; }
        public int VendorID { get; set; }
        public int ReceiptSN { get; set; }
        public double PaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime Paid_Date { get; set; }
        public ICollection<InvoicePay> Invoices { get; set; }
    }
    public class InvoicePay
    {
        public int SellingID { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }


    }

    public class InvoicePaySingle : InvoicePay
    {
        public int VendorID { get; set; }
        public int RegistrationID { get; set; }
        public int ReceiptSN { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime SellingPaid_Date { get; set; }
    }
    public class IncomeVM
    {
        public int SellingID { get; set; }
        public int VendorID { get; set; }
        public string VendorCompanyName { get; set; }
        public int SellingSN { get; set; }
        public double SellingPaidAmount { get; set; }
        public string Payment_Situation { get; set; }
        public DateTime SellingPaid_Date { get; set; }
        public string ReceivedBy { get; set; }
    }
    public class SellingRecord
    {
        public int SellingID { get; set; }
        public int VendorID { get; set; }
        public string VendorCompanyName { get; set; }
        public int SellingSN { get; set; }
        public double SellingAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
    }

    public class SellingUpdateViewModel
    {
        public SellingUpdateViewModel()
        {
            SellingCarts = new HashSet<SellingUpdateCart>();
        }
        public VendorVM VendorInfo { get; set; }
        public int SellingID { get; set; }
        public int SellingSN { get; set; }
        public double SellingTotalPrice { get; set; }
        public double? SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public ICollection<SellingUpdateCart> SellingCarts { get; set; }
    }
    public class SellingUpdateCart
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double SellingQuantity { get; set; }
        public double SellingUnitPrice { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class SellingBillChangeViewModel
    {
        public SellingBillChangeViewModel()
        {
            SellingCarts = new HashSet<SellingCart>();
        }
        public int SellingID { get; set; }
        public double SellingTotalPrice { get; set; }
        public double? SellingDiscountAmount { get; set; }
        public ICollection<SellingCart> SellingCarts { get; set; }
    }
}
