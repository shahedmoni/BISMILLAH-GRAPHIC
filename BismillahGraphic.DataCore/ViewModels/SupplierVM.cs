using System;
using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public class SupplierVM
    {
        public int SupplierID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string SupplierCompanyName { get; set; }
        [Display(Name = "Name")]
        public string SupplierName { get; set; }
        [Display(Name = "Address")]
        public string SupplierAddress { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string SupplierPhone { get; set; }

        [Required]
        [Display(Name = "SMS Number")]
        [RegularExpression(@"^(88)?((011)|(015)|(016)|(017)|(018)|(019)|(013)|(014))\d{8,8}$", ErrorMessage = "Invalid Mobile Number.")]
        public string SmsNumber { get; set; }
        [Display(Name = "Add Date")]
        public DateTime Insert_Date { get; set; }
        [Display(Name = "Due")]
        public double SupplierDue { get; set; }
    }

    public class SupplierPaidDue
    {
        public int SupplierID { get; set; }
        public string SupplierCompanyName { get; set; }
        public double SupplierDue { get; set; }
        public double SupplierPaid { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
    }


}
