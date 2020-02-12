using BismillahGraphic.DataCore;
using System.Web.Mvc;

namespace BismillahGraphic.Controllers
{
    public class ProductCategoryModelsController : Controller
    {
        private IUnitOfWork db = new UnitOfWork(new DataContext());


        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.ProductCategories.ToListAsync());
        //}


        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductCategoryModel productCategoryModel = await db.ProductCategoryModels.FindAsync(id);
        //    if (productCategoryModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productCategoryModel);
        //}


        //public ActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ProductCategoryID,ProductCategoryName")] ProductCategoryModel productCategoryModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ProductCategoryModels.Add(productCategoryModel);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(productCategoryModel);
        //}


        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductCategoryModel productCategoryModel = await db.ProductCategoryModels.FindAsync(id);
        //    if (productCategoryModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productCategoryModel);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ProductCategoryID,ProductCategoryName")] ProductCategoryModel productCategoryModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(productCategoryModel).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(productCategoryModel);
        //}

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductCategoryModel productCategoryModel = await db.ProductCategoryModels.FindAsync(id);
        //    if (productCategoryModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productCategoryModel);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    ProductCategoryModel productCategoryModel = await db.ProductCategoryModels.FindAsync(id);
        //    db.ProductCategoryModels.Remove(productCategoryModel);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
