using BismillahGraphic.DataCore;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize]
    public class SellingController : Controller
    {
        private readonly IUnitOfWork _db;

        public SellingController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Selling
        [Authorize(Roles = "Admin, Selling")]
        public ActionResult Selling()
        {
            return View();
        }

        public async Task<JsonResult> FindVendor(string prefix)
        {
            var data = await _db.Vendors.SearchAsync(prefix);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FindProduct(string prefix)
        {
            var data = await _db.Products.SearchAsync(prefix);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // POST: Selling
        public async Task<int> PostSelling(SellingVM model)
        {
            if (model.SellingTotalPrice <= 0) return 0;

            model.RegistrationID = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            model.SellingSN = _db.Selling.GetSellingSN();
            model.ReceiptSN = _db.SellingPaymentReceipts.GetReceiptSN();
            var selling = _db.Selling.Selling(model);

            await _db.SaveChangesAsync();

            _db.Vendors.UpdatePaidDue(model.VendorID);
            var status = _db.SaveChanges();

            return status != 0 ? selling.SellingID : status;
        }

        //GET: Receipt
        public ActionResult Receipt(int? id)
        {
            if (id == null) return RedirectToAction($"Selling");

            var data = _db.Selling.Sold(id.GetValueOrDefault());
            data.InstitutionInfo = _db.Institutions.FindCustom();

            return View(data);
        }

        //GET: Change Receipt
        public ActionResult ReceiptChange(int? id)
        {
            if (id == null) return RedirectToAction("Record");
            var data = _db.Selling.FindUpdateBill(id.GetValueOrDefault());
            return View(data);
        }

        //Post: Change Receipt
        [HttpPost]
        public async Task<int> ReceiptChange(SellingBillChangeViewModel model)
        {
            if (model.SellingTotalPrice <= 0) return 0;
            _db.Selling.BillUpdated(model);
            await _db.SaveChangesAsync();

            return model.SellingID;
        }
        //GET: Record
        [Authorize(Roles = "Admin, SellingRecord")]
        public ActionResult Record()
        {
            return View();
        }

        public JsonResult IndexData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }

        //GET: Due Collection
        public ActionResult DueCollection(int? id)
        {
            if (id == null) return RedirectToAction($"Record");
            var model = _db.Selling.Sold(id.GetValueOrDefault());

            return View(model);
        }

        [HttpPost]
        public ActionResult DueCollection(InvoicePaySingle model)
        {
            model.RegistrationID = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction($"DueCollection");

            model.ReceiptSN = _db.SellingPaymentReceipts.GetReceiptSN();

            if (!_db.Selling.dueCollection(model)) return RedirectToAction($"DueCollection");

            _db.SaveChanges();
            _db.Vendors.UpdatePaidDue(model.VendorID);
            _db.SaveChanges();

            return Content("success");
        }

        public void DeleteBill(int id)
        {
            var Isdelete = _db.Selling.DeleteBill(id);

            if (Isdelete) _db.SaveChanges();
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