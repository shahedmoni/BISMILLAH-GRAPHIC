using System;
using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public class VendorVM
    {
        public int VendorID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string VendorCompanyName { get; set; }
        [Display(Name = "Name")]
        public string VendorName { get; set; }
        [Display(Name = "Address")]
        public string VendorAddress { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string VendorPhone { get; set; }
        [Required]
        [Display(Name = "SMS Number")]
        public string SmsNumber { get; set; }
        [Display(Name = "Add Date")]
        public DateTime Insert_Date { get; set; }
        [Display(Name = "Due")]
        public double VendorDue { get; set; }
    }

    public class VendorPaidDue
    {
        public int VendorID { get; set; }
        public string VendorCompanyName { get; set; }
        public double VendorDue { get; set; }
        public double VendorPaid { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
    }


}
