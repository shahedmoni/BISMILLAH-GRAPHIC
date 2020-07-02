using BismillahGraphic.DataCore;
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
            return Json(model);
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