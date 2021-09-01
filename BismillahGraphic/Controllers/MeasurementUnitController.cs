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
    public class MeasurementUnitController : Controller
    {
        private readonly IUnitOfWork _db;

        public MeasurementUnitController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }


        //Call from ajax
        public JsonResult IndexData()
        {
            var data = _db.ExpanseCategories.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View($"_Create");
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Create(ExpanseCategory model)
        {
            var exist = _db.ExpanseCategories.Any(n => n.CategoryName == model.CategoryName);

            if (exist) ModelState.AddModelError("CategoryName", "Category Name already exist!");
            if (!ModelState.IsValid) return View($"_Create", model);

            _db.ExpanseCategories.Add(model);

            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }


        // GET: Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = _db.ExpanseCategories.Find(id.GetValueOrDefault());

            if (model == null) return HttpNotFound();
            if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Edit(ExpanseCategory model)
        {
            var exist = _db.ExpanseCategories.Any(n => (n.CategoryName == model.CategoryName) && n.ExpanseCategoryID != model.ExpanseCategoryID);
            if (exist) ModelState.AddModelError("CategoryName", "Category Name must be unique!");

            if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);


            _db.ExpanseCategories.Update(model);
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
            if (!_db.ExpanseCategories.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }
    }
}