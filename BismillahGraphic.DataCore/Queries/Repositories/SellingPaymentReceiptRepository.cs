using Microsoft.EntityFrameworkCore;
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
            var receipt = Context.SellingPaymentReceipt.Where(r => r.ReceiptID == id).Select(r => new PaymentReceiptPrint
            {
                Invoices = r.SellingPaymentRecord.Select(p => new PaidInvoiceList
                {
                    SellingID = p.SellingID,
                    SellingSN = p.Selling.SellingSN,
                    SellingAmount = p.Selling.SellingTotalPrice,
                    SellingPaidAmount = p.SellingPaidAmount,
                    SellingDate = p.Selling.SellingDate
                }).ToList(),

                VendorInfo = new VendorVM
                {
                    VendorID = r.VendorID,
                    VendorCompanyName = r.Vendor.VendorCompanyName,
                    VendorName = r.Vendor.VendorName,
                    VendorAddress = r.Vendor.VendorAddress,
                    VendorPhone = r.Vendor.VendorPhone,
                    Insert_Date = r.Vendor.Insert_Date,
                    VendorDue = r.Vendor.VendorDue
                },
                ReceiptID = r.ReceiptID,
                PaidDate = r.Paid_Date,
                ReceiptSN = r.ReceiptSN,
                PaidAmount = r.PaidAmount,
                Payment_Situation = r.Payment_Situation,
                Description = r.Description,
                CollectBy = r.Registration.Name
            }).FirstOrDefault();

            return receipt;

        }

        public CustomDataResult<PaymentReceiptList> DateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var sell = Context.SellingPaymentReceipt.Include(s => s.Vendor).Select(s => new PaymentReceiptList
            {
                ReceiptID = s.ReceiptID,
                Date = s.Paid_Date,
                Vendor = s.Vendor.VendorCompanyName,
                Receipt = s.ReceiptSN,
                Amount = s.PaidAmount
            }).Where(e => e.Date <= eD && e.Date >= sD).OrderBy(e => e.Date);

            return sell.ToDataResultCustom(request);
        }

        public SellingPaymentReceipt dueCollection(PaymentReceipt model)
        {
            var sells = Context.Selling.Where(s => model.Invoices.Select(i => i.SellingID).Contains(s.SellingID)).ToList();

            foreach (var invoice in model.Invoices)
            {
                var sell = sells.FirstOrDefault(s => s.SellingID == invoice.SellingID);
                sell.SellingDiscountAmount = invoice.SellingDiscountAmount;
                var due = Math.Round(sell.SellingTotalPrice - (sell.SellingDiscountAmount.GetValueOrDefault() + sell.SellingPaidAmount.GetValueOrDefault()), 2);
                if (due < invoice.SellingPaidAmount) return null;
                sell.SellingPaidAmount += invoice.SellingPaidAmount;
            }

            var receipt = new SellingPaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                VendorID = model.VendorID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.PaidAmount,
                Payment_Situation = model.Payment_Situation,
                Description = model.Description,
                Paid_Date = model.Paid_Date,
                SellingPaymentRecord = model.Invoices.Select(i => new SellingPaymentRecord
                {
                    SellingID = i.SellingID,
                    RegistrationID = model.RegistrationID,
                    SellingPaidAmount = i.SellingPaidAmount,
                    Payment_Situation = model.Payment_Situation,
                    Description = model.Description,
                    SellingPaid_Date = model.Paid_Date
                }).ToList()
            };

            Context.SellingPaymentReceipt.Add(receipt);

            Context.Selling.UpdateRange(sells);

            return receipt;
        }
    }
}
