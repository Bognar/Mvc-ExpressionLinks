using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionLinks
{
    public static class UrlHelperExtensions
    {
        public static string Action<TController>
            ( this UrlHelper helper
            , Expression<Action<TController>> actionExpr
            , object routeValues = null
            , string protocol = null
            , string hostname = null
            )
            where TController : Controller
        {
            var parseValues = ExpressionParser.Parse(actionExpr);
            parseValues.RouteValues.AddObject(routeValues);
            return helper.Action(parseValues.ActionName, parseValues.ControllerName, parseValues.RouteValues, protocol, hostname);
        }
    }
}
