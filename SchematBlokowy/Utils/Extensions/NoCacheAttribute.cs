using System;
using System.Web;
using System.Web.Mvc;

namespace SchematBlokowy.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class NoCacheAttribute : FilterAttribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetExpires(DateTime.Now.AddDays(-1));
            cache.AppendCacheExtension("private");
            cache.AppendCacheExtension("no-cache=Set-Cookie");
            cache.SetProxyMaxAge(TimeSpan.Zero);
            cache.SetMaxAge(TimeSpan.Zero);
            cache.AppendCacheExtension("must-revalidate, no-store");
            cache.SetValidUntilExpires(false);
            cache.SetNoStore();
        }
    }
}