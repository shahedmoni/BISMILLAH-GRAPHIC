using BismillahGraphic.DataCore;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize(Roles = "Admin, Expanse")]
    public class ExpansesController : Controller
    {
        private readonly IUnitOfWork _db;

        public ExpansesController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // GET: Expanses
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IndexData()
        {
            var data = _db.Expanses.ToListCustom();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Expanses/Create
        public ActionResult Create()
        {
            ViewBag.ExpanseCategoryID = new SelectList(_db.ExpanseCategories.ddl(), "value", "label");
            return View($"_Create");
        }

        // POST: Expanses/Create
        [HttpPost]
        public async Task<ActionResult> Create(ExpanseVM model)
        {
            model.RegistrationID = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            if (!ModelState.IsValid) return View($"_Create", model);

            ViewBag.ExpanseCategoryID = new SelectList(_db.ExpanseCategories.ddl(), "value", "label", model.ExpanseCategoryID);


            _db.Expanses.AddCustom(model);

            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }


        // POST: Delete/5
        public int Delete(int id)
        {
            _db.Expanses.RemoveCustom(id);
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
