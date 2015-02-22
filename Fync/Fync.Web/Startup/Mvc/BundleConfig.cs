using System.Web.Optimization;

namespace Fync.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/Scripts")
                .Include());

            bundles.Add(new LessBundle("~/Content/Css")
                .Include("~/Styles/Site.less"));

        }
    }
}
