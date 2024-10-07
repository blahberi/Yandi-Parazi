using System;

namespace ZeTaim
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ServiceCreationResolver serviceCreationResolver;
        private readonly ServiceProvider serviceProvider;

        public ServiceProviderBuilder()
        {
            this.serviceCreationResolver = new ServiceCreationResolver();
            this.serviceProvider = new ServiceProvider(serviceCreationResolver);

            this.RegisterServiceCreator<PrototypeAttribute, PrototypeCreation>();
            this.RegisterServiceCreator<SingletonAttribute, SingletonCreation>();
        }

        public IServiceProviderBuilder RegisterService<TService, TImplementation>() where TImplementation : TService
        {
            this.serviceProvider.RegisterService(new ServiceDescriptor(
                typeof(TService),
                typeof(TImplementation)
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

        public IServiceProviderBuilder RegisterServiceCreator<TAttribute, TCreator>()
            where TAttribute : Attribute 
            where TCreator : ICreationStrategy
        {
            this.serviceCreationResolver.RegisterServiceCreator<TAttribute, TCreator>();
            return this;
        }

        public IServiceProvider Build()
        {
            return this.serviceProvider;
        }
    }
}
