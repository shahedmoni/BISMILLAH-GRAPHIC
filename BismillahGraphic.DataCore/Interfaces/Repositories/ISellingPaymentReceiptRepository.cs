using System;

namespace BismillahGraphic.DataCore
{
    public interface ISellingPaymentReceiptRepository : IRepository<SellingPaymentReceipt>
    {
        int GetReceiptSN();
        SellingPaymentReceipt dueCollection(PaymentReceipt model);
        PaymentReceiptPrint Print(int id);
        CustomDataResult<PaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
    }
}
