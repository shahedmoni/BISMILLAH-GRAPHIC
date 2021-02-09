using System;
using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class VendorDetails
    {
        private readonly DateTime _sDate;
        private readonly DateTime _eDate;
        private readonly IUnitOfWork _db;
        private readonly int _vendorId;
        public VendorVM VendorInfo { get; private set; }
        public ICollection<SellingRecord> Sales { get; private set; }
        public ICollection<SellingRecord> Dues { get; private set; }

        public double DateToDateSale { get; }
        public double DateToDatePaid { get; }
        public double DateToDateDue { get; }
        public double DateToDateDiscount { get; }
        public VendorDetails(IUnitOfWork unitOfWork, int verdorId, DateTime? sDate, DateTime? eDate)
        {
            _vendorId = verdorId;
            _db = unitOfWork;
            _sDate = sDate ?? new DateTime(1000, 1, 1);
            _eDate = eDate ?? new DateTime(3000, 1, 1);
            GetVendorInfo();
            GetSales();
            GetDues();
            DateToDateDue = Math.Round(Sales.Sum(s => s.SellingDueAmount), 2);
            DateToDatePaid = Math.Round(Sales.Sum(s => s.SellingPaidAmount), 2);
            DateToDateSale = Math.Round(Sales.Sum(s => s.SellingAmount), 2);
            DateToDateDiscount = Math.Round(Sales.Sum(s => s.SellingDiscountAmount), 2);



        }


        private void GetVendorInfo()
        {
            this.VendorInfo = _db.Vendors.FindCustom(_vendorId);
        }

        private void GetSales()
        {
            this.Sales = _db.Vendors.SellDateToDate(this._vendorId, this._sDate, this._eDate);
        }

        private void GetDues()
        {
            this.Dues = Sales.Where(s => s.SellingDueAmount > 0).ToList();
        }
    }
}
