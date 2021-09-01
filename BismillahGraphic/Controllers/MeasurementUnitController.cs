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
            var data = _db.MeasurementUnits.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View($"_Create");
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Create(MeasurementUnit model)
        {
            var exist = _db.MeasurementUnits.Any(n => n.MeasurementUnitName == model.MeasurementUnitName);

            if (exist) ModelState.AddModelError("MeasurementUnitName", "Measurement Unit Name already exist!");
            if (!ModelState.IsValid) return View($"_Create", model);

            _db.MeasurementUnits.Add(model);

            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }


        // GET: Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = _db.MeasurementUnits.Find(id.GetValueOrDefault());

            if (model == null) return HttpNotFound();
            if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Edit(MeasurementUnit model)
        {
            var exist = _db.MeasurementUnits.Any(n => (n.MeasurementUnitName == model.MeasurementUnitName) && n.MeasurementUnitId != model.MeasurementUnitId);
            if (exist) ModelState.AddModelError("MeasurementUnitName", "Measurement Unit Name must be unique!");

            if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);


            _db.MeasurementUnits.Update(model);
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
            if (!_db.MeasurementUnits.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }
    }
}