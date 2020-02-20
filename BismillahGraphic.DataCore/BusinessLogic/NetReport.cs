using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class NetReport
    {
        private readonly IUnitOfWork _db;

        public NetReport(IUnitOfWork unitOfWork, int year)
        {
            DefaultYear = year;
            _db = unitOfWork;

            Years = YearsDdls();
            SaleYearly = GetSaleYearly(DefaultYear);
            ExpenseYearly = GetExpenseYearly(DefaultYear);
            NetYearly = SaleYearly - ExpenseYearly;
            TotalDue = GetDue();
            MonthlyReports = GetReports(DefaultYear);
        }

        //Public Properties
        public int DefaultYear { get; }
        public ICollection<DDL> Years { get; }
        public double SaleYearly { get; }
        public double ExpenseYearly { get; }
        public double NetYearly { get; }
        public double TotalDue { get; }
        public ICollection<MonthIncomeExpenseNet> MonthlyReports { get; }


        //Private Functions
        private ICollection<DDL> YearsDdls()
        {
            var years = _db.Expanses.Years();
            years = years.Union(_db.Selling.Years()).ToList();

            return years.Select(y => new DDL
            {
                value = y,
                label = "Year: " + y.ToString()
            }).ToList();
        }
        private double GetSaleYearly(int year)
        {
            return _db.Selling.SaleYearly(year);
        }
        private double GetExpenseYearly(int year)
        {
            return _db.Expanses.ExpenseYearly(year);
        }
        private double GetDue()
        {
            return _db.Selling.TotalDue();
        }
        private ICollection<MonthIncomeExpenseNet> GetReports(int year)
        {
            var months = new AllMonth();

            var result = (from m in months.AllMonthNames
                          join e in _db.Expanses.MonthlyAmounts(year) on m.MonthNumber equals e.MonthNumber
                                    into expanse
                          from e in expanse.DefaultIfEmpty()

                          join s in _db.Selling.MonthlyAmounts(year) on m.MonthNumber equals s.MonthNumber
                          into sell
                          from s in sell.DefaultIfEmpty()

                          select new MonthIncomeExpenseNet
                          {
                              Month = m.Month,
                              Income = s?.Amount ?? 0,
                              Expense = e?.Amount ?? 0,
                              Net = (s?.Amount ?? 0) - (e?.Amount ?? 0)
                          }).ToList();

            return result ?? new List<MonthIncomeExpenseNet>();
        }
    }
}
