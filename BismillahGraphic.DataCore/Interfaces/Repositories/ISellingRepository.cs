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
        CustomDataResult<SellingRecord> SellDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
        CustomDataResult<IncomeVM> IncomeDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
    }
}
