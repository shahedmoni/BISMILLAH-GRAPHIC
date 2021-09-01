using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        DataResult<SupplierVM> ToListCustom(DataRequest request);
        Task<ICollection<SupplierVM>> ToListCustomAsync();
        Task<ICollection<SupplierVM>> SearchAsync(string key);
        Supplier AddCustom(SupplierVM model);
        void UpdateCustom(SupplierVM model);
        SupplierVM FindCustom(int? id);
        void UpdatePaidDue(int? id);
        bool RemoveCustom(int id);
        ICollection<SupplierPaidDue> TopDue();
        CustomDataResult<SupplierPaidDue> PaidDues(CustomDataRequest request);
        Supplier Details(int id);
        ICollection<PurchaseRecord> SellDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime);
        ICollection<PurchaseRecord> DueDateToDate(int id, DateTime? sDateTime, DateTime? eDateTime);
        void UpdateSmsNumber(int id, string number);
        bool IsExistSmsNumber(int id, string number);
    }
}
