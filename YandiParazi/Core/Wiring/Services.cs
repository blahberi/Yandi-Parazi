using System.Linq.Expressions;
using System.Reflection;
using Yandi.Abstractions;

namespace Yandi.Core.Wiring;

public static class Services
{
    public static ServiceFactory GetFactory<TImplementation>()
    {
        return CompileFactory(typeof(TImplementation));
    }

    private static ServiceFactory CompileFactory(Type implementationType)
    {
        ParameterExpression serviceProviderParameter = WiringUtils.GetServiceProviderParam();

        ConstructorInfo constructor = implementationType.GetConstructors().First();
        IEnumerable<Expression> arguments = constructor.GetParameters()
            .Select(p => WiringUtils.Resolve(p.ParameterType, serviceProviderParameter));

        Expression constructionExpression = Expression.New(constructor, arguments);

        return Expression.Lambda<ServiceFactory>(constructionExpression, serviceProviderParameter).Compile();
    }

}
