using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc.ExpressionLinks
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ActionLink<TController>
            ( this HtmlHelper helper
            , string linkText
            , Expression<Action<TController>> actionExpr
            , object htmlAttributes = null
            , object routeValues = null
            , string protocol = null
            , string hostName = null
            , string fragment = null
            )
            where TController : Controller
        {
            var parseValues = ExpressionParser.Parse(actionExpr);
            parseValues.RouteValues.AddObject(routeValues);
            return helper.ActionLink(linkText, parseValues.ActionName, parseValues.ControllerName, protocol,
                hostName, fragment, parseValues.RouteValues, new RouteValueDictionary(htmlAttributes));
        }
    }
}
