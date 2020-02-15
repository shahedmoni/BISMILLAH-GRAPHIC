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
                    Length = c.Length,
                    Width = c.Width
                }).ToList(),
                SellingPaymentRecord = model.SellingPaidAmount == 0
                    ? null
                    : new List<SellingPaymentRecord>
                    {
                        new SellingPaymentRecord
                        {
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
                    Insert_Date = vendor.Insert_Date,
                    VendorDue = vendor.VendorDue

                }).FirstOrDefault();
            if (receipt.SellingInfo != null)
                receipt.SoildBy = Context.Registration.Find(receipt.SellingInfo.RegistrationID).Name;
            return receipt;
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
            return ToList().Where(s => s.SellingDate.Year == year)
                .Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault());
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
                    Amount = g.Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount.GetValueOrDefault())
                })
                .ToList();

            return months ?? new List<MonthlyAmount>();
        }

        public double TotalDue()
        {
            return ToList().Sum(s => s.SellingDueAmount.GetValueOrDefault());
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

        public bool dueCollection(SellingDuePay model)
        {
            var sell = Find(model.SellingID);
            if (sell.SellingDueAmount < model.SellingPaidAmount) return false;

            var pay = new SellingPaymentRecord
            {
                SellingID = model.SellingID,
                RegistrationID = model.RegistrationID,
                SellingPaidAmount = model.SellingPaidAmount,
                Payment_Situation = model.Payment_Situation,
                SellingPaid_Date = model.SellingPaid_Date
            };
            Context.SellingPaymentRecord.Add(pay);
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
