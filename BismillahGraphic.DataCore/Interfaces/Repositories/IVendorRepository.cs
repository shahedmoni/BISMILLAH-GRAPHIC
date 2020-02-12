using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        ICollection<VendorVM> ToListCustom();
        Task<ICollection<VendorVM>> ToListCustomAsync();
        Task<ICollection<VendorVM>> SearchAsync(string key);
        Vendor AddCustom(VendorVM model);
        void UpdateCustom(VendorVM model);
        VendorVM FindCustom(int? id);

        void UpdatePaidDue(int? id);

        bool RemoveCustom(int id);

        ICollection<VendorPaidDue> TopDue();
        ICollection<VendorPaidDue> PaidDues();
        Vendor Details(int id);
        ICollection<SellingRecord> SellDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime);
    }
}
