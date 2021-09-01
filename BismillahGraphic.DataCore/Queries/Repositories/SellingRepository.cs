using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class SellingRepository : Repository<Selling>, ISellingRepository
    {
        public SellingRepository(DataContext context) : base(context)
        {
        }

        public Selling Selling(SellingVM model)
        {

            var sell = new Selling
            {
                RegistrationID = model.RegistrationID,
                VendorID = model.VendorID,
                SellingSN = model.SellingSN,
                SellingTotalPrice = model.SellingTotalPrice,
                SellingDiscountAmount = model.SellingDiscountAmount,
                SellingPaidAmount = model.SellingPaidAmount,
                SellingDate = model.SellingDate,
                SellingList = model.SellingCarts.Select(c => new SellingList
                {
                    ProductID = c.ProductID,
                    RegistrationID = model.RegistrationID,
                    SellingQuantity = c.SellingQuantity,
                    SellingUnitPrice = c.SellingUnitPrice,
                    MeasurementUnitId = c.MeasurementUnitId,
                    Length = c.Length,
                    Width = c.Width
                }).ToList(),
                SellingPaymentRecord = model.SellingPaidAmount == 0
                    ? null
                    : new List<SellingPaymentRecord>
                    {
                        new SellingPaymentRecord
                        { SellingPaymentReceipt = new SellingPaymentReceipt
                                {
                                    RegistrationID = model.RegistrationID,
                                    VendorID = model.VendorID,
                                    ReceiptSN = model.ReceiptSN,
                                    PaidAmount = model.SellingPaidAmount,
                                    Payment_Situation = model.Payment_Situation,
                                    Paid_Date = model.SellingDate,
                                },
                            RegistrationID = model.RegistrationID,
                            SellingPaidAmount = model.SellingPaidAmount,
                            Payment_Situation = model.Payment_Situation,
                            SellingPaid_Date = model.SellingDate
                        }
                    }
            };
            Add(sell);
            return sell;

        }

        public SellingReceipt Sold(int id)
        {
            var receipt = new SellingReceipt
            {
                SellingInfo = Context.Selling.Include(s => s.SellingList).ThenInclude(l => l.Product)
                    .Include(s => s.SellingList).ThenInclude(l => l.MeasurementUnit)
                    .Include(s => s.SellingPaymentRecord).FirstOrDefault(s => s.SellingID == id)
            };

            receipt.VendorInfo = Context.Vendor.Where(v => v.VendorID == receipt.SellingInfo.VendorID).Select(vendor =>
                new VendorVM
                {
                    VendorID = vendor.VendorID,
                    VendorCompanyName = vendor.VendorCompanyName,
                    VendorName = vendor.VendorName,
                    VendorAddress = vendor.VendorAddress,
                    VendorPhone = vendor.VendorPhone,
                    SmsNumber = vendor.SmsNumber,
                    Insert_Date = vendor.Insert_Date,
                    VendorDue = vendor.VendorDue

                }).FirstOrDefault();
            if (receipt.SellingInfo != null)
                receipt.SoildBy = Context.Registration.Find(receipt.SellingInfo.RegistrationID).Name;
            return receipt;
        }

        public SellingUpdateViewModel FindUpdateBill(int id)
        {
            var bill = Context.Selling
                .Include(s => s.Vendor)
                .Include(s => s.SellingList)
                .ThenInclude(l => l.Product)
                .Select(s => new SellingUpdateViewModel
                {
                    VendorInfo = new VendorVM
                    {
                        VendorID = s.Vendor.VendorID,
                        VendorCompanyName = s.Vendor.VendorCompanyName,
                        VendorName = s.Vendor.VendorName,
                        VendorAddress = s.Vendor.VendorAddress,
                        VendorPhone = s.Vendor.VendorPhone,
                        Insert_Date = s.Vendor.Insert_Date,
                        VendorDue = s.Vendor.VendorDue
                    },
                    SellingID = s.SellingID,
                    SellingSN = s.SellingSN,
                    SellingTotalPrice = s.SellingTotalPrice,
                    SellingDiscountAmount = s.SellingDiscountAmount,
                    SellingPaidAmount = s.SellingPaidAmount.GetValueOrDefault(),
                    SellingDate = s.SellingDate,
                    SellingCarts = s.SellingList.Select(l => new SellingUpdateCart
                    {
                        ProductID = l.ProductID,
                        MeasurementUnitId = l.MeasurementUnitId,
                        ProductName = l.Product.ProductName,
                        SellingQuantity = l.SellingQuantity,
                        SellingUnitPrice = l.SellingUnitPrice,
                        Length = l.Length,
                        Width = l.Width
                    }).ToList()
                }).FirstOrDefault(s => s.SellingID == id);

            return bill;
        }

        public void BillUpdated(SellingBillChangeViewModel model)
        {
            var selling = Context.Selling
                .Include(s => s.SellingList)
                .FirstOrDefault(s => s.SellingID == model.SellingID);

            if (selling == null) return;

            var due = (model.SellingTotalPrice - model.SellingDiscountAmount.GetValueOrDefault()) - selling.SellingPaidAmount;

            if (due < 0) return;

            //product stock update
            #region StockUpdate
            var previousProductIds = selling.SellingList.Select(s => s.ProductID).ToArray();
            var newProducts = model.SellingCarts.Where(s => !previousProductIds.Contains(s.ProductID)).ToList();

            foreach (var list in newProducts)
            {
                var product = Context.Product.Find(list.ProductID);
                product.Stock -= list.SellingQuantity;
                Context.Product.Update(product);
            }

            foreach (var list in selling.SellingList)
            {

                var newQuantity = model.SellingCarts.FirstOrDefault(s => s.ProductID == list.ProductID)?.SellingQuantity ?? 0;

                var product = Context.Product.Find(list.ProductID);
                product.Stock += list.SellingQuantity - newQuantity;
                Context.Product.Update(product);
            }
            #endregion



            selling.SellingDiscountAmount = model.SellingDiscountAmount;
            selling.SellingTotalPrice = model.SellingTotalPrice;
            selling.SellingList = model.SellingCarts.Select(c => new SellingList
            {
                ProductID = c.ProductID,
                RegistrationID = selling.RegistrationID,
                SellingQuantity = c.SellingQuantity,
                SellingUnitPrice = c.SellingUnitPrice,
                Length = c.Length,
                Width = c.Width
            }).ToList();

            Context.Selling.Update(selling);
        }

        public bool DeleteBill(int id)
        {
            var selling = Context.Selling
                .Include(s => s.SellingList)
                .Include(s => s.SellingPaymentRecord)
                .FirstOrDefault(s => s.SellingID == id);
            if (selling.SellingPaymentRecord.Count > 0) return false;

            foreach (var list in selling.SellingList)
            {
                var product = Context.Product.Find(list.ProductID);
                product.Stock += list.SellingQuantity;
                Context.Product.Update(product);
            }
            Context.Selling.Remove(selling);
            return true;

        }

        public int GetSellingSN()
        {
            var sn = 0;
            if (Context.Selling.Any())
            {
                sn = Context.Selling.Max(s => s == null ? 0 : s.SellingSN);
            }

            return sn + 1;
        }

        public ICollection<int> Years()
        {
            var years = Context.Selling.GroupBy(e => new { Year = e.SellingDate.Year }).Select(g => g.Key.Year)
                .OrderBy(o => o).ToList();

            var currentYear = DateTime.Now.Year;
            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public double SaleYearly(int year)
        {
            return Math.Round(ToList().Where(s => s.SellingDate.Year == year).Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault()), 2);
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Selling.Where(s => s.SellingDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.SellingDate.Month
                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = Math.Round(g.Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault()), 2)
                })
                .ToList();

            return months ?? new List<MonthlyAmount>();
        }

        public double TotalDue()
        {
            return Math.Round(ToList().Sum(s => s.SellingDueAmount.GetValueOrDefault()), 2);
        }

        public DataResult<SellingRecord> Records(DataRequest request)
        {
            var r = Context.Selling.Include(s => s.Vendor).Select(s => new SellingRecord
            {
                SellingID = s.SellingID,
                VendorID = s.VendorID,
                VendorCompanyName = s.Vendor.VendorCompanyName,
                SellingSN = s.SellingSN,
                SellingAmount = s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault(),
                SellingPaidAmount = s.SellingPaidAmount.GetValueOrDefault(),
                SellingDueAmount = s.SellingDueAmount.GetValueOrDefault(),
                SellingDiscountAmount = s.SellingDiscountAmount.GetValueOrDefault(),
                SellingDate = s.SellingDate
            });

            return r.ToDataResult(request);
        }

        public bool dueCollection(InvoicePaySingle model)
        {

            var sell = Find(model.SellingID);
            if (sell.SellingDueAmount < model.SellingPaidAmount) return false;


            var receipt = new SellingPaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                VendorID = model.VendorID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.SellingPaidAmount,
                Payment_Situation = model.Payment_Situation,
                Paid_Date = model.SellingPaid_Date,
                SellingPaymentRecord = new List<SellingPaymentRecord>
                {
                    new SellingPaymentRecord
                    {
                        SellingID = model.SellingID,
                        RegistrationID = model.RegistrationID,
                        SellingPaidAmount = model.SellingPaidAmount,
                        Payment_Situation = model.Payment_Situation,
                        SellingPaid_Date = model.SellingPaid_Date
                    }

                }
            };

            Context.SellingPaymentReceipt.Add(receipt);

            sell.SellingPaidAmount += model.SellingPaidAmount;
            Update(sell);

            return true;
        }

        public CustomDataResult<SellingRecord> SellDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var sell = Context.Selling.Include(s => s.Vendor).Select(s => new SellingRecord
            {
                SellingID = s.SellingID,
                VendorID = s.VendorID,
                VendorCompanyName = s.Vendor.VendorCompanyName,
                SellingSN = s.SellingSN,
                SellingAmount = s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault(),
                SellingPaidAmount = s.SellingPaidAmount.GetValueOrDefault(),
                SellingDueAmount = s.SellingDueAmount.GetValueOrDefault(),
                SellingDiscountAmount = s.SellingDiscountAmount.GetValueOrDefault(),
                SellingDate = s.SellingDate
            }).Where(e => e.SellingDate <= eD && e.SellingDate >= sD).OrderBy(e => e.SellingDate);

            return sell.ToDataResultCustom(request);
        }

        public CustomDataResult<IncomeVM> IncomeDateToDate(CustomDataRequest request, DateTime? sDateTime,
            DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var income = Context.SellingPaymentRecord.Include(r => r.Selling).ThenInclude(s => s.Vendor)
                .Include(r => r.Registration)
                .Where(r => r.SellingPaid_Date <= eD && r.SellingPaid_Date >= sD).Select(r => new IncomeVM
                {
                    SellingID = r.SellingID,
                    VendorID = r.Selling.VendorID,
                    VendorCompanyName = r.Selling.Vendor.VendorCompanyName,
                    SellingSN = r.Selling.SellingSN,
                    SellingPaidAmount = r.SellingPaidAmount,
                    Payment_Situation = r.Payment_Situation,
                    SellingPaid_Date = r.SellingPaid_Date,
                    ReceivedBy = r.Registration.Name
                });
            return income.ToDataResultCustom(request);
        }
    }
}
