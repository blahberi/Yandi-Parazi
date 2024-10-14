using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new TransientCreator());
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new SingletonCreation());
            return builder;
        }
        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new ScopedCreation());
            return builder;
        }
    }
}
