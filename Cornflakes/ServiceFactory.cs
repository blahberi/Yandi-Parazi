using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cornflakes.LifetimeStrategies
{
    public delegate object ServiceFactory(IServiceProvider serviceProvider);
    public static class DefaultServiceFactory
    {
        public static ServiceFactory GetServiceFactory<TImplementation>()
        {
            return GenerateFactory(typeof(TImplementation));
        }

        private static ServiceFactory GenerateFactory(Type implementationType)
        {
            ParameterExpression serviceProviderParameter = Expression.Parameter(typeof(IServiceProvider));

            ConstructorInfo constructor = implementationType.GetConstructors().First();
            IEnumerable<UnaryExpression> arguments = constructor.GetParameters()
                .Select(p => Expression.Convert(
                    Expression.Call(
                        serviceProviderParameter,
                        typeof(IServiceProvider)
                        .GetMethod(nameof(IServiceProvider.GetService)),
                        Expression.Constant(p.ParameterType)),
                    p.ParameterType));

            NewExpression constructionExpression = Expression.New(constructor, arguments);

            return Expression.Lambda<ServiceFactory>(constructionExpression, serviceProviderParameter).Compile();
        }
    }
}
