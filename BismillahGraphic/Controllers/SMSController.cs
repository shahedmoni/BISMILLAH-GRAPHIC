using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BismillahGraphic.DataCore;

namespace BismillahGraphic.Controllers
{
    public class SMSController : Controller
    {
        // GET: Vendor SMS
        public ActionResult VendorSMS()
        {
            return View();
        }

        //POST: Vendor SMS
        [HttpPost]
        public ActionResult VendorSMS(VendorVM model)
        {
            return View(model);
        }
    }
}