using System;
using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public interface IUnitOfWork : IDisposable
    {
        IPageLinkRepository PageLinks { get; }
        IPageLinkCategoryRepository PageLinkCategorys { get; }
        IPageLinkAssignRepository PageLinkAssigns { get; }
        IProductRepository Products { get; }
        IProductCategoryRepository ProductCategories { get; }
        IRegistrationRepository Registrations { get; }
        IExpanseCategoryRepository ExpanseCategories { get; }
        IVendorRepository Vendors { get; }
        IExpanseRepository Expanses { get; }
        ISellingRepository Selling { get; }
        ISellingListRepository SellingLists { get; }
        ISellingPaymentReceiptRepository SellingPaymentReceipts { get; }
        ISellingPaymentRecordRepository SellingPaymentRecords { get; }
        IInstitutionRepository Institutions { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
