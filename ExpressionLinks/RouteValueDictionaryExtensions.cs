using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace System.Web.Mvc.ExpressionLinks
{
    internal static class RouteValueDictionaryExtensions
    {
        public static void AddObject(this RouteValueDictionary dict, object obj)
        {
            foreach (var pair in new RouteValueDictionary(obj))
                dict[pair.Key] = pair.Value;
        }
    }
}
