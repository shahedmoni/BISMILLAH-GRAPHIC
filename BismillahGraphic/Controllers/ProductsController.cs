using BismillahGraphic.DataCore;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize(Roles = "Admin, Product")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductsController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        //Call from ajax
        public async Task<JsonResult> IndexData()
        {
            var data = await _db.Products.ToListCustomAsync();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories.ddl(), "value", "label");
            return View($"_Create");
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<ActionResult> Create(ProductVM model)
        {
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories.ddl(), "value", "label", model.ProductCategoryID);

            var exist = _db.Products.Any(n => n.ProductName == model.ProductName);

            if (exist) ModelState.AddModelError("ProductName", "Product Name already exist!");
            if (!ModelState.IsValid) return View($"_Create", model);


            _db.Products.AddCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = _db.Products.FindCustom(id);
            if (model == null) return HttpNotFound();

            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories.ddl(), "value", "label", model.ProductCategoryID);
            if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ProductVM model)
        {
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryID", "ProductCategoryName", model.ProductCategoryID);

            var exist = _db.Products.Any(n => (n.ProductName == model.ProductName) && n.ProductID != model.ProductID);
            if (exist) ModelState.AddModelError("ProductName", "Product Name must be unique!");

            if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);

            _db.Products.UpdateCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
                return Request.IsAjaxRequest() ? (ActionResult)Content("success") : RedirectToAction("Index");

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);
        }

        //delete
        public int Delete(int id)
        {
            if (!_db.Products.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
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
