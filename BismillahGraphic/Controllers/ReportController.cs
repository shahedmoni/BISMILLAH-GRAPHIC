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

        [Authorize(Roles = "Admin, Report_Income")]
        // GET: Income
        public ActionResult Income()
        {
            return View();
        }

        public JsonResult GetIncome(DataRequest request, string sFromDate, string sToDate)
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

        public JsonResult GetSelling(DataRequest request, string sFromDate, string sToDate)
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

        public JsonResult GetVendor(DataRequest request)
        {
            var model = _db.Vendors.PaidDues(request);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        //Income Summery
        public ActionResult SalesSummery(int? id)
        {
            var year = id ?? DateTime.Now.Year;
            var model = new NetReport(_db, year);
            return View();
        }

        //Product Summery
        public ActionResult ProductSummery(DateTime? fDateTime, DateTime tDateTime)
        {
            _db.Products.SoldReport(fDateTime, tDateTime);
            return View();
        }


        //Payment Summery
        public ActionResult PaymentSummery()
        {
            return View();
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