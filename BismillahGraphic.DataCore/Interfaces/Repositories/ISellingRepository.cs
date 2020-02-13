using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface ISellingRepository : IRepository<Selling>
    {
        Selling Selling(SellingVM model);
        SellingReceipt Sold(int id);
        int GetSellingSN();
        ICollection<int> Years();
        double SaleYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        double TotalDue();
        DataResult<SellingRecord> Records(DataRequest request);
        bool dueCollection(SellingDuePay model);
        DataResult<SellingRecord> SellDateToDate(DataRequest request, DateTime? sDateTime, DateTime? eDateTime);
        ICollection<IncomeVM> IncomeDateToDate(DateTime? sDateTime, DateTime? eDateTime);
    }
}
