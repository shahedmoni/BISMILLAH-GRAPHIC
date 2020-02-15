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
