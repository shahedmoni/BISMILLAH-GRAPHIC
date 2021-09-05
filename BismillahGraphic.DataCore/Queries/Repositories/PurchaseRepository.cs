using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DataContext context) : base(context)
        {
        }

        public Purchase Purchase(PurchaseVM model)
        {

            var purchase = new Purchase
            {
                RegistrationID = model.RegistrationID,
                SupplierID = model.SupplierID,
                PurchaseSN = model.PurchaseSN,
                PurchaseTotalPrice = model.PurchaseTotalPrice,
                PurchaseDiscountAmount = model.PurchaseDiscountAmount,
                PurchasePaidAmount = model.PurchasePaidAmount,
                PurchaseDate = model.PurchaseDate,
                Description = model.Description,
                PurchaseList = model.PurchaseCarts.Select(c => new PurchaseList
                {
                    ProductID = c.ProductID,
                    RegistrationID = model.RegistrationID,
                    PurchaseQuantity = c.PurchaseQuantity,
                    PurchaseUnitPrice = c.PurchaseUnitPrice,
                    MeasurementUnitId = c.MeasurementUnitId
                }).ToList(),
                PurchasePaymentRecord = model.PurchasePaidAmount == 0
                    ? null
                    : new List<PurchasePaymentRecord>
                    {
                        new PurchasePaymentRecord
                        { PurchasePaymentReceipt = new PurchasePaymentReceipt
                                {
                                    RegistrationID = model.RegistrationID,
                                    SupplierID = model.SupplierID,
                                    ReceiptSN = model.ReceiptSN,
                                    PaidAmount = model.PurchasePaidAmount,
                                    Payment_Situation = model.Payment_Situation,
                                    Description = model.Description,
                                    Paid_Date = model.PurchaseDate,
                                },
                            RegistrationID = model.RegistrationID,
                            PurchasePaidAmount = model.PurchasePaidAmount,
                            Payment_Situation = model.Payment_Situation,
                            Description = model.Description,
                            PurchasePaid_Date = model.PurchaseDate
                        }
                    }
            };
            Add(purchase);
            return purchase;

        }

        public PurchaseReceipt Sold(int id)
        {
            var receipt = new PurchaseReceipt
            {
                PurchaseInfo = Context.Purchase.Include(s => s.PurchaseList).ThenInclude(l => l.Product)
                    .Include(s => s.PurchaseList).ThenInclude(l => l.MeasurementUnit)
                    .Include(s => s.PurchasePaymentRecord).FirstOrDefault(s => s.PurchaseID == id)
            };

            receipt.SupplierInfo = Context.Supplier.Where(v => v.SupplierID == receipt.PurchaseInfo.SupplierID).Select(Supplier =>
                new SupplierVM
                {
                    SupplierID = Supplier.SupplierID,
                    SupplierCompanyName = Supplier.SupplierCompanyName,
                    SupplierName = Supplier.SupplierName,
                    SupplierAddress = Supplier.SupplierAddress,
                    SupplierPhone = Supplier.SupplierPhone,
                    SmsNumber = Supplier.SmsNumber,
                    Insert_Date = Supplier.Insert_Date,
                    SupplierDue = Supplier.SupplierDue

                }).FirstOrDefault();
            if (receipt.PurchaseInfo != null)
                receipt.SoldBy = Context.Registration.Find(receipt.PurchaseInfo.RegistrationID).Name;
            return receipt;
        }

        public PurchaseUpdateViewModel FindUpdateBill(int id)
        {
            var bill = Context.Purchase
                .Include(s => s.Supplier)
                .Include(s => s.PurchaseList)
                .ThenInclude(l => l.Product)
                .Select(s => new PurchaseUpdateViewModel
                {
                    SupplierInfo = new SupplierVM
                    {
                        SupplierID = s.Supplier.SupplierID,
                        SupplierCompanyName = s.Supplier.SupplierCompanyName,
                        SupplierName = s.Supplier.SupplierName,
                        SupplierAddress = s.Supplier.SupplierAddress,
                        SupplierPhone = s.Supplier.SupplierPhone,
                        Insert_Date = s.Supplier.Insert_Date,
                        SupplierDue = s.Supplier.SupplierDue
                    },
                    PurchaseID = s.PurchaseID,
                    PurchaseSN = s.PurchaseSN,
                    PurchaseTotalPrice = s.PurchaseTotalPrice,
                    PurchaseDiscountAmount = s.PurchaseDiscountAmount,
                    PurchasePaidAmount = s.PurchasePaidAmount.GetValueOrDefault(),
                    PurchaseDate = s.PurchaseDate,
                    Description = s.Description,
                    PurchaseCarts = s.PurchaseList.Select(l => new PurchaseUpdateCart
                    {
                        ProductID = l.ProductID,
                        ProductName = l.Product.ProductName,
                        PurchaseQuantity = l.PurchaseQuantity,
                        PurchaseUnitPrice = l.PurchaseUnitPrice,
                        MeasurementUnitId = l.MeasurementUnitId

                    }).ToList()
                }).FirstOrDefault(s => s.PurchaseID == id);

            return bill;
        }

        public void BillUpdated(PurchaseBillChangeViewModel model)
        {
            var Purchase = Context.Purchase
                .Include(s => s.PurchaseList)
                .FirstOrDefault(s => s.PurchaseID == model.PurchaseID);

            if (Purchase == null) return;

            var due = (model.PurchaseTotalPrice - model.PurchaseDiscountAmount.GetValueOrDefault()) - Purchase.PurchasePaidAmount;

            if (due < 0) return;

            //product stock update
            #region StockUpdate
            var previousProductIds = Purchase.PurchaseList.Select(s => s.ProductID).ToArray();
            var newProducts = model.PurchaseCarts.Where(s => !previousProductIds.Contains(s.ProductID)).ToList();

            foreach (var list in newProducts)
            {
                var product = Context.Product.Find(list.ProductID);
                product.Stock -= list.PurchaseQuantity;
                Context.Product.Update(product);
            }

            foreach (var list in Purchase.PurchaseList)
            {

                var newQuantity = model.PurchaseCarts.FirstOrDefault(s => s.ProductID == list.ProductID)?.PurchaseQuantity ?? 0;

                var product = Context.Product.Find(list.ProductID);
                product.Stock += list.PurchaseQuantity - newQuantity;
                Context.Product.Update(product);
            }
            #endregion



            Purchase.PurchaseDiscountAmount = model.PurchaseDiscountAmount;
            Purchase.PurchaseTotalPrice = model.PurchaseTotalPrice;
            Purchase.Description = model.Description;
            Purchase.PurchaseList = model.PurchaseCarts.Select(c => new PurchaseList
            {
                ProductID = c.ProductID,
                RegistrationID = Purchase.RegistrationID,
                PurchaseQuantity = c.PurchaseQuantity,
                PurchaseUnitPrice = c.PurchaseUnitPrice,
                MeasurementUnitId = c.MeasurementUnitId
            }).ToList();

            Context.Purchase.Update(Purchase);
        }

        public bool DeleteBill(int id)
        {
            var Purchase = Context.Purchase
                .Include(s => s.PurchaseList)
                .Include(s => s.PurchasePaymentRecord)
                .FirstOrDefault(s => s.PurchaseID == id);
            if (Purchase.PurchasePaymentRecord.Count > 0) return false;

            foreach (var list in Purchase.PurchaseList)
            {
                var product = Context.Product.Find(list.ProductID);
                product.Stock += list.PurchaseQuantity;
                Context.Product.Update(product);
            }
            Context.Purchase.Remove(Purchase);
            return true;

        }

        public int GetPurchaseSN()
        {
            var sn = 0;
            if (Context.Purchase.Any())
            {
                sn = Context.Purchase.Max(s => s == null ? 0 : s.PurchaseSN);
            }

            return sn + 1;
        }

        public ICollection<int> Years()
        {
            var years = Context.Purchase.GroupBy(e => new { Year = e.PurchaseDate.Year }).Select(g => g.Key.Year)
                .OrderBy(o => o).ToList();

            var currentYear = DateTime.Now.Year;
            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public double SaleYearly(int year)
        {
            return Math.Round(ToList().Where(s => s.PurchaseDate.Year == year).Sum(s => s.PurchaseTotalPrice - s.PurchaseDiscountAmount.GetValueOrDefault()), 2);
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Purchase.Where(s => s.PurchaseDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.PurchaseDate.Month
                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = Math.Round(g.Sum(s => s.PurchaseTotalPrice - s.PurchaseDiscountAmount.GetValueOrDefault()), 2)
                })
                .ToList();

            return months ?? new List<MonthlyAmount>();
        }

        public double TotalDue()
        {
            return Math.Round(ToList().Sum(s => s.PurchaseDueAmount.GetValueOrDefault()), 2);
        }

        public DataResult<PurchaseRecord> Records(DataRequest request)
        {
            var r = Context.Purchase.Include(s => s.Supplier).Select(s => new PurchaseRecord
            {
                PurchaseID = s.PurchaseID,
                SupplierID = s.SupplierID,
                SupplierCompanyName = s.Supplier.SupplierCompanyName,
                PurchaseSN = s.PurchaseSN,
                PurchaseAmount = s.PurchaseTotalPrice - s.PurchaseDiscountAmount.GetValueOrDefault(),
                PurchasePaidAmount = s.PurchasePaidAmount.GetValueOrDefault(),
                PurchaseDueAmount = s.PurchaseDueAmount.GetValueOrDefault(),
                PurchaseDiscountAmount = s.PurchaseDiscountAmount.GetValueOrDefault(),
                PurchaseDate = s.PurchaseDate
            });

            return r.ToDataResult(request);
        }

        public bool dueCollection(PurchaseInvoicePaySingle model)
        {

            var sell = Find(model.PurchaseID);
            if (sell.PurchaseDueAmount < model.PurchasePaidAmount) return false;


            var receipt = new PurchasePaymentReceipt
            {
                RegistrationID = model.RegistrationID,
                SupplierID = model.SupplierID,
                ReceiptSN = model.ReceiptSN,
                PaidAmount = model.PurchasePaidAmount,
                Payment_Situation = model.Payment_Situation,
                Description = model.Description,
                Paid_Date = model.PurchasePaid_Date,
                PurchasePaymentRecord = new List<PurchasePaymentRecord>
                {
                    new PurchasePaymentRecord
                    {
                        PurchaseID = model.PurchaseID,
                        RegistrationID = model.RegistrationID,
                        PurchasePaidAmount = model.PurchasePaidAmount,
                        Payment_Situation = model.Payment_Situation,
                        Description = model.Description,
                        PurchasePaid_Date = model.PurchasePaid_Date
                    }

                }
            };

            Context.PurchasePaymentReceipt.Add(receipt);

            sell.PurchasePaidAmount += model.PurchasePaidAmount;
            Update(sell);

            return true;
        }

        public CustomDataResult<PurchaseRecord> SellDateToDate(CustomDataRequest request, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var sell = Context.Purchase.Include(s => s.Supplier).Select(s => new PurchaseRecord
            {
                PurchaseID = s.PurchaseID,
                SupplierID = s.SupplierID,
                SupplierCompanyName = s.Supplier.SupplierCompanyName,
                PurchaseSN = s.PurchaseSN,
                PurchaseAmount = s.PurchaseTotalPrice - s.PurchaseDiscountAmount.GetValueOrDefault(),
                PurchasePaidAmount = s.PurchasePaidAmount.GetValueOrDefault(),
                PurchaseDueAmount = s.PurchaseDueAmount.GetValueOrDefault(),
                PurchaseDiscountAmount = s.PurchaseDiscountAmount.GetValueOrDefault(),
                PurchaseDate = s.PurchaseDate
            }).Where(e => e.PurchaseDate <= eD && e.PurchaseDate >= sD).OrderBy(e => e.PurchaseDate);

            return sell.ToDataResultCustom(request);
        }

        public CustomDataResult<PurchaseIncomeVM> IncomeDateToDate(CustomDataRequest request, DateTime? sDateTime,
            DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.Year, 12, 31);

            var income = Context.PurchasePaymentRecord.Include(r => r.Purchase).ThenInclude(s => s.Supplier)
                .Include(r => r.Registration)
                .Where(r => r.PurchasePaid_Date <= eD && r.PurchasePaid_Date >= sD).Select(r => new PurchaseIncomeVM
                {
                    PurchaseID = r.PurchaseID,
                    SupplierID = r.Purchase.SupplierID,
                    SupplierCompanyName = r.Purchase.Supplier.SupplierCompanyName,
                    PurchaseSN = r.Purchase.PurchaseSN,
                    PurchasePaidAmount = r.PurchasePaidAmount,
                    Payment_Situation = r.Payment_Situation,
                    Description = r.Description,
                    PurchasePaid_Date = r.PurchasePaid_Date,
                    ReceivedBy = r.Registration.Name
                });
            return income.ToDataResultCustom(request);
        }
    }
}
