using System;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class SellingPaymentReceiptRepository : Repository<SellingPaymentReceipt>, ISellingPaymentReceiptRepository
    {
        public SellingPaymentReceiptRepository(DataContext context) : base(context)
        {
        }

        public int GetReceiptSN()
        {
            var sn = 0;
            if (Context.SellingPaymentReceipt.Any())
            {
                sn = Context.SellingPaymentReceipt.Max(s => s == null ? 0 : s.ReceiptSN);
            }

            return sn + 1;
        }

        public PaymentReceiptPrint Print(int id)
        {
            throw new NotImplementedException();
        }

        public CustomDataResult<PaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime)
        {
            throw new NotImplementedException();
        }

        public SellingPaymentReceipt AddCustom(PaymentReceipt model)
        {
            var receipt = new SellingPaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                VendorID = model.VendorID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.PaidAmount,
                Payment_Situation = model.Payment_Situation,
                Paid_Date = model.Paid_Date,
                SellingPaymentRecord = model.Invoices.Select(i => new SellingPaymentRecord
                {
                    SellingID = i.SellingID,
                    RegistrationID = model.RegistrationID,
                    SellingPaidAmount = i.SellingPaidAmount,
                    Payment_Situation = i.Payment_Situation,
                    SellingPaid_Date = i.SellingPaid_Date

                }).ToList()
            };

            Add(receipt);
            return receipt;
        }


        public bool dueCollection(PaymentReceipt model)
        {
            foreach (var invoice in model.Invoices)
            {
                var sell = Context.Selling.Find(invoice.SellingID);
                if (sell.SellingDueAmount < invoice.SellingPaidAmount) return false;
            }

            var receipt = new SellingPaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                VendorID = model.VendorID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.PaidAmount,
                Payment_Situation = model.Payment_Situation,
                Paid_Date = model.Paid_Date,
                SellingPaymentRecord = model.Invoices.Select(i => new SellingPaymentRecord
                {
                    SellingID = i.SellingID,
                    RegistrationID = model.RegistrationID,
                    SellingPaidAmount = i.SellingPaidAmount,
                    Payment_Situation = i.Payment_Situation,
                    SellingPaid_Date = i.SellingPaid_Date
                }).ToList()
            };

            Context.SellingPaymentReceipt.Add(receipt);

            foreach (var invoice in model.Invoices)
            {
                var sell = Context.Selling.Find(invoice.SellingID);
                sell.SellingPaidAmount += invoice.SellingPaidAmount;
                Context.Selling.Update(sell);
            }
            return true;
        }
    }
}
