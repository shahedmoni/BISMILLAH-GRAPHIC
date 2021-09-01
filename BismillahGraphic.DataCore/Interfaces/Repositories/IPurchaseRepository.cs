using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Purchase Purchase(PurchaseVM model);
        PurchaseReceipt Sold(int id);
        PurchaseUpdateViewModel FindUpdateBill(int id);
        void BillUpdated(PurchaseBillChangeViewModel model);
        bool DeleteBill(int id);
        int GetPurchaseSN();
        ICollection<int> Years();
        double SaleYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        double TotalDue();
        DataResult<PurchaseRecord> Records(DataRequest request);
        bool dueCollection(PurchaseInvoicePaySingle model);
        CustomDataResult<PurchaseRecord> SellDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
        CustomDataResult<PurchaseIncomeVM> IncomeDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
    }
}
