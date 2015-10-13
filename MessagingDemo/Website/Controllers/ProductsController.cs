using Sales.Data.Context;
using Sales.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
  public class ProductsController : Controller
  {
    private readonly ISalesContext SalesContext;
    public ProductsController(ISalesContext context)
    {
      SalesContext = context;
    }

    public ActionResult ProductSelector()
    {
      ProductSelectorViewModel vm = new ProductSelectorViewModel();
      vm.Products = new List<SelectListItem>();
      SalesContext.Products.OrderBy(s => s.Description).ToList().ForEach(s => vm.Products.Add(new SelectListItem() { Text = string.Format("{0} ({1})", s.Description, s.Price), Value = s.Id.ToString() }));

      return View("_ProductSelector", vm);
    }

    public class ProductSelectorViewModel
    {
      public List<SelectListItem> Products { get; set; }
      public string ProductId { get; set; }
    }
  }
}