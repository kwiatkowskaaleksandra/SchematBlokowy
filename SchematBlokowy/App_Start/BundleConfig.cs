using System.Web.Optimization;

namespace SchematBlokowy
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region scriptBundle
            var scriptBundle = new ScriptBundle("~/Scripts/jquery");
            scriptBundle.Include(
                "~/Scripts/jquery-3.4.1.min.js",
                 "~/Scripts/js.cookie.js",
                 "~/Scripts/jquery.unobtrusive-ajax.min.js"
                 );

            // CLDR scripts
            scriptBundle
                .Include("~/Scripts/cldr.js")
                .Include("~/Scripts/cldr/event.js")
                .Include("~/Scripts/cldr/supplemental.js")
                .Include("~/Scripts/cldr/unresolved.js");

            bundles.Add(scriptBundle);
            #endregion

            #region Modernizr
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            #endregion

            #region bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css",
                      "~/Content/bootstrap.min.css"
                      ));
            #endregion

            #region jquery
            bundles.Add(new ScriptBundle("~/bundles/jqueryBase64").Include(
                "~/Scripts/jquery.base64.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            #endregion

#if !DEBUG
                                BundleTable.EnableOptimizations = false;
#endif
        }
    }
}
