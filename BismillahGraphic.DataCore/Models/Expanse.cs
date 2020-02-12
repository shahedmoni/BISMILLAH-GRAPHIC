using System;

namespace BismillahGraphic.DataCore
{
    public partial class Expanse
    {
        public int ExpanseID { get; set; }
        public int RegistrationID { get; set; }
        public int ExpanseCategoryID { get; set; }
        public double ExpanseAmount { get; set; }
        public string ExpanseFor { get; set; }
        public string Expense_Payment_Method { get; set; }
        public DateTime ExpanseDate { get; set; }
        public DateTime? Insert_Date { get; set; }

        public virtual ExpanseCategory ExpanseCategory { get; set; }
    }
}
