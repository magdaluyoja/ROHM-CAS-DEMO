using System.Web;
using System.Web.Optimization;

namespace ROHM_CAS_DEMO
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string[] LayoutJS = new string[]
            {
                "~/Content/assets/global/plugins/jquery.min.js",
                "~/Content/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                "~/Content/assets/global/plugins/js.cookie.min.js",
                "~/Content/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                "~/Content/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/assets/global/plugins/jquery.blockui.min.js",
                "~/Content/assets/global/plugins/uniform/jquery.uniform.min.js",
                "~/Content/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                "~/Content/assets/global/scripts/app.min.js",
                "~/Content/assets/layouts/layout/scripts/layout.min.js",
                "~/Content/assets/layouts/layout/scripts/demo.min.js",
                "~/Content/assets/layouts/global/scripts/quick-sidebar.min.js",
                "~/Content/js/Custom.js"
            };
            string[] LayoutCSS = new string[]
            {
                "~/Content/assets/global/plugins/font-awesome/css/font-awesome.min.css",
                "~/Content/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/Content/assets/global/plugins/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/global/plugins/uniform/css/uniform.default.css",
                "~/Content/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                "~/Content/assets/global/css/components-md.min.css",
                "~/Content/assets/global/css/plugins-md.min.css",
                "~/Content/assets/layouts/layout/css/layout.min.css",
                "~/Content/assets/layouts/layout/css/custom.min.css",
                "~/Content/css/Custom.css"
            };
            string[] TrxJS = new string[]
            {
                "~/Content/assets/global/plugins/babel.min.js",
                "~/Content/assets/global/plugins/core.min.js",
                "~/Content/js/DataTables/datatables.js",
                "~/Content/assets/global/plugins/DataTables-New/datatables.js",
                "~/Content/assets/global/plugins/iziToast/dist/js/iziToast.js",
                "~/Content/assets/global/plugins/Parsleyjs/dist/parsley.js",
                "~/Content/js/Classes/common/Message.js",
                "~/Content/js/Classes/common/Data.js"
            };
            string[] TrxCSS = new string[]
            {
                "~/Content/js/DataTables/datatables.css",
                "~/Content/assets/global/plugins/DataTables-New/datatables.css",
                "~/Content/assets/global/plugins/iziToast/dist/css/iziToast.css",
                "~/Content/assets/global/plugins/Parsleyjs/src/parsley.min.css"
            };

            var googleFonts = "http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all";


            bundles.Add(new StyleBundle("~/Login-CSS", googleFonts)
                    .Include(LayoutCSS)
                    .Include("~/Content/assets/pages/css/login.min.css")
            );
            bundles.Add(new ScriptBundle("~/Login-JS")
                    .Include(
                        "~/Content/assets/global/plugins/jquery.min.js",
                        "~/Content/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                        "~/Content/assets/global/plugins/bootstrap-toastr/toastr.min.js",
                        "~/Scripts/Helpers.js",
                        "~/Scripts/login.js"
                    )
            );

            bundles.Add(new StyleBundle("~/Home-CSS", googleFonts)

                    .Include(LayoutCSS)
            );
            bundles.Add(new ScriptBundle("~/Home-Js")
                    .Include(LayoutJS)
            );

            bundles.Add(new StyleBundle("~/MasterMaintenanceHome-CSS", googleFonts)
                    .Include(LayoutCSS)
            );
            bundles.Add(new ScriptBundle("~/MasterMaintenanceHome-Js")
                    .Include(LayoutJS)
            );

            bundles.Add(new StyleBundle("~/UserMaster-CSS", googleFonts)
                    .Include(LayoutCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/global/plugins/select2/css/select2.min.css",
                        "~/Content/assets/global/plugins/select2/css/select2-bootstrap.min.css"
                    )
            );
            bundles.Add(new ScriptBundle("~/UserMaster-Js")
                    .Include(LayoutJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/global/plugins/select2/js/select2.full.min.js",
                        "~/Areas/MasterMaintenance/Scripts/UserMasterClass.js",
                        "~/Areas/MasterMaintenance/Scripts/UserMaster.js"
                    )
            );
            //BundleTable.EnableOptimizations = true;
        }
    }
}
