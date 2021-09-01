using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class SupplierDetails
    {
        private readonly DateTime _sDate;
        private readonly DateTime _eDate;
        private readonly IUnitOfWork _db;
        private readonly int _SupplierId;
        public SupplierVM SupplierInfo { get; private set; }
        public ICollection<PurchaseRecord> Purchases { get; private set; }
        public ICollection<PurchaseRecord> Dues { get; private set; }

        public double DateToDateSale { get; }
        public double DateToDatePaid { get; }
        public double DateToDateDue { get; }
        public double DateToDateDiscount { get; }
        public SupplierDetails(IUnitOfWork unitOfWork, int supplierId, DateTime? sDate, DateTime? eDate)
        {
            _SupplierId = supplierId;
            _db = unitOfWork;
            _sDate = sDate ?? new DateTime(1000, 1, 1);
            _eDate = eDate ?? new DateTime(3000, 1, 1);
            GetSupplierInfo();
            GetPurchases();
            GetDues();
            DateToDateDue = Math.Round(Purchases.Sum(s => s.PurchaseDueAmount), 2);
            DateToDatePaid = Math.Round(Purchases.Sum(s => s.PurchasePaidAmount), 2);
            DateToDateSale = Math.Round(Purchases.Sum(s => s.PurchaseAmount), 2);
            DateToDateDiscount = Math.Round(Purchases.Sum(s => s.PurchaseDiscountAmount), 2);



        }


        private void GetSupplierInfo()
        {
            this.SupplierInfo = _db.Suppliers.FindCustom(_SupplierId);
        }

        private void GetPurchases()
        {
            this.Purchases = _db.Suppliers.SellDateToDate(this._SupplierId, this._sDate, this._eDate);
        }

        private void GetDues()
        {
            this.Dues = Purchases.Where(s => s.PurchaseDueAmount > 0).ToList();
        }
    }
}
