using System.Runtime.CompilerServices;

namespace Cornflakes
{
    internal class Scope : IScope
    {
        public Scope(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}