using System.Linq.Expressions;
using System.Reflection;

namespace Cornflakes
{
    public delegate object ServiceFactory(IProviderOfServices serviceProvider);
    public static class DefaultServiceFactory
    {
        public static ServiceFactory GetServiceFactory<TImplementation>()
        {
            return GenerateFactory(typeof(TImplementation));
        }

        private static ServiceFactory GenerateFactory(Type implementationType)
        {
            ParameterExpression serviceProviderParameter = Expression.Parameter(typeof(IProviderOfServices), "serviceProvider");
            MethodInfo? GetService = typeof(IProviderOfServices)
                .GetMethod(nameof(IProviderOfServices.GetService));

            if (GetService == null)
            {
                throw new MissingMethodException(nameof(IProviderOfServices), nameof(IProviderOfServices.GetService));
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
