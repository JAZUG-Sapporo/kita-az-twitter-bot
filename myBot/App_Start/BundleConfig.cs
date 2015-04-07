using System.Web;
using System.Web.Optimization;

namespace myBot
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/materialize.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/mybot-common.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/materialize.css",
                      "~/Content/site.css"));
        }
    }
}
