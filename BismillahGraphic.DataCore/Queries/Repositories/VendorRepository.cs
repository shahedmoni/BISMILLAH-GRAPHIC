using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(DataContext context) : base(context)
        {
        }

        public DataResult<VendorVM> ToListCustom(DataRequest request)
        {
            var vendor = Context.Vendor.Select(v => new VendorVM
            {
                VendorID = v.VendorID,
                VendorCompanyName = v.VendorCompanyName,
                VendorName = v.VendorName,
                VendorAddress = v.VendorAddress,
                VendorPhone = v.VendorPhone,
                Insert_Date = v.Insert_Date,
                VendorDue = v.VendorDue
            });

            return vendor.ToDataResult(request);
        }

        public async Task<ICollection<VendorVM>> ToListCustomAsync()
        {
            var vendor = await Context.Vendor.Select(v => new VendorVM
            {
                VendorID = v.VendorID,
                VendorCompanyName = v.VendorCompanyName,
                VendorName = v.VendorName,
                VendorAddress = v.VendorAddress,
                VendorPhone = v.VendorPhone,
                Insert_Date = v.Insert_Date,
                VendorDue = v.VendorDue
            }).ToListAsync();

            return vendor;
        }

        public async Task<ICollection<VendorVM>> SearchAsync(string key)
        {
            return await Context.Vendor.Where(v => v.VendorName.Contains(key) || v.VendorPhone.Contains(key) || v.VendorCompanyName.Contains(key)).Select(v =>
                  new VendorVM
                  {
                      VendorID = v.VendorID,
                      VendorCompanyName = v.VendorCompanyName,
                      VendorName = v.VendorName,
                      VendorAddress = v.VendorAddress,
                      VendorPhone = v.VendorPhone,
                      Insert_Date = v.Insert_Date,
                      VendorDue = v.VendorDue
                  }).Take(5).ToListAsync();
        }

        public Vendor AddCustom(VendorVM model)
        {
            var vendor = new Vendor
            {
                VendorCompanyName = model.VendorCompanyName,
                VendorName = model.VendorName,
                VendorAddress = model.VendorAddress,
                VendorPhone = model.VendorPhone
            };
            Add(vendor);
            return vendor;
        }

        public void UpdateCustom(VendorVM model)
        {
            var vendor = Find(model.VendorID);
            vendor.VendorCompanyName = model.VendorCompanyName;
            vendor.VendorName = model.VendorName;
            vendor.VendorAddress = model.VendorAddress;
            vendor.VendorPhone = model.VendorPhone;
            Update(vendor);
        }

        public VendorVM FindCustom(int? id)
        {
            var vendor = Find(id.GetValueOrDefault());

            if (vendor == null) return null;
            return new VendorVM
            {
                VendorID = vendor.VendorID,
                VendorCompanyName = vendor.VendorCompanyName,
                VendorName = vendor.VendorName,
                VendorAddress = vendor.VendorAddress,
                VendorPhone = vendor.VendorPhone,
                Insert_Date = vendor.Insert_Date,
                VendorDue = vendor.VendorDue

            };
        }

        public void UpdatePaidDue(int? id)
        {
            var verdor = Find(id.GetValueOrDefault());

            var obj = Context.Selling.Where(s => s.VendorID == verdor.VendorID).GroupBy(s => s.VendorID).Select(s =>
                   new
                   {
                       TotalAmount = s.Sum(c => c.SellingTotalPrice),
                       TotalDiscount = s.Sum(c => c.SellingDiscountAmount),
                       VendorPaid = s.Sum(c => c.SellingPaidAmount)
                   }).FirstOrDefault();

            verdor.TotalAmount = obj.TotalAmount;
            verdor.TotalDiscount = obj.TotalDiscount.GetValueOrDefault();
            verdor.VendorPaid = obj.VendorPaid.GetValueOrDefault();

            Update(verdor);
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Selling.Any(s => s.VendorID == id)) return false;
            Remove(Find(id));
            return true;
        }

        public ICollection<VendorPaidDue> TopDue()
        {
            return Context.Vendor.OrderByDescending(v => v.VendorDue).Take(5).Select(v => new VendorPaidDue
            {
                VendorID = v.VendorID,
                VendorCompanyName = v.VendorCompanyName,
                VendorDue = v.VendorDue,
                VendorPaid = v.VendorPaid,
                TotalAmount = v.TotalAmount,
                TotalDiscount = v.TotalDiscount
            }).ToList();
        }

        public ICollection<VendorPaidDue> PaidDues()
        {
            return Context.Vendor.OrderBy(v => v.VendorCompanyName).Select(v => new VendorPaidDue
            {
                VendorID = v.VendorID,
                VendorCompanyName = v.VendorCompanyName,
                TotalAmount = v.TotalAmount,
                VendorDue = v.VendorDue,
                VendorPaid = v.VendorPaid
            }).ToList();
        }

        public Vendor Details(int id)
        {
            return Context.Vendor.Include(v => v.Selling).FirstOrDefault(v => v.VendorID == id);
        }

        public ICollection<SellingRecord> SellDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);
            return Context.Vendor.Include(v => v.Selling).FirstOrDefault(v => v.VendorID == id)?.Selling.Select(s => new SellingRecord
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
            }).Where(e => e.SellingDate <= eD && e.SellingDate >= sD).OrderBy(e => e.SellingDate).ToList();
        }
    }
}
