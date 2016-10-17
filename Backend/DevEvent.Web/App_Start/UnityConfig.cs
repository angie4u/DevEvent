using System.Web.Mvc;
using Microsoft.Practices.Unity;
using DevEvent.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using DevEvent.Data.Services;
using DevEvent.Web.Controllers;
using System.Web.Http;

namespace DevEvent.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<AccountController>(new InjectionConstructor());

            // Admin 사용자 관련
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<UserManager<ApplicationUser>>();
            container.RegisterType<DbContext, ApplicationDbContext>();
            container.RegisterType<ApplicationUserManager>();

            
            // Azure Storage Blob 서비스 
            container.RegisterType<IStorageService, AzureStorageService>();
            container.RegisterType<IThumbnailService, ThumbnailService>();
            // DevEvent 서비스
            //container.RegisterType<IEventService, EventService>(new InjectionFactory((c,t,s) => new EventService(
            //    container.Resolve<ApplicationDbContext>(),
            //    container.Resolve<IStorageService>(),
            //    container.Resolve<IThumbnailService>()
            //    )));

            container.RegisterType<IEventService, EventService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}