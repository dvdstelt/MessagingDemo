using Autofac;
using Autofac.Integration.Mvc;
using Sales.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Website.Code;

namespace Website.App_Start
{
	public class DependenciesConfig
	{
		public static IContainer RegisterDependencies()
		{
			ContainerBuilder builder = new ContainerBuilder();

      builder.RegisterModule(new AutofacWebTypesModule());
      builder.RegisterControllers(Assembly.GetExecutingAssembly());

      //Needed later
      //var config = GlobalConfiguration.Configuration;
      //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
      //builder.RegisterWebApiFilterProvider(config);

      builder.RegisterType<CacheHelper>().As<ICacheHelper>();

      //
      // *** Register data contexts
      //
      builder
        .RegisterType<SalesContext>()
        .As<ISalesContext>()
        .InstancePerLifetimeScope();

      builder.RegisterFilterProvider();
			var container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
      //Needed later
      //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

			return container;
		}
	}
}