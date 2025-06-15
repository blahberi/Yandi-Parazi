using Cornflakes.Scopes;

namespace Cornflakes.Extensions;

public static class ServiceCollectionExtensions
{
    
    public static IServiceProvider BuildServiceProvider(this IServiceCollection collection)
    {
        collection
            .AddTransient<IServiceProvider>(sp => sp)
            .AddSingleton<IServiceProviderFactroy>(_ => new ServiceProviderFactory(collection))
            .AddSingleton<IScopeService, ScopeService>()
            .Finalize();
        return new ServiceProvider(collection);
    }
}