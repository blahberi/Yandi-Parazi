using System;
using ZeTaim;
namespace Test
{
    [Singleton]
    internal class Foo : IFoo
    {
        private readonly IBar bar;
        public Foo(IBar bar)
        {
            this.bar = bar;
            Console.WriteLine("Foo has been initialized");
        }

        public void Method()
        {
            int res = this.bar.Method(5);
            Console.WriteLine($"Foo calls bar method was called from instance {GetHashCode()} which returns {res}");
        }
    }
}
