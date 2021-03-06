﻿using System.Web;
using System.Web.Optimization;

namespace DevEvent.Web
{
    public class BundleConfig
    {
        // 번들 작성에 대한 자세한 내용은 http://go.microsoft.com/fwlink/?LinkId=301862 링크를 참조하십시오.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/views/adminevent/create").Include(
                         "~/js/views/adminevent-create.js"));

            bundles.Add(new ScriptBundle("~/js/views/adminevent/index").Include(
                         "~/js/views/adminevent-index.js"));

            bundles.Add(new ScriptBundle("~/js/views/adminevent/edit").Include(
                         "~/js/views/adminevent-edit.js"));

            bundles.Add(new ScriptBundle("~/js/views/account/register").Include(
                         "~/js/views/account-register.js"));

        }
    }
}
