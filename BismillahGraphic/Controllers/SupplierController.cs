using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BismillahGraphic.DataCore;

namespace BismillahGraphic.Controllers
{
   
    [Authorize(Roles = "Admin, Vendor")]
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _db;

        public SupplierController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Vendors

        public ActionResult Index()
        {
            return View();
        }

        //Call from ajax
        public JsonResult IndexData(DataRequest request)
        {
            var data = _db.Suppliers.ToListCustom(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // GET: Vendors/Create
        public ActionResult Create()
        {
            return View($"_Create");
        }

        // POST: Vendors/Create
        [HttpPost]
        public async Task<ActionResult> Create(SupplierVM model)
        {
            if (_db.Vendors.IsExistSmsNumber(0, model.SmsNumber))
                ModelState.AddModelError("SmsNumber", "SMS Number Already exist");

            if (!ModelState.IsValid) return View($"_Create", model);

            var vendor = _db.Suppliers.AddCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
            {
                model.SupplierID = vendor.SupplierID;
                var result = new AjaxContent<SupplierVM> { Status = true, Data = model };
                return Json(result);
            }

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = _db.Suppliers.FindCustom(id);

            if (model == null) return HttpNotFound();
            if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        // POST: Vendors/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(SupplierVM model)
        {
            if (_db.Vendors.IsExistSmsNumber(model.SupplierID, model.SmsNumber))
                ModelState.AddModelError("SmsNumber", "SMS Number Already exist");

            if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);

            _db.Suppliers.UpdateCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
                return Request.IsAjaxRequest() ? (ActionResult)Content("success") : RedirectToAction("Index");

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);
        }

        // POST: Delete/5
        public int Delete(int id)
        {
            if (!_db.Suppliers.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }

        //GET: Details
        public ActionResult Details(int? id, DateTime? fromDate, DateTime? toDate)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.id = id;

            return View(new VendorDetails(_db, id.GetValueOrDefault(), fromDate, toDate));
        }

        //Call from ajax
        public JsonResult GetDetails(int? id, DateTime? fromDate, DateTime? toDate)
        {
            var data = new VendorDetails(_db, id.GetValueOrDefault(), fromDate, toDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET: PayDue Receipt
        public ActionResult DueReceipt(int? id, DateTime? fromDate, DateTime? toDate)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.id = id;

            return View(new VendorDetails(_db, id.GetValueOrDefault(), fromDate, toDate));
        }

        //Post: receipt
        [HttpPost]
        public int PostReceipt(PaymentReceipt model)
        {
            if (model.PaidAmount <= 0) return 0;

            model.RegistrationID = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            model.ReceiptSN = _db.SellingPaymentReceipts.GetReceiptSN();
            var receipt = _db.SellingPaymentReceipts.dueCollection(model);
            
            if (receipt == null) return 0;

            _db.SaveChanges();
            _db.Vendors.UpdatePaidDue(model.VendorID);
            var status = _db.SaveChanges();

            return status != 0 ? receipt.ReceiptID : status;
        }

        //receipt
        public ActionResult PaidReceipt(int? id)
        {
            if (id == null) return RedirectToAction($"Index");

            var data = _db.SellingPaymentReceipts.Print(id.GetValueOrDefault());
            data.InstitutionInfo = _db.Institutions.FindCustom();

            return View(data);
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