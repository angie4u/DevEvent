using DevEvent.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DevEvent.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeStorage();
        }

        /// <summary>
        /// Storage Account 초기화
        /// </summary>
        private async void InitializeStorage()
        {
            AzureStorageService storagesvc = new AzureStorageService();
            await storagesvc.CreateContainerAsync("images", true);
            await storagesvc.CreateContainerAsync("thumbs", true);

            // Queue for thumbnail
            AzureQueueService queuesvc = new AzureQueueService();
            await queuesvc.CreateQueueAsync("thumbrequestqueue");
        }
    }
}
