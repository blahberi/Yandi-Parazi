using Cornflakes.Extensions;
using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ServiceCollection services = new ServiceCollection();

        public ServiceProviderBuilder()
        {
            this.services
                .AddTransient<IServiceProvider>(sp => sp)
                .AddTransient<IScopeFactory, ScopeFactory>()
                .AddSingleton<IScope, GlobalScope>();
        }

        public IServiceProviderBuilder RegisterService<TService>(ILifetimeManager lifetimeManager)
        {
            this.services.AddService<TService>(lifetimeManager);
            return this;
        }

        public IServiceProviderBuilder RegisterServices(IServiceCollection services)
        {
            foreach (ServiceDescriptor service in services)
            {
                this.services.Add(service);
            }
            return this;
        }

        public IServiceProvider Build()
        {
            return new ServiceProvider(this.services);
        }
    }
}
