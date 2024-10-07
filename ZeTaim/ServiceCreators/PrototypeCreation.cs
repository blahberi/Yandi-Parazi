using System;

namespace ZeTaim
{
    internal class PrototypeCreation : BaseCreationStrategy
    {
        public override object GetInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            return this.CreateInstance(implementationType, serviceProvider);
        }
    }
}
