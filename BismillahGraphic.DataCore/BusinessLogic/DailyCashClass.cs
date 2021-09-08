using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class DailyCashClass
    {
        private readonly IUnitOfWork _db;
        public DailyCashClass(IUnitOfWork db, DateTime date)
        {
            _db = db;

            ExpanseRecords = _db.Expanses.DailyRecord(date);
            IncomeRecords = _db.Selling.IncomeDailyRecord(date);
            Income = _db.Selling.IncomeDailyAmount(date);
            Expense = _db.Expanses.DailyAmount(date);
            Net = Income - Expense;
        }

        public ICollection<IncomeVM> IncomeRecords { get; set; }
        public ICollection<ExpanseVM> ExpanseRecords { get; set; }
        public double Income { get; set; }
        public double Expense { get; set; }
        public double Net { get; set; }
    }
}