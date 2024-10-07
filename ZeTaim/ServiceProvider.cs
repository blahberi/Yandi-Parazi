using System;
using System.Collections.Generic;

namespace ZeTaim
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly IServiceCreationResolver creationResolver;
        private readonly Dictionary<Type, ServiceDescriptor> descriptors = new Dictionary<Type, ServiceDescriptor>();
        private readonly Dictionary<Type, ICreationStrategy> creators = new Dictionary<Type, ICreationStrategy>();

        public ServiceProvider(IServiceCreationResolver creationResolver)
        {
            this.creationResolver = creationResolver;
        }

        public void RegisterService(ServiceDescriptor descriptor)
        {
            this.descriptors[descriptor.ServiceType] = descriptor;
            this.creators[descriptor.ServiceType] = this.creationResolver.GetServiceCreator(descriptor.ServiceImplementation);

        }

        public TService GetService<TService>()
        {
            Type implementationType = this.descriptors[typeof(TService)].ServiceImplementation;
            return (TService)this.creators[typeof(TService)].GetInstance(implementationType, this);
        }
    }
}
