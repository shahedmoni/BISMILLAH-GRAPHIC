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

    public class SellingDuePay
    {
        public int SellingID { get; set; }
        public int VendorID { get; set; }
        public int RegistrationID { get; set; }
        public double SellingPaidAmount { get; set; }
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


}
