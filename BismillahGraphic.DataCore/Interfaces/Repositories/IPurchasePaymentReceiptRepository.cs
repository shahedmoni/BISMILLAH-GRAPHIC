using System;

namespace BismillahGraphic.DataCore
{
    public interface IPurchasePaymentReceiptRepository : IRepository<PurchasePaymentReceipt>
    {
        int GetReceiptSN();
        PurchasePaymentReceipt dueCollection(PurchasePaymentReceiptModel model);
        PurchasePaymentReceiptPrint Print(int id);
        CustomDataResult<PurchasePaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
    }
}
