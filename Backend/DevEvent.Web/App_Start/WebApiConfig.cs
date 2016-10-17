using AutoMapper;
using DevEvent.Data.DataObjects;
using DevEvent.Data.Models;
using DevEvent.Data.Services;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DevEvent.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Auto mapping setup
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MobileEvent, Event>();
                cfg.CreateMap<Event, MobileEvent>()
                    .ForMember(dst => dst.CreateUserId, map => map.MapFrom(x => x.CreateUser.Id));

            });

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}