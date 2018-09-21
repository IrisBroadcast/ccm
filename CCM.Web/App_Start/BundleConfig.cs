using System.Web.Optimization;

namespace CCM.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Inkluderas på adminsidor för Location, Profile & UserAgents. Troligen används bara sortable. Kolla.
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            // Inkluderas på statistik-sidan
            bundles.Add(new ScriptBundle("~/bundles/statistics").Include(
                "~/Scripts/src/statistics.js"));

            // Inkluderas i förstasidan
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/angular.js",
                "~/Scripts/src/home.js",
                "~/Scripts/src/codeccontrol.js",
                "~/Scripts/src/studiomonitor.js",
                "~/Scripts/ngStorage.js",
                "~/Scripts/ui-bootstrap-tpls-{version}.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/signalr.js"
            ));

            // Inkluderas i samtliga sidor
            bundles.Add(new ScriptBundle("~/bundles/ccm").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap-colorpicker.js",
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/src/ccm.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/less/Site.css",
                "~/Content/less/colorpicker.css",
                "~/Content/bootstrap-datetimepicker-build.css"
            ));

        }
    }
}
