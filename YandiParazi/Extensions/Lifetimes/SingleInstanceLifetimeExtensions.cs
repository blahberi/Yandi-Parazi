using Yandi.Abstractions;
using Yandi.Core.LifetimeManagers;

namespace Yandi.Extensions.Lifetimes;

public static class SingleInstanceLifetimeExtensions
{
    private static readonly LifetimeManagerFactory SingleInstance = static sf => new SingleInstanceLifetime(sf);
    public static IServiceCollection AddSingleInstance<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(SingleInstance, serviceFactory);
    }

    public static IServiceCollection AddSingleInstance<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(SingleInstance);
    }

    public static IServiceCollection AddSingleInstance<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        return collection.AddTransient<TService>(sp => instance);
    }

    public static IServiceCollection AddSingleInstanceDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(SingleInstance);
    }
}
