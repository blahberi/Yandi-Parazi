using Cornflakes.LifetimeStrategies;
using System;

namespace Cornflakes
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ServiceProvider serviceProvider = new ServiceProvider();

        public IServiceProviderBuilder RegisterService<TService, TImplementation>(ILfetimeStrategy creationStrategy)
            where TImplementation : TService
        {
            this.serviceProvider.RegisterService(new ServiceDescriptor(
                typeof(TService),
                typeof(TImplementation),
                creationStrategy
            ));
            return this;
        }

        public IServiceProviderBuilder RegisterServices(IServiceCollection services)
        {
            foreach (var service in services)
            {
                this.serviceProvider.RegisterService(service);
            }
            return this;
        }

        public IServiceProvider Build()
        {
            return this.serviceProvider;
        }
    }
}
