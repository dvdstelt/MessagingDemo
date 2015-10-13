using Sales.Data.Context;
using Sales.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Code;

namespace Website.Controllers
{
	public class HomeController : Controller
	{
    private readonly ICacheHelper CacheHelper;
    public HomeController(ICacheHelper cacheHelper)
    {
      CacheHelper = cacheHelper;
    }

		public ActionResult Index()
		{
			return View();
		}

    public ActionResult AddToShoppingCart(string product)
    {
      var form = Request.Form["Product"];

      CacheHelper.Insert("LastAddedItem", form, DateTime.UtcNow.AddSeconds(5));

      return RedirectToAction("Index");
    }

    public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}