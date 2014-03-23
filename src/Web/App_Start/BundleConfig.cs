using System;
using System.Web.Optimization;

namespace Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Styles/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            // Metronic Theme
            var styleBundle = new Bundle("~/Styles/themes/metronic")
                    .Include("~/Content/themes/metronic/plugins/bootstrap/css/bootstrap.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/bootstrap-toggle-buttons/static/stylesheets/bootstrap-toggle-buttons.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/bootstrap/css/bootstrap-responsive.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/jquery-ui/jquery-ui-{version}.custom.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/style-metro.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/chosen-bootstrap/chosen/chosen.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/style.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/fonts/font.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/style-responsive.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/themes/default.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/style-metronic.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/uniform/css/uniform.default.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/plugins/gritter/css/jquery.gritter.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/metronic/css/pages/login.css", new CssRewriteUrlTransform())
                    .Include("~/Content/styles/custom.css", new CssRewriteUrlTransform()); 
            bundles.Add(styleBundle);

            bundles.Add(new ScriptBundle("~/Content/themes/metronic/scripts/global").Include(
                        "~/Content/themes/metronic/plugins/jquery-1.8.3.min.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Content/themes/metronic/plugins/bootstrap-toggle-buttons/static/js/jquery.toggle.buttons.js",
                        "~/Content/scripts/global.js",
                        "~/Content/themes/metronic/plugins/jquery-ui/jquery-ui-1.10.1.custom.min.js",
                        "~/Content/themes/metronic/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/themes/metronic/plugins/breakpoints/breakpoints.js",
                        "~/Content/themes/metronic/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/themes/metronic/plugins/jquery.blockui.js",
                        "~/Content/themes/metronic/plugins/jquery.cookie.js",
                        "~/Content/themes/metronic/plugins/uniform/jquery.uniform.min.js",
                        "~/Content/themes/metronic/plugins/gritter/js/jquery.gritter.js",
                        "~/Content/themes/metronic/plugins/chosen-bootstrap/chosen/chosen.jquery.min.js",
                        "~/Scripts/jquery.maskedinput-{version}.js",
                        "~/Scripts/jquery.maskMoney.js",
                        "~/Scripts/digdin.js",
                        "~/Content/themes/metronic/scripts/app.js",
                        "~/Scripts/signalr-alerts.js"
              ));

            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }

    }
}