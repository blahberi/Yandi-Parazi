using System;
using ZeTaim.ServiceCreators;

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
