using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public interface IExpanseRepository : IRepository<Expanse>, IAddCustom<ExpanseVM>
    {
        DataResult<ExpanseVM> ToListCustom(DataRequest request);
        Task<ICollection<ExpanseVM>> ToListCustomAsync();
        // void AddCustom(ExpanseVM model);
        void RemoveCustom(int id);
        ICollection<int> Years();
        double ExpenseYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);

        ICollection<ExpanseVM> DateToDate(DateTime? sDateTime, DateTime? eDateTime);

        ICollection<ExpanseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime);
        ICollection<ExpanseVM> DailyRecord(DateTime date);
        double DailyAmount(DateTime date);

    }
}
