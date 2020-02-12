using BismillahGraphic.DataCore;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    [Authorize(Roles = "Admin, Sub-Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _db;

        public DashboardController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        public ActionResult Index(int? id)
        {
            var model = id != null ? new Dashboard(_db, id.GetValueOrDefault()) : new Dashboard(_db);
            model.JsonChartData = JsonConvert.SerializeObject(model.MonthlyReports);

            return View(model);
        }

        //GET: Profile update
        public ActionResult UserProfile()
        {
            var user = _db.Registrations.GetAdminInfo(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(AdminInfo user)
        {
            if (!ModelState.IsValid) return View(user);

            _db.Registrations.UpdateCustom(User.Identity.Name, user);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}