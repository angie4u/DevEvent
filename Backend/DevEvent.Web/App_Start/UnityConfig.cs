using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using DevEvent.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using DevEvent.Data.Services;

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

            // Admin ����� ����
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<UserManager<ApplicationUser>>();
            container.RegisterType<DbContext, ApplicationDbContext>();
            container.RegisterType<ApplicationUserManager>();

            // DevEvent ����
            container.RegisterType<IEventService, EventService>();

            // Azure Storage Blob ���� 
            container.RegisterType<IStorageService, AzureStorageService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}