using System.Web.Optimization;
using NGZB.Controllers;

namespace NGZB
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862 ，使用此技术必须.net4.5以上
        public static void RegisterBundles(BundleCollection bundles)
        {
            //全局js文件加载
            bundles.Add(new ScriptBundle("~/Scripts/js") { Orderer = new Models.Class.AsIsBundleOrderer() }.Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/jquery.validate.js",
                            "~/Scripts/modernizr-{version}.js",
                            "~/Scripts/respond.js",
                            "~/Scripts/iehtml5.js",
                            "~/Scripts/jQuery.md5.js",
                            "~/Scripts/layer/layer.js",
                            "~/Scripts/myJs.js"
                            ));
            //全局css文件加载
            string _easyuithemes = "~/Content/themes/themestemplet/" + SysController.__SystemThems() + "/easyui.css";
            bundles.Add(new StyleBundle("~/Content/css")
                   .Include(_easyuithemes, new CssRewriteUrlTransform())
                   .Include("~/Content/awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/icon.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/myicon.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/color.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/site.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/classcss.css", new CssRewriteUrlTransform())
                   .Include("~/Content/themes/idcss.css", new CssRewriteUrlTransform())
                   .Include("~/Scripts/layer/skin/layer.css", new CssRewriteUrlTransform())
                   );
            //登录界面js文件加载
            bundles.Add(new ScriptBundle("~/Scripts/loginjs") { Orderer = new Models.Class.AsIsBundleOrderer() }.Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/Content/login/js/supersized.min.js",
                            "~/Content/login/js/supersized-init.js",
                            "~/Content/login/js/scripts.js",
                            "~/Scripts/jQuery.md5.js",
                            "~/Scripts/layer/layer.js",
                            "~/ Scripts/iehtml5.js"
                            ));
            //登录界面css文件加载
            bundles.Add(new StyleBundle("~/Content/logincss")
                  .Include("~/Content/login/css/reset.css", new CssRewriteUrlTransform())
                  .Include("~/Content/login/css/supersized.css", new CssRewriteUrlTransform())
                  .Include("~/Content/login/css/style.css", new CssRewriteUrlTransform())
                  .Include("~/Scripts/layer/skin/layer.css", new CssRewriteUrlTransform())
                  );
        }
    }
}