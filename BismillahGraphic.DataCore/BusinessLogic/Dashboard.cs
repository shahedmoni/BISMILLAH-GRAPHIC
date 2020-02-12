using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class Dashboard
    {
        private readonly IUnitOfWork _db;

        public Dashboard(IUnitOfWork unitOfWork)
        {
            DefaultYear = DateTime.Now.Year;
            _db = unitOfWork;

            Years = YearsDdls();
            SaleYearly = GetSaleYearly(DefaultYear);
            ExpenseYearly = GetExpenseYearly(DefaultYear);
            NetYearly = SaleYearly - ExpenseYearly;
            TotalDue = GetDue();
            Vendors = GetVendors();
            MonthlyReports = GetReports(DefaultYear);
        }
        public Dashboard(IUnitOfWork unitOfWork, int year)
        {
            DefaultYear = year;
            _db = unitOfWork;

            Years = YearsDdls();
            SaleYearly = GetSaleYearly(DefaultYear);
            ExpenseYearly = GetExpenseYearly(DefaultYear);
            NetYearly = SaleYearly - ExpenseYearly;
            TotalDue = GetDue();
            Vendors = GetVendors();
            MonthlyReports = GetReports(DefaultYear);
        }

        //Public Properties
        public int DefaultYear { get; }
        public ICollection<DDL> Years { get; }
        public double SaleYearly { get; }
        public double ExpenseYearly { get; }
        public double NetYearly { get; }
        public double TotalDue { get; }
        public ICollection<VendorPaidDue> Vendors { get; }
        public ICollection<MonthIncomeExpense> MonthlyReports { get; }

        public string JsonChartData { get; set; }


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
        private ICollection<VendorPaidDue> GetVendors()
        {
            return _db.Vendors.TopDue() ?? new List<VendorPaidDue>();
        }
        private ICollection<MonthIncomeExpense> GetReports(int year)
        {
            var months = new AllMonth();

            var a = months.AllMonthNames;
            var b = _db.Expanses.MonthlyAmounts(year);
            var c = _db.Selling.MonthlyAmounts(year);

            var result = (from m in months.AllMonthNames
                              //from e in _db.Expanses.MonthlyAmounts(year).Where(x => x.MonthNumber == m.MonthNumber).DefaultIfEmpty()
                              //from s in _db.Selling.MonthlyAmounts(year).Where(i => i.MonthNumber == m.MonthNumber).DefaultIfEmpty()
                          join e in _db.Expanses.MonthlyAmounts(year) on m.MonthNumber equals e.MonthNumber
                          into expanse
                          from e in expanse.DefaultIfEmpty()

                          join s in _db.Selling.MonthlyAmounts(year) on m.MonthNumber equals s.MonthNumber
                          into sell
                          from s in sell.DefaultIfEmpty()

                          select new MonthIncomeExpense
                          {
                              Month = m.Month,
                              Income = s?.Amount ?? 0,
                              Expense = e?.Amount ?? 0
                          }).ToList();

            return result ?? new List<MonthIncomeExpense>(); ;
        }


    }
}
