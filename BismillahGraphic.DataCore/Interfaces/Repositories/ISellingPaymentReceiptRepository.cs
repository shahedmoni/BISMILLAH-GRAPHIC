using System;

namespace BismillahGraphic.DataCore
{
    public interface ISellingPaymentReceiptRepository : IRepository<SellingPaymentReceipt>
    {
        int GetReceiptSN();
        SellingPaymentReceipt AddCustom(PaymentReceipt model);
        bool dueCollection(PaymentReceipt model);
        PaymentReceiptPrint Print(int id);
        CustomDataResult<PaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime);
    }
}
