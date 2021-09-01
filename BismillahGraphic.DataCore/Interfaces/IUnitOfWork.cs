using System;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public interface IUnitOfWork : IDisposable
    {
        IMeasurementUnitRepository MeasurementUnits { get; }
        IPageLinkRepository PageLinks { get; }
        IPageLinkCategoryRepository PageLinkCategorys { get; }
        IPageLinkAssignRepository PageLinkAssigns { get; }
        IProductRepository Products { get; }
        IProductCategoryRepository ProductCategories { get; }
        IPurchaseRepository Purchases { get; }
        IRegistrationRepository Registrations { get; }
        IExpanseCategoryRepository ExpanseCategories { get; }
        IVendorRepository Vendors { get; }
        IExpanseRepository Expanses { get; }
        ISellingRepository Selling { get; }
        ISellingListRepository SellingLists { get; }
        ISellingPaymentReceiptRepository SellingPaymentReceipts { get; }
        ISellingPaymentRecordRepository SellingPaymentRecords { get; }
        ISupplierRepository Suppliers { get; }
        IInstitutionRepository Institutions { get; }
        ISmsRepository SMS { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
