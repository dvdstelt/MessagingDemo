using Sales.Data.Context;
using Sales.Data.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Code;

namespace Website.Controllers
{
  public class ShoppingCartController : Controller
  {
    private const string ShoppingCartCacheKey = "shoppingCart";
    private readonly TimeSpan expirationTime = new TimeSpan(0, 1, 0);

    private readonly ICacheHelper CacheHelper;
    private readonly ISalesContext Context;
    public ShoppingCartController(ICacheHelper cacheHelper, ISalesContext context)
    {
      Context = context;
      CacheHelper = cacheHelper;
    }

    public ActionResult AddToShoppingCart(string productId)
    {
      Guid productGuid = Guid.Parse(productId);
      Product product = Context.Products.Where(s => s.Id == productGuid).Single();

      // Dit moet met NServiceBus
      Context.OrderedItems.Add(new OrderedItem() { Product = product, Id = Guid.NewGuid() });
      Context.SaveChanges();

      var shoppingCart = CacheHelper.GetOrAdd<List<Product>>(ShoppingCartCacheKey, f => GetOrderedItems(), DateTime.UtcNow.Add(expirationTime));
      shoppingCart.Add(product);
      CacheHelper.Insert(ShoppingCartCacheKey, shoppingCart, DateTime.UtcNow.Add(expirationTime));

      return RedirectToAction("Index", "Home");
    }

    public ActionResult RemoveItem(string id)
    {
      var productId = Guid.Parse(id);

      var shoppingCart = CacheHelper.GetOrAdd<List<Product>>(ShoppingCartCacheKey, f => GetOrderedItems(), DateTime.UtcNow.Add(expirationTime));
      var index = shoppingCart.FindIndex(s => s.Id == productId);
      shoppingCart.RemoveAt(index);

      // Should be handled by NServiceBus
      OrderedItem orderedItem = Context.OrderedItems.Include(s => s.Product).Where(s => s.Product.Id == productId).First();
      Context.OrderedItems.Remove(orderedItem);
      Context.SaveChanges();

      return RedirectToAction("Index", "Home");
    }
    private List<Product> GetOrderedItems()
    {
      var query = from o in Context.OrderedItems
                  select o;

      return query.Select(s => s.Product).ToList();
    }

    public ActionResult Items()
    {
      return View("_Items", CacheHelper.GetOrAdd<List<Product>>(ShoppingCartCacheKey, f => GetOrderedItems(), DateTime.UtcNow.Add(expirationTime)));
    }
  }
}