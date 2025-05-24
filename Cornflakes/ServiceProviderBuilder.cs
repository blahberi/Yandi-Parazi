using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ProviderOfServices serviceProvider = new ProviderOfServices();

        public ServiceProviderBuilder()
        {
            this.RegisterService<IProviderOfServices>(new TransientLifetime((IProviderOfServices sp, out object instance) => instance = sp));
        }

        public IServiceProviderBuilder RegisterService<TService>(ILifetimeManager creationStrategy)
        {
            this.serviceProvider.RegisterService(new ServiceDescriptor(
                typeof(TService),
                creationStrategy
            ));
            return this;
        }

        public IServiceProviderBuilder RegisterServices(IServiceCollection services)
        {
            foreach (ServiceDescriptor service in services)
            {
                this.serviceProvider.RegisterService(service);
            }
            return this;
        }

        public IProviderOfServices Build()
        {
            return this.serviceProvider;
        }
    }
}
