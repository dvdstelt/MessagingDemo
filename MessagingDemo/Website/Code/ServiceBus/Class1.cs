using Autofac;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Code.ServiceBus
{
  public class ServiceBus
  {
    public static ISendOnlyBus Bus { get; private set; }
    private static readonly object padlock = new object();

    public static void Init(ILifetimeScope container)
    {
      if (Bus != null) return;

      NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

      lock (padlock)
      {
        if (Bus != null) return;

        var configuration = new BusConfiguration();
        configuration.UseSerialization<JsonSerializer>();
        configuration.UseContainer<AutofacBuilder>(x => x.ExistingLifetimeScope(container));
        configuration.UseTransport<MsmqTransport>();

        ConventionsBuilder conventions = configuration.Conventions();
        conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Orders.Messages") && t.Namespace.EndsWith("Commands"));

        Bus = NServiceBus.Bus.CreateSendOnly(configuration);
      }
    }
  }
}