using System;

namespace Cornflakes
{
    internal class TransientCreator : BaseCreationStrategy
    {
        public override object GetInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            return this.CreateInstance(implementationType, serviceProvider);
        }
    }
}
