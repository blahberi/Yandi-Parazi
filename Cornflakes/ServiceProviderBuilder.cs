using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ServiceProvider serviceProvider = new ServiceProvider();

        public ServiceProviderBuilder()
        {
            this.RegisterService<IServiceProvider>(new TransientLifetime(sp => sp));
        }

        public IServiceProviderBuilder RegisterService<TService>(ILifetimeStrategy creationStrategy)
        {
            this.serviceProvider.RegisterService(new ServiceDescriptor(
                typeof(TService),
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
