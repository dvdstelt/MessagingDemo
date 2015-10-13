using Autofac;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Host.Config
{
	internal class ContainerSetup
	{
		static readonly ILog Log = LogManager.GetLogger(typeof(ContainerSetup));

		public static IContainer Create()
		{
			Log.Info("Initializing dependancy injection...");

			var builder = new ContainerBuilder();
			// builder.RegisterType<BookingContext>().As<IBookingContext>();

			return builder.Build();
		}
	}
}
