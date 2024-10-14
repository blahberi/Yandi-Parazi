using System;

namespace Cornflakes.CreationStrategies
{
    internal class SingletonCreation : ICreationStrategy
    {
        private object instance;
        private readonly ServiceFactory serviceFactory;

        public SingletonCreation(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            if (this.instance == null)
            {
                this.instance = this.serviceFactory(serviceProvider);
            }
            return this.instance;
        }
    }
}
