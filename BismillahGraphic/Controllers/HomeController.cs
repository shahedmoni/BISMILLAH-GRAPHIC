using BismillahGraphic.DataCore;
using System.Web.Mvc;
using System.Web.UI;

namespace BismillahGraphic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _db;

        public HomeController()
        {
            _db = new UnitOfWork(new DataContext());
        }

        // [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            //var model = _db.Institutions.HomeInfo();
            return View();
        }
    }
}