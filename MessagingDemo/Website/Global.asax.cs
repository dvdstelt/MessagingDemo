using Autofac;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Website.App_Start;
using Website.Code.ServiceBus;

namespace Website
{
  public class MvcApplication : System.Web.HttpApplication
  {
    private readonly ILog Log = LogManager.GetLogger<MvcApplication>();
    private static IContainer _container;


    protected void Application_Start()
    {
      Log.Info("Start");
      InitLog();

      _container = DependenciesConfig.RegisterDependencies();

      ServiceBus.Init(_container);

      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    protected void Application_Stop()
    {
      Log.Info("Stop");
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
      Log.TraceFormat("======= BeginRequest: {0} [{1}] =======", Context.Request.RawUrl, Context.Request.Url);
    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {
      Log.TraceFormat("======= EndRequest: {0} [{1}] =======", Context.Request.RawUrl, Context.Request.Url);
    }

    private void InitLog()
    {
      if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogFirstChangeExceptions"]))
      {
        Log.Info("Logging FirstChanceExceptions.");
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.FirstChanceException += currentDomain_FirstChanceException;
      }
    }
    private void currentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
      Log.Trace("FirstChanceException", e.Exception);
    }

  }
}
