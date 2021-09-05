using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class PurchasePaymentReceiptRepository : Repository<PurchasePaymentReceipt>, IPurchasePaymentReceiptRepository
    {
        public PurchasePaymentReceiptRepository(DataContext context) : base(context)
        {
        }

        public int GetReceiptSN()
        {
            var sn = 0;
            if (Context.PurchasePaymentReceipt.Any())
            {
                sn = Context.PurchasePaymentReceipt.Max(s => s == null ? 0 : s.ReceiptSN);
            }

            return sn + 1;
        }

        public PurchasePaymentReceiptPrint Print(int id)
        {
            var receipt = Context.PurchasePaymentReceipt.Where(r => r.PurchaseReceiptID == id).Select(r => new PurchasePaymentReceiptPrint
            {
                Invoices = r.PurchasePaymentRecord.Select(p => new PurchasePaidInvoiceList
                {
                    PurchaseID = p.PurchaseID,
                    PurchaseSN = p.Purchase.PurchaseSN,
                    PurchaseAmount = p.Purchase.PurchaseTotalPrice,
                    PurchasePaidAmount = p.PurchasePaidAmount,
                    PurchaseDate = p.Purchase.PurchaseDate
                }).ToList(),

                SupplierInfo = new SupplierVM
                {
                    SupplierID = r.SupplierID,
                    SupplierCompanyName = r.Supplier.SupplierCompanyName,
                    SupplierName = r.Supplier.SupplierName,
                    SupplierAddress = r.Supplier.SupplierAddress,
                    SupplierPhone = r.Supplier.SupplierPhone,
                    Insert_Date = r.Supplier.Insert_Date,
                    SupplierDue = r.Supplier.SupplierDue
                },
                PurchaseReceiptID = r.PurchaseReceiptID,
                PaidDate = r.Paid_Date,
                ReceiptSN = r.ReceiptSN,
                PaidAmount = r.PaidAmount,
                Payment_Situation = r.Payment_Situation,
                Description = r.Description,
                CollectBy = r.Registration.Name
            }).FirstOrDefault();

            return receipt;

        }

        public CustomDataResult<PurchasePaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var sell = Context.PurchasePaymentReceipt.Include(s => s.Supplier).Select(s => new PurchasePaymentReceiptList
            {
                PurchaseReceiptID = s.PurchaseReceiptID,
                Date = s.Paid_Date,
                Supplier = s.Supplier.SupplierCompanyName,
                Receipt = s.ReceiptSN,
                Amount = s.PaidAmount
            }).Where(e => e.Date <= eD && e.Date >= sD).OrderBy(e => e.Date);

            return sell.ToDataResultCustom(request);
        }

        public PurchasePaymentReceipt dueCollection(PurchasePaymentReceiptModel model)
        {
            var sells = Context.Purchase.Where(s => model.Invoices.Select(i => i.PurchaseID).Contains(s.PurchaseID)).ToList();

            foreach (var invoice in model.Invoices)
            {
                var sell = sells.FirstOrDefault(s => s.PurchaseID == invoice.PurchaseID);
                sell.PurchaseDiscountAmount = invoice.PurchaseDiscountAmount;
                var due = Math.Round(sell.PurchaseTotalPrice - (sell.PurchaseDiscountAmount.GetValueOrDefault() + sell.PurchasePaidAmount.GetValueOrDefault()), 2);
                if (due < invoice.PurchasePaidAmount) return null;
                sell.PurchasePaidAmount += invoice.PurchasePaidAmount;
            }

            var receipt = new PurchasePaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                SupplierID = model.SupplierID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.PaidAmount,
                Payment_Situation = model.Payment_Situation,
                Paid_Date = model.Paid_Date,
                Description = model.Description,
                PurchasePaymentRecord = model.Invoices.Select(i => new PurchasePaymentRecord
                {
                    PurchaseID = i.PurchaseID,
                    RegistrationID = model.RegistrationID,
                    PurchasePaidAmount = i.PurchasePaidAmount,
                    Payment_Situation = model.Payment_Situation,
                    Description = model.Description,
                    PurchasePaid_Date = model.Paid_Date
                }).ToList()
            };

            Context.PurchasePaymentReceipt.Add(receipt);

            Context.Purchase.UpdateRange(sells);

            return receipt;
        }
    }
}
