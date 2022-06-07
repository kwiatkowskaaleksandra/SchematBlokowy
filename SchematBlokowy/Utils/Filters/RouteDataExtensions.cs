using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace SchematBlokowy.Utils
{
    public static class RouteDataExtensions
    {
        public static string Action(this RouteData routeData)
        {
            return routeData.Values["action"] as string;
        }

        public static string Controller(this RouteData routeData)
        {
            return routeData.Values["controller"] as string;
        }

        public static string Area(this RouteData routeData)
        {
            return routeData.DataTokens["area"] as string;
        }

        public static IDictionary<string, object> RouteValues(this RouteData routeData)
        {
            return routeData.Values
                .Where(value => !new[] { "action", "controller" }.Contains(value.Key))
                .ToDictionary(value => value.Key, value => value.Value);
        }
    }
}