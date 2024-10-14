using System;
using Cornflakes;
namespace Test
{
    internal class Foo : IFoo
    {
        private readonly IBar bar;
        public Foo(IBar bar)
        {
            this.bar = bar;
            Console.WriteLine($"Foo has been initialized: {this.GetHashCode()}");
        }

        public void Method()
        {
            int res = this.bar.Method(5);
            Console.WriteLine($"Foo calls bar which returns {res}: {this.GetHashCode()}");
        }
    }
}
