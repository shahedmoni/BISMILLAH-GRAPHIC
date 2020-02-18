using BismillahGraphic.DataCore;
using BismillahGraphic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace BismillahGraphic.Controllers
{
    [Authorize]
    public class BasicController : Controller
    {
        private readonly IUnitOfWork _db;

        private readonly RoleManager<IdentityRole> _roleManager;
        public BasicController()
        {
            _db = new UnitOfWork(new DataContext());
            var context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        //GET: institution
        public ActionResult Institution()
        {
            var model = _db.Institutions.FindCustom();
            return View(model);
        }

        //POST: institution
        [HttpPost]
        public ActionResult Institution(InstitutionVM model)
        {
            _db.Institutions.UpdateCustom(model);
            _db.SaveChanges();

            return RedirectToAction($"Index", $"Dashboard");
        }

        //Login Info
        [Authorize(Roles = "Admin, Sub-Admin")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public string GetUserLoggedInInfo()
        {
            var admin = _db.Registrations.GetAdminBasic(User.Identity.Name);
            return JsonConvert.SerializeObject(admin);
        }


        //Side Menu
        [Authorize(Roles = "Admin, Sub-Admin")]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public string GetSideMenu()
        {
            var data = _db.PageLinks.GetSideMenuByUser(User.Identity.Name);
            return JsonConvert.SerializeObject(data);
        }


        /******PAGE ACCESS ROLE********/
        public ActionResult PageRole()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        //GET
        public ActionResult CreateRole()
        {
            var role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        public ActionResult CreateRole(IdentityRole role)
        {
            if (role.Name == null) return View();
            if (_roleManager.RoleExists(role.Name)) return View();
            _roleManager.Create(role);

            return RedirectToAction("PageRole");
        }

        public bool DeleteRole(string roleName)
        {
            if (roleName == null) return false;

            var role = _roleManager.FindByName(roleName);
            if (role.Users.Count > 0) return false;

            var r = _roleManager.Delete(role);
            return r.Succeeded;
        }

        // Page Links
        public ActionResult PageLink()
        {
            ViewBag.roleList = _roleManager.Roles.Select(r => new RoleDDL { RoleId = r.Id, Role = r.Name }).ToList();

            var model = _db.PageLinkCategorys.GetCategoryWithLink();
            return View(model);
        }

        public ActionResult CreateLinks()
        {
            ViewBag.roleList = _roleManager.Roles.Select(r => new RoleDDL { RoleId = r.Id, Role = r.Name }).ToList();
            ViewBag.Category = _db.PageLinkCategorys.ddl();
            return View();
        }

        [HttpPost]
        public ActionResult CreateLinks(PageLink model)
        {
            if (!ModelState.IsValid) return View();

            _db.PageLinks.Add(model);
            _db.SaveChanges();

            return RedirectToAction("PageLink");
        }

        public bool PageLinkUpdate(int linkId, string roleId)
        {
            var linkage = _db.PageLinkCategorys.LinkRoleUpdate(linkId, roleId);
            _db.PageLinks.Update(linkage);
            var r = _db.SaveChanges();

            return r > 0;
        }
    }
}
