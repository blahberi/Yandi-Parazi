namespace Cornflakes.CreationStrategies
{
    internal class TransientCreator : ICreationStrategy 
    {
        private readonly ServiceFactory serviceFactory;
        public TransientCreator(ServiceFactory serviceFactory) 
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            return this.serviceFactory(serviceProvider);
        }
    }
}
