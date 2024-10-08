using System;

namespace ZeTaim
{
    internal class SingletonCreation : BaseCreationStrategy
    {
        private object instance;

        public override object GetInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            if (this.instance == null)
            {
                this.instance = this.CreateInstance(implementationType, serviceProvider);
            }
            return this.instance;
        }
    }
}
