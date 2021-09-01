using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DataContext context) : base(context)
        {
        }

        public DataResult<SupplierVM> ToListCustom(DataRequest request)
        {
            var Supplier = Context.Supplier.Select(v => new SupplierVM
            {
                SupplierID = v.SupplierID,
                SupplierCompanyName = v.SupplierCompanyName,
                SupplierName = v.SupplierName,
                SupplierAddress = v.SupplierAddress,
                SupplierPhone = v.SupplierPhone,
                Insert_Date = v.Insert_Date,
                SupplierDue = v.SupplierDue,
                SmsNumber = v.SmsNumber
            });

            return Supplier.ToDataResult(request);
        }

        public async Task<ICollection<SupplierVM>> ToListCustomAsync()
        {
            var Supplier = await Context.Supplier.Select(v => new SupplierVM
            {
                SupplierID = v.SupplierID,
                SupplierCompanyName = v.SupplierCompanyName,
                SupplierName = v.SupplierName,
                SupplierAddress = v.SupplierAddress,
                SupplierPhone = v.SupplierPhone,
                Insert_Date = v.Insert_Date,
                SupplierDue = v.SupplierDue,
                SmsNumber = v.SmsNumber
            }).ToListAsync();

            return Supplier;
        }

        public async Task<ICollection<SupplierVM>> SearchAsync(string key)
        {
            return await Context.Supplier.Where(v => v.SupplierName.Contains(key) || v.SupplierPhone.Contains(key) || v.SupplierCompanyName.Contains(key)).Select(v =>
                  new SupplierVM
                  {
                      SupplierID = v.SupplierID,
                      SupplierCompanyName = v.SupplierCompanyName,
                      SupplierName = v.SupplierName,
                      SupplierAddress = v.SupplierAddress,
                      SupplierPhone = v.SupplierPhone,
                      Insert_Date = v.Insert_Date,
                      SupplierDue = v.SupplierDue,
                      SmsNumber = v.SmsNumber
                  }).Take(5).ToListAsync();
        }

        public Supplier AddCustom(SupplierVM model)
        {
            var Supplier = new Supplier
            {
                SupplierCompanyName = model.SupplierCompanyName,
                SupplierName = model.SupplierName,
                SupplierAddress = model.SupplierAddress,
                SupplierPhone = model.SupplierPhone,
                SmsNumber = model.SmsNumber
            };
            Add(Supplier);
            return Supplier;
        }

        public void UpdateCustom(SupplierVM model)
        {
            var Supplier = Find(model.SupplierID);
            Supplier.SupplierCompanyName = model.SupplierCompanyName;
            Supplier.SupplierName = model.SupplierName;
            Supplier.SupplierAddress = model.SupplierAddress;
            Supplier.SupplierPhone = model.SupplierPhone;
            Supplier.SmsNumber = model.SmsNumber;
            Update(Supplier);
        }

        public SupplierVM FindCustom(int? id)
        {
            var Supplier = Find(id.GetValueOrDefault());

            if (Supplier == null) return null;
            return new SupplierVM
            {
                SupplierID = Supplier.SupplierID,
                SupplierCompanyName = Supplier.SupplierCompanyName,
                SupplierName = Supplier.SupplierName,
                SupplierAddress = Supplier.SupplierAddress,
                SupplierPhone = Supplier.SupplierPhone,
                Insert_Date = Supplier.Insert_Date,
                SupplierDue = Supplier.SupplierDue,
                SmsNumber = Supplier.SmsNumber

            };
        }

        public void UpdatePaidDue(int? id)
        {
            var verdor = Find(id.GetValueOrDefault());

            var obj = Context.Purchase.Where(s => s.SupplierID == verdor.SupplierID).GroupBy(s => s.SupplierID).Select(s =>
                   new
                   {
                       TotalAmount = s.Sum(c => c.PurchaseTotalPrice),
                       TotalDiscount = s.Sum(c => c.PurchaseDiscountAmount),
                       SupplierPaid = s.Sum(c => c.PurchasePaidAmount)
                   }).FirstOrDefault();

            verdor.TotalAmount = Math.Round(obj.TotalAmount, 2);
            verdor.TotalDiscount = Math.Round(obj.TotalDiscount.GetValueOrDefault(), 2);
            verdor.SupplierPaid = Math.Round(obj.SupplierPaid.GetValueOrDefault(), 2);

            Update(verdor);
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Purchase.Any(s => s.SupplierID == id)) return false;
            Remove(Find(id));
            return true;
        }

        public ICollection<SupplierPaidDue> TopDue()
        {
            return Context.Supplier.OrderByDescending(v => v.SupplierDue).Take(5).Select(v => new SupplierPaidDue
            {
                SupplierID = v.SupplierID,
                SupplierCompanyName = v.SupplierCompanyName,
                SupplierDue = v.SupplierDue,
                SupplierPaid = v.SupplierPaid,
                TotalAmount = v.TotalAmount,
                TotalDiscount = v.TotalDiscount
            }).ToList();
        }

        public CustomDataResult<SupplierPaidDue> PaidDues(CustomDataRequest request)
        {
            return Context.Supplier.Where(v => v.SupplierDue > 0).OrderBy(v => v.SupplierCompanyName).Select(v => new SupplierPaidDue
            {
                SupplierID = v.SupplierID,
                SupplierCompanyName = v.SupplierCompanyName,
                TotalAmount = v.TotalAmount,
                SupplierDue = v.SupplierDue,
                SupplierPaid = v.SupplierPaid
            }).ToDataResultCustom(request);
        }

        public Supplier Details(int id)
        {
            return Context.Supplier.Include(v => v.Purchase).FirstOrDefault(v => v.SupplierID == id);
        }

        public ICollection<PurchaseRecord> SellDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);
            return Context.Supplier.Include(v => v.Purchase).FirstOrDefault(v => v.SupplierID == id)?.Purchase.Select(s => new PurchaseRecord
            {
                PurchaseID = s.PurchaseID,
                SupplierID = s.SupplierID,
                SupplierCompanyName = s.Supplier.SupplierCompanyName,
                PurchaseSN = s.PurchaseSN,
                PurchaseAmount = s.PurchaseTotalPrice,
                PurchasePaidAmount = s.PurchasePaidAmount.GetValueOrDefault(),
                PurchaseDueAmount = s.PurchaseDueAmount.GetValueOrDefault(),
                PurchaseDiscountAmount = s.PurchaseDiscountAmount.GetValueOrDefault(),
                PurchaseDate = s.PurchaseDate
            }).Where(e => e.PurchaseDate <= eD && e.PurchaseDate >= sD).OrderBy(e => e.PurchaseDate).ToList();
        }

        public ICollection<PurchaseRecord> DueDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);
            return Context.Supplier.Include(v => v.Purchase).FirstOrDefault(v => v.SupplierID == id)?.Purchase.Select(s => new PurchaseRecord
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
            }).Where(e => e.PurchaseDueAmount > 0 && e.PurchaseDate <= eD && e.PurchaseDate >= sD).OrderBy(e => e.PurchaseDate).ToList();
        }

        public void UpdateSmsNumber(int id, string number)
        {
            var Supplier = Context.Supplier.Find(id);
            Supplier.SmsNumber = number;
            Context.Supplier.Update(Supplier);
        }

        public bool IsExistSmsNumber(int id, string number)
        {
            return Context.Supplier.Any(v => v.SupplierID != id && v.SmsNumber == number);
        }
    }
}
