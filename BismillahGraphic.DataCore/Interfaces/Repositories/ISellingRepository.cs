﻿using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface ISellingRepository : IRepository<Selling>
    {
        Selling Selling(SellingVM model);
        SellingReceipt Sold(int id);
        SellingUpdateViewModel FindUpdateBill(int id);
        void BillUpdated(SellingBillChangeViewModel model);
        bool DeleteBill(int id);
        int GetSellingSN();
        ICollection<int> Years();
        double SaleYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        double TotalDue();
        DataResult<SellingRecord> Records(DataRequest request);
        bool dueCollection(InvoicePaySingle model);
        CustomDataResult<SellingRecord> SellDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
        CustomDataResult<IncomeVM> IncomeDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
        ICollection<IncomeVM> IncomeDailyRecord(DateTime date);
        double IncomeDailyAmount(DateTime date);
    }
}
