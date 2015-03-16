using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc.ExpressionLinks
{
    public static class ExpressionLinkExtensions
    {
        public static MvcHtmlString ActionLink<TController>(this HtmlHelper helper, string linkText, Expression<Action<TController>> actionExpr) where TController : Controller
        {
            var expr = actionExpr is LambdaExpression ? actionExpr.Body : actionExpr;
            var mce = expr as MethodCallExpression;
            if (mce == null)
                throw new ArgumentException("Expression must be a method call", /*nameof(actionExpr)*/ "actionExpr"); // TODO: C# 6 nameof

            var routeValues = GetActionParameters(mce);
            routeValues.Add("controller", GetControllerName<TController>());

            return helper.ActionLink(linkText, mce.Method.Name, routeValues);
        }

        private static RouteValueDictionary GetActionParameters(MethodCallExpression mce)
        {
            var parameterValues = mce.Arguments
                .Select(p => Expression.Lambda(p).Compile().DynamicInvoke()); // TODO: This could be slow
            var parameterNames = mce.Method.GetParameters().Select(p => p.Name);

            var valueDict = parameterNames
                .Zip(parameterValues, (n, v) => new { Name = n, Value = v })
                .ToDictionary(p => p.Name, p => p.Value);
            return new RouteValueDictionary(valueDict);
        }

        private static string GetControllerName<TController>() where TController : Controller
        {
            var controllerTypeName = typeof(TController).Name;
            if (!controllerTypeName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Controller type name must end with Controller");
            return controllerTypeName.Substring(0, controllerTypeName.Length - 10); // "Controller"
        }
    }
}
