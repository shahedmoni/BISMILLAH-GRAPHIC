﻿using BismillahGraphic.DataCore;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize(Roles = "Admin, SMS")]
    public class SMSController : Controller
    {
        private readonly IUnitOfWork _db;

        public SMSController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Vendor SMS
        public ActionResult VendorSMS()
        {
            return View();
        }

        //POST: Vendor SMS
        [HttpPost]
        public ActionResult VendorSMS(SmsSendMultipleVendorVM model)
        {
            var r = _db.SMS.SendMultipleToVendor(model);
            if (r == "Success") _db.SaveChanges();
            return Json(r);
        }


        // GET: SingleSMS
        public ActionResult SingleSMS()
        {
            return View();
        }

        //POST: SingleSMS
        [HttpPost]
        public ActionResult SingleSMS(SmsSendSingleVM model)
        {
            return Json(model);
        }


        //for sms balance.. from ajax
        public int SmsBalance()
        {
            return _db.SMS.SmsBalance();
        }

    }
}