using System.Web;
using System.Web.Optimization;

namespace Roots.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-responsive.css"));

            bundles.Add(new ScriptBundle("~/bundles/molecule").Include(
                        "~/Scripts/Marvin/gui/gui.nocache.js",
                        "~/Scripts/molecule.js"));

            bundles.Add(new ScriptBundle("~/bundles/moleculecss").Include(
                        "~/Scripts/Marvin/gui/common/canvas.css",
                        "~/Scripts/Marvin/gui/common/error.css",
                        "~/Scripts/Marvin/gui/common/global.css",
                        "~/Scripts/Marvin/gui/common/periodicsystem.css",
                        "~/Scripts/Marvin/gui/common/toolbar.css"));
        }
    }
}
