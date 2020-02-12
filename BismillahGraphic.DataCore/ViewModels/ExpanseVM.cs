using System;
using System.ComponentModel.DataAnnotations;

namespace BismillahGraphic.DataCore
{
    public class ExpanseVM
    {
        public int ExpanseID { get; set; }
        [Required]
        public int RegistrationID { get; set; }
        [Required]
        public int ExpanseCategoryID { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public double ExpanseAmount { get; set; }
        [Display(Name = "Expense For")]
        public string ExpanseFor { get; set; }
        [Display(Name = "Payment Method")]
        public string Expense_Payment_Method { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "d mmm yyyy")]
        public DateTime ExpanseDate { get; set; }
    }

    public class ExpanseCategoryWise
    {
        public int ExpanseCategoryID { get; set; }
        public string CategoryName { get; set; }
        public double TotalExpanse { get; set; }
    }
}
