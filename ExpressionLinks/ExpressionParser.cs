using System.Linq;
using System.Linq.Expressions;
using System.Web.Routing;

namespace System.Web.Mvc.ExpressionLinks
{
    internal static class ExpressionParser
    {
        public static ParsedValues Parse<TController>(Expression<Action<TController>> expr) where TController : Controller
        {
            var mce = expr.Body as MethodCallExpression;
            if (mce == null)
                throw new ArgumentException("Expression must be a method call", /*nameof(expr)*/ "expr"); // TODO: C# 6 nameof

            return new ParsedValues
            {
                ActionName = mce.Method.Name,
                ControllerName = GetControllerName<TController>(),
                RouteValues = GetActionParameters(mce)
            };
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

    public class ParsedValues
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}