using BismillahGraphic.DataCore;
using System;
using System.Web.Mvc;


namespace BismillahGraphic.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _db;

        public ReportController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        [Authorize(Roles = "Admin, Report_DailyCash")]
        public ActionResult DailyCashReport()
        {
            var date = DateTime.Now;
            var model = new DailyCashClass(_db, date);
            return View(model);
        }


        [Authorize(Roles = "Admin, Report_Income")]
        // GET: Income
        public ActionResult Income()
        {
            return View();
        }

        public JsonResult GetIncome(CustomDataRequest request, string sFromDate, string sToDate)
        {
            DateTime.TryParse(sFromDate, out var fDate);
            DateTime.TryParse(sToDate, out var tDate);
            var model = _db.Selling.IncomeDateToDate(request, fDate, tDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Report_Expense")]
        // GET: Expense
        public ActionResult Expense()
        {
            return View();
        }

        public JsonResult GetExpense(DateTime? fromDate, DateTime? toDate)
        {
            var model = new ExpenseReport(_db, fromDate, toDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Admin, Report_Selling")]
        // GET: Selling
        public ActionResult Selling()
        {
            return View();
        }

        public JsonResult GetSelling(CustomDataRequest request, string sFromDate, string sToDate)
        {
            DateTime.TryParse(sFromDate, out var fDate);
            DateTime.TryParse(sToDate, out var tDate);

            var model = _db.Selling.SellDateToDate(request, fDate, tDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        // GET: Vendor
        [Authorize(Roles = "Admin, Report_Vendor")]
        public ActionResult Vendor()
        {
            return View();
        }

        public JsonResult GetVendor(CustomDataRequest request)
        {
            var model = _db.Vendors.PaidDues(request);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        //Net Summery
        public ActionResult NetSummery(int? id)
        {
            var year = id ?? DateTime.Now.Year;
            var model = new NetReport(_db, year);
            return View(model);
        }


        //Payment Summery
        public ActionResult PaymentSummery()
        {
            return View();
        }

        //call from ajax
        public JsonResult GetPaymentSummery(CustomDataRequest request, string sFromDate, string sToDate)
        {
            DateTime.TryParse(sFromDate, out var fDate);
            DateTime.TryParse(sToDate, out var tDate);
            var model = _db.SellingPaymentReceipts.DateToDate(request, fDate, tDate);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Product Summery
        public ActionResult ProductSummery()
        {
            return View();
        }

        //ajax call Product Summery
        public JsonResult GetProductSummery(DateTime? formDate, DateTime toDate)
        {
            var model = _db.Products.SoldReport(formDate, toDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}