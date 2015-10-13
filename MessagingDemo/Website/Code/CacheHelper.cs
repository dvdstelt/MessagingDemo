using Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Website.Code
{
  public interface ICacheHelper
  {
    void Insert<T>(string key, T item, DateTime expirationUtc);
    void Flush();
    T GetOrAdd<T>(string key, Func<string, T> creator, DateTime expirationUtc);
    T Get<T>(string key);
  }

  public class CacheHelper : ICacheHelper
  {
    private const string VisitorFormCacheKeyFormat = "visitorform_";  // Important for ignoring these items when flushing cache.

    private static readonly Cache Cache = HttpContext.Current.Cache;
    private static readonly ILog Log = LogManager.GetLogger<CacheHelper>();

    public void Flush()
    {
      foreach (DictionaryEntry item in Cache)
      {
        if (item.Key.ToString().StartsWith(VisitorFormCacheKeyFormat))
          continue;

        Log.TraceFormat("Removing item from cache with key : {0}", item.Key.ToString());
        Cache.Remove(item.Key.ToString());
      }
    }

    public void Insert<T>(string key, T item, DateTime expirationUtc)
    {
      Cache.Insert(key, item, null, expirationUtc, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, _removed);
    }

    private static readonly object nullObject = new object();

    public T GetOrAdd<T>(string key, Func<string, T> creator, DateTime expirationUtc)
    {
      T item = Get<T>(key);
      var isCached = item != null;

      if (!isCached)
      {
        var start = Stopwatch.StartNew();
        item = creator(key);
        if (item != null)
        {
          Log.TraceFormat("Creator '{0}' duration: {1}ms", key, start.ElapsedMilliseconds);
          Insert(key, (object)item ?? nullObject, expirationUtc);
        }
      }

      return item;
    }

    public T Get<T>(string key)
    {
      var item = (T)Cache[key];

      if (item == null)
        Log.TraceFormat("Cache miss: {0}", key);
      else
        Log.TraceFormat("Cache hit: {0}", key);

      if (ReferenceEquals(nullObject, item)) return default(T);

      return item;
    }

    static void _removed(string key, object value, CacheItemRemovedReason reason)
    {
      Log.TraceFormat("Item '{0}' removed from cache. Reason: {1}", key, reason);
    }
  }
}