using System.Web;
using System.Web.Optimization;

namespace SQuadro
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"/*
                        "~/Scripts/bootstrap-modal.js",
                        "~/Scripts/bootstrap-modalmanager.js"*/));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                "~/Scripts/date-time/moment.js",
                "~/Scripts/date-time/bootstrap-datepicker.js",
                "~/Scripts/date-time/bootstrap-timepicker.js",
                "~/Scripts/fuelux/fuelux.wizard.js",
                "~/Scripts/library/*.js",
                "~/Scripts/ListTemplate/*.js",
                "~/Scripts/FormScripts/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include("~/Scripts/jquery.dataTables.js",
                "~/Scripts/jquery.dataTables.responsive.js",
                "~/Scripts/jquery.dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendors").Include("~/Scripts/select2.js",
                "~/Scripts/jquery.bootstrap-growl.js",
                "~/Scripts/ace.js",
                "~/Scripts/ace-elements.js",
                "~/Scripts/ace-extra.js",
                "~/Scripts/jquery.autosize.js",
                "~/Scripts/jquery.maskedinput.js",
                "~/Scripts/wysihtml5-0.4.0pre.js",
                "~/Scripts/bootstrap-wysiwyg.js"));

            bundles.Add(new StyleBundle("~/styles/themes").Include("~/Content/jquery-ui-1.10.3.custom.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-wysihtml5.css",
                //"~/Content/bootstrap-modal.css",
                "~/Content/chosen.css",
                "~/Content/font-awesome-ie7.css",
                "~/Content/font-awesome.css",
                "~/Content/prettify.css",
                "~/Content/select2.css",
                "~/Content/ace-ie.css",
                "~/Content/ace-rtl.css",
                "~/Content/ace-skins.css",
                "~/Content/ace.css",
                "~/Content/site.css",
                "~/Content/site-listTemplate.css",
                "~/Content/site-dataTables-responsive.css",
                "~/Content/site-listTemplate-responsive.css",
                "~/Content/datepicker.css",
                "~/Content/bootstrap-timepicker.css"
            ));
        }
    }
}