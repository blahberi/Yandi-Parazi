using System.Linq.Expressions;
using System.Reflection;
using Yandi.Abstractions;

namespace Yandi.Core.Wiring;

public static class Decorators
{
    internal static DecoratorFactory GetFactory<TService, TImplementation>()
    {
        return CompileFactory(typeof(TService), typeof(TImplementation));
    }

    private static DecoratorFactory CompileFactory(Type serviceType, Type implementationType)
    {
        ParameterExpression serviceProviderParam = WiringUtils.GetServiceProviderParam();
        ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");

        ConstructorInfo constructor = implementationType
            .GetConstructors()
            .First(c => c.GetParameters().Any(p => p.ParameterType == serviceType));

        IEnumerable<Expression> args = constructor.GetParameters().Select(p =>
            p.ParameterType == serviceType
                ? Expression.Convert(instanceParam, serviceType)
                : WiringUtils.Resolve(p.ParameterType, serviceProviderParam));

        NewExpression newExpr = Expression.New(constructor, args);
        Expression body = Expression.Convert(newExpr, typeof(object));
        return Expression.Lambda<DecoratorFactory>(body, instanceParam, serviceProviderParam).Compile();
    }
}
