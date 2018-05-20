using System.Web;
using System.Web.Optimization;

namespace Storage.WebApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Assets/Scripts/JS").Include(
    "~/Assets/Scripts/jquery-{version}.js",
    "~/Assets/Scripts/modernizr-*",
    "~/Assets/Scripts/respond.js",
    "~/Assets/Scripts/ripples.js",
    "~/Assets/Scripts/material.js",
    "~/Assets/Scripts/jquery.validate.js",
    "~/Assets/Scripts/jquery.validate.unobtrusive.js",
    "~/Assets/Scripts/angular/angular.js",
    "~/Assets/Scripts/angular/angular-route.js",
    "~/Assets/Scripts/angular/angular-resource.js",
    "~/Assets/Scripts/angular/angular-animate.js",
    "~/Assets/Scripts/angular/angular-loader.js",
    "~/Assets/Scripts/angular/angular-aria.js",
    "~/Assets/Scripts/angular/angular-cookies.js",
    "~/Assets/Scripts/angular/angular-message-format.js",
    "~/Assets/Scripts/angular/angular-messages.js",
    "~/Assets/Scripts/angular/angular-mocks.js",
    "~/Assets/Scripts/angular/angular-sanitize.js",
    "~/Assets/Scripts/angular/angular-scenario.js",
    "~/Assets/Scripts/angular/angular-touch.js",
    "~/Assets/Scripts/angular/i18n/angular-locale_en-us.js",
    "~/Assets/Scripts/angular/ui-bootstrap.js",
    "~/Assets/Scripts/angular/ui-bootstrap-tpls.js",
    "~/App/app.js"
    ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Assets/Styles/Css").Include(
                "~/Assets/Styles/bootstrap.css",
                "~/Assets/Styles/roboto.css",
                "~/Assets/Styles/ripples.css",
                "~/Assets/Styles/material.css",
                "~/Assets/Styles/styles.css"
                ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
