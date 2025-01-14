﻿using System.Threading.Tasks;

namespace BismillahGraphic.DataCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;

            MeasurementUnits = new MeasurementUnitRepository(_context);
            PageLinks = new PageLinkRepository(_context);
            PageLinkCategorys = new PageLinkCategoryRepository(_context);
            PageLinkAssigns = new PageLinkAssignRepository(_context);
            Products = new ProductRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
            Purchases = new PurchaseRepository(_context);
            PurchasePaymentReceipts = new PurchasePaymentReceiptRepository(_context);
            Registrations = new RegistrationRepository(_context);
            ExpanseCategories = new ExpanseCategoryRepository(_context);
            Vendors = new VendorRepository(_context);
            Expanses = new ExpanseRepository(_context);
            Selling = new SellingRepository(_context);
            SellingPaymentReceipts = new SellingPaymentReceiptRepository(_context);
            Suppliers = new SupplierRepository(_context);
            Institutions = new InstitutionRepository(_context);
            SMS = new SmsRepository(_context);
        }


        public IMeasurementUnitRepository MeasurementUnits { get; }
        public IPageLinkRepository PageLinks { get; private set; }
        public IPageLinkCategoryRepository PageLinkCategorys { get; private set; }
        public IPageLinkAssignRepository PageLinkAssigns { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductCategoryRepository ProductCategories { get; private set; }
        public IPurchaseRepository Purchases { get; }
        public IPurchasePaymentReceiptRepository PurchasePaymentReceipts { get; }
        public IRegistrationRepository Registrations { get; private set; }
        public IExpanseCategoryRepository ExpanseCategories { get; private set; }
        public IVendorRepository Vendors { get; }
        public IExpanseRepository Expanses { get; }
        public ISellingRepository Selling { get; }
        public ISellingPaymentReceiptRepository SellingPaymentReceipts { get; }
        public ISupplierRepository Suppliers { get; }
        public IInstitutionRepository Institutions { get; }
        public ISmsRepository SMS { get; }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
