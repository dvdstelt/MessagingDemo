using Autofac;
using Common.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Host.Config
{
	public class EndpointConfig : IConfigureThisEndpoint, IDisposable, AsA_Server
	{
		static readonly ILog Log = LogManager.GetLogger(typeof(EndpointConfig));
		readonly IContainer Container;

		/// <summary>
		/// Constructor
		/// </summary>
		public EndpointConfig()
		{
			Log.Info("Setup logging");
			NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

			Container = ContainerSetup.Create();
		}

		public void Customize(BusConfiguration configuration)
		{
			Log.Info("Customize...");
			configuration.EndpointName("orders");
			configuration.UseSerialization<JsonSerializer>();
			//configuration.DisableFeature<SecondLevelRetries>();
			//configuration.DisableFeature<TimeoutManager>();
			configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(Container));
			configuration.UseTransport<MsmqTransport>();
			configuration.UsePersistence<NHibernatePersistence>();
			configuration.EnableOutbox();

			ConventionsBuilder conventions = configuration.Conventions();
			conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace == "Orders.Messages.Commands");
			conventions.DefiningEventsAs(t => t.Namespace != null && (t.Namespace == "Orders.Messages.Events" || t.Namespace == "Orders.Messages.EventDocuments"));

			//RegisterMappings.Init();

			configuration.EnableInstallers();
		}

		public void Dispose()
		{
			if (Container != null)
				Container.Dispose();
		}
	}
}
