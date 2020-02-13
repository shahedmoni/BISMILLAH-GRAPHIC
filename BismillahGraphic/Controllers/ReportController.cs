﻿using BismillahGraphic.DataCore;
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

        public JsonResult GetIncome(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.Selling.IncomeDateToDate(fromDate, toDate);
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

        public JsonResult GetSelling(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.Selling.SellDateToDate(fromDate, toDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        // GET: Vendor
        [Authorize(Roles = "Admin, Report_Vendor")]
        public ActionResult Vendor()
        {
            return View();
        }

        public JsonResult GetVendor()
        {
            var model = _db.Vendors.PaidDues();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        //Income Summery
        public ActionResult SalesSummery()
        {
            return View();
        }

        //Product Summery
        public ActionResult ProductSummery()
        {
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