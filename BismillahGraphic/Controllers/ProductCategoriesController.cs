using BismillahGraphic.DataCore;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize(Roles = "Admin, ProductCategory")]
    public class ProductCategoriesController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductCategoriesController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: ProductCategories
        public ActionResult IndexView()
        {
            return View();
        }
        //Call from ajax
        public JsonResult IndexData()
        {
            var data = _db.ProductCategories.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View($"_Create");
        }

        // POST: Create
        [HttpPost]
        public async Task<ActionResult> Create(ProductCategoryVM model)
        {
            var exist = _db.ProductCategories.Any(n => n.ProductCategoryName == model.ProductCategoryName);

            if (exist) ModelState.AddModelError("ProductCategoryName", "Category Name already exist!");
            if (!ModelState.IsValid) return View($"_Create", model);


            _db.ProductCategories.AddCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = _db.ProductCategories.FindCustom(id);

            if (model == null) return HttpNotFound();
            if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        // POST: ProductCategories/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ProductCategoryVM model)
        {
            var exist = _db.ProductCategories.Any(n => (n.ProductCategoryName == model.ProductCategoryName) && n.ProductCategoryID != model.ProductCategoryID);
            if (exist) ModelState.AddModelError("ProductCategoryName", "Category Name must be unique!");

            if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);

            _db.ProductCategories.UpdateCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
                return Request.IsAjaxRequest() ? (ActionResult)Content("success") : RedirectToAction("IndexView");

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);
        }

        //delete
        public int Delete(int id)
        {
            if (!_db.ProductCategories.RemoveCustom(id)) return -1;
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
