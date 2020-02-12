using System.Web.Optimization;

namespace BismillahGraphic
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/JS/jquery.validate*"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
