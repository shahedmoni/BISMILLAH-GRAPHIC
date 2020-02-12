using BismillahGraphic.DataCore;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _db;

        public HomeController()
        {
            _db = new UnitOfWork(new DataContext());
        }
        public ActionResult Index()
        {
            var model = _db.Institutions.HomeInfo();
            return View(model);
        }
    }
}