using System.Web.Optimization;
using System.Web.Optimization.React;

namespace Fync.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new JsxBundle("~/Bundles/Scripts")
                .Include("~/Scripts/lib/moment.js")
                .Include("~/Scripts/lib/flux.js")
                .Include("~/Scripts/lib/jquery.js")
                .Include("~/Scripts/lib/react.js")
                .Include("~/Scripts/lib/require.js")
                .Include("~/Scripts/bootStrap.js")
                .IncludeDirectory("~/Scripts/react/", "*.js", searchSubdirectories: true)
                .IncludeDirectory("~/Scripts/react/", "*.jsx", searchSubdirectories: true));

            bundles.Add(new LessBundle("~/Content/Css")
                .Include("~/Styles/Styles.less"));
        }
    }
}
