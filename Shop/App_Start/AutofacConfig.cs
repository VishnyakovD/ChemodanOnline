using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Shop.db;
using Shop.Logger;
using Shop.ServiceData;


namespace Shop
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new LoggerModule());

            builder.RegisterModule(new ImagesPathModule());

            builder.RegisterModule(new RegisterModules());

            builder.RegisterModule(new DbModule(ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString));

            builder.RegisterModule(new DataServiceModule());

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            // MVC Resolver
            var autofacResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(autofacResolver);

            // Create the depenedency resolver.
            //var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            //GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}