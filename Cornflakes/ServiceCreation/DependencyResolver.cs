using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Cornflakes.Extensions;

namespace Cornflakes.ServiceCreation;

public static class DependencyResolver
{
    public static ServiceFactory GetServiceFactory<TImplementation>()
    {
        return GenerateFactory(typeof(TImplementation));
    }
    
    internal static DecoratorFactory GetDecoratorFactory<TService, TImplementation>()
    {
        return GenerateDecoratorFactory(typeof(TService), typeof(TImplementation));
    }

    private static ServiceFactory GenerateFactory(Type implementationType)
    {
        ParameterExpression serviceProviderParameter = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
        
        MethodInfo? GetService = typeof(IServiceProvider)
            .GetMethod(nameof(IServiceProvider.GetService));

        if (GetService is null)
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
    
    private static DecoratorFactory GenerateDecoratorFactory(Type serviceType, Type implementationType)
    {
        ParameterExpression serviceProviderParam = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
        ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");

        MethodInfo? getService = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService));
        if (getService is null)
        {
            throw new MissingMethodException(nameof(IServiceProvider), nameof(IServiceProvider.GetService));
        }

        ConstructorInfo? ctor = implementationType
            .GetConstructors()
            .FirstOrDefault(c => c.GetParameters().Any(p => p.ParameterType == serviceType));

        if (ctor is null)
            throw new InvalidOperationException(
                $"Type '{implementationType.FullName}' must expose a constructor that takes '{serviceType.FullName}'.");

        IEnumerable<Expression> args = ctor.GetParameters().Select(p =>
            p.ParameterType == serviceType
                ? Expression.Convert(instanceParam, serviceType)
                : Expression.Convert(
                      Expression.Call(serviceProviderParam, getService, Expression.Constant(p.ParameterType)),
                      p.ParameterType));

        NewExpression newExpr = Expression.New(ctor, args);

        Expression body = Expression.Convert(newExpr, typeof(object));

        return Expression.Lambda<DecoratorFactory>(body, serviceProviderParam, instanceParam).Compile();
    }
}
