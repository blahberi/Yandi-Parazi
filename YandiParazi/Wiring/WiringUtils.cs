using System.Linq.Expressions;
using System.Reflection;

namespace Yandi.Wiring;

internal static class WiringUtils
{
    private static readonly MethodInfo GetServiceMethod = typeof(IServiceProvider)
            .GetMethod(nameof(IServiceProvider.GetService))
            ?? throw new MissingMethodException(
                nameof(IServiceProvider),
                nameof(IServiceProvider.GetService));


    internal static ParameterExpression GetServiceProviderParam()
    {
        return Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
    }

    internal static Expression Resolve(Type serviceType,
        ParameterExpression serviceProviderParam)
    {
        return Expression.Convert(
            Expression.Call(
                serviceProviderParam,
                GetServiceMethod,
                Expression.Constant(serviceType)),
            serviceType);
    }
}
