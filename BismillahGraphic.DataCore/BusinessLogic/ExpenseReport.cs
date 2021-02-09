using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class ExpenseReport
    {
        private readonly DateTime _sDate;
        private readonly DateTime _eDate;
        private readonly IUnitOfWork _db;
        public ExpenseReport(IUnitOfWork unitOfWork, DateTime? sDate, DateTime? eDate)
        {
            _db = unitOfWork;
            _sDate = sDate ?? new DateTime(1000, 1, 1);
            _eDate = eDate ?? new DateTime(3000, 1, 1);
            GetExpenses();
            GetCategoryExpenses();
            this.TotalExpense = Math.Round(Expenses.Sum(e => e.ExpanseAmount), 2);


        }

        public ICollection<ExpanseVM> Expenses { get; private set; }
        public ICollection<ExpanseCategoryWise> ExpanseCategoryWises { get; private set; }
        public double TotalExpense { get; private set; }

        private void GetExpenses()
        {
            this.Expenses = _db.Expanses.DateToDate(this._sDate, this._eDate);
        }
        private void GetCategoryExpenses()
        {
            this.ExpanseCategoryWises = _db.Expanses.CategoryWistDateToDate(this._sDate, this._eDate);
        }
    }
}
