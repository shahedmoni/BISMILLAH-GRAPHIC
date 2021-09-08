using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public class ExpanseRepository : Repository<Expanse>, IExpanseRepository
    {
        public ExpanseRepository(DataContext context) : base(context)
        {
        }

        public DataResult<ExpanseVM> ToListCustom(DataRequest request)
        {
            var expanse = Context.Expanse.Include(e => e.ExpanseCategory).Select(e => new ExpanseVM
            {
                ExpanseID = e.ExpanseID,
                RegistrationID = e.RegistrationID,
                ExpanseCategoryID = e.ExpanseCategoryID,
                CategoryName = e.ExpanseCategory.CategoryName,
                ExpanseAmount = e.ExpanseAmount,
                ExpanseFor = e.ExpanseFor,
                Expense_Payment_Method = e.Expense_Payment_Method,
                ExpanseDate = e.ExpanseDate
            });
            return expanse.ToDataResult(request);
        }

        public async Task<ICollection<ExpanseVM>> ToListCustomAsync()
        {
            var expanse = await Context.Expanse.Include(e => e.ExpanseCategory).Select(e => new ExpanseVM
            {
                ExpanseID = e.ExpanseID,
                RegistrationID = e.RegistrationID,
                ExpanseCategoryID = e.ExpanseCategoryID,
                CategoryName = e.ExpanseCategory.CategoryName,
                ExpanseAmount = e.ExpanseAmount,
                ExpanseFor = e.ExpanseFor,
                Expense_Payment_Method = e.Expense_Payment_Method,
                ExpanseDate = e.ExpanseDate
            }).ToListAsync();

            return expanse;
        }

        public void AddCustom(ExpanseVM model)
        {
            Add(new Expanse
            {
                RegistrationID = model.RegistrationID,
                ExpanseCategoryID = model.ExpanseCategoryID,
                ExpanseAmount = model.ExpanseAmount,
                ExpanseFor = model.ExpanseFor,
                Expense_Payment_Method = model.Expense_Payment_Method,
                ExpanseDate = model.ExpanseDate
            });

            var eCategory = Context.ExpanseCategory.Find(model.ExpanseCategoryID);
            eCategory.TotalExpanse = eCategory.TotalExpanse + model.ExpanseAmount;
            Context.ExpanseCategory.Update(eCategory);
        }

        public void RemoveCustom(int id)
        {
            var expanse = Find(id);
            Remove(expanse);
            var eCategory = Context.ExpanseCategory.Find(expanse.ExpanseCategoryID);
            eCategory.TotalExpanse = eCategory.TotalExpanse - expanse.ExpanseAmount;
            Context.ExpanseCategory.Update(eCategory);
        }

        public ICollection<int> Years()
        {
            var years = new List<int>();

            years = Context.Expanse
                .GroupBy(e => new
                {
                    Year = e.ExpanseDate.Year
                })
                .Select(g => g.Key.Year)
                .OrderBy(o => o)
                .ToList();

            var currentYear = DateTime.Now.Year;

            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public double ExpenseYearly(int year)
        {
            return Math.Round(ToList().Where(s => s.ExpanseDate.Year == year).Sum(s => s.ExpanseAmount), 2);
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Expanse.Where(e => e.ExpanseDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.ExpanseDate.Month

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = Math.Round(g.Sum(e => e.ExpanseAmount), 2)
                })
                .ToList();

            return months ?? new List<MonthlyAmount>(); ;
        }

        public ICollection<ExpanseVM> DateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var expanse = Context.Expanse.Include(e => e.ExpanseCategory).Where(e => e.ExpanseDate <= eD && e.ExpanseDate >= sD).Select(e => new ExpanseVM
            {
                ExpanseID = e.ExpanseID,
                RegistrationID = e.RegistrationID,
                ExpanseCategoryID = e.ExpanseCategoryID,
                CategoryName = e.ExpanseCategory.CategoryName,
                ExpanseAmount = e.ExpanseAmount,
                ExpanseFor = e.ExpanseFor,
                Expense_Payment_Method = e.Expense_Payment_Method,
                ExpanseDate = e.ExpanseDate
            }).ToList();

            return expanse;
        }
        public ICollection<ExpanseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var ex = Context.Expanse.Include(e => e.ExpanseCategory).Where(e => e.ExpanseDate <= eD && e.ExpanseDate >= sD)
                .GroupBy(e => new
                {
                    ExpanseCategoryID = e.ExpanseCategoryID,
                    CategoryName = e.ExpanseCategory.CategoryName

                })
                .Select(g => new ExpanseCategoryWise
                {
                    ExpanseCategoryID = g.Key.ExpanseCategoryID,
                    CategoryName = g.Key.CategoryName,
                    TotalExpanse = Math.Round(g.Sum(e => e.ExpanseAmount), 2)

                })
                .ToList();

            return ex;
        }

        public ICollection<ExpanseVM> DailyRecord(DateTime date)
        {
            return Context.Expanse
                .Include(e => e.ExpanseCategory)
                .Where(e => e.ExpanseDate == date)
                .Select(e => new ExpanseVM
                {
                    ExpanseID = e.ExpanseID,
                    RegistrationID = e.RegistrationID,
                    ExpanseCategoryID = e.ExpanseCategoryID,
                    CategoryName = e.ExpanseCategory.CategoryName,
                    ExpanseAmount = e.ExpanseAmount,
                    ExpanseFor = e.ExpanseFor,
                    Expense_Payment_Method = e.Expense_Payment_Method,
                    ExpanseDate = e.ExpanseDate
                }).ToList();
        }

        public double DailyAmount(DateTime date)
        {
            return Context.Expanse
                  .Where(e => e.ExpanseDate == date)
                  .Sum(e => e.ExpanseAmount);
        }
    }
}
