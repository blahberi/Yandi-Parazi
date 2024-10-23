using System.Linq.Expressions;
using System.Reflection;

namespace Cornflakes
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
            ParameterExpression serviceProviderParameter = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
            MethodInfo? GetService = typeof(IServiceProvider)
                .GetMethod(nameof(IServiceProvider.GetService));

            if (GetService == null)
            {
                throw new MissingMethodException(nameof(IServiceProvider), nameof(IServiceProvider.GetService));
            }

            ConstructorInfo constructor = implementationType.GetConstructors().First();
            IEnumerable<Expression> arguments = constructor.GetParameters()
                .Select(p => Expression.Convert(
                    Expression.Call(
                        serviceProviderParameter,
                        GetService,
                        Expression.Constant(p.ParameterType)), 
                    p.ParameterType));

            Expression constructionExpression = Expression.New(constructor, arguments);

            return Expression.Lambda<ServiceFactory>(constructionExpression, serviceProviderParameter).Compile();
        }
    }
}
