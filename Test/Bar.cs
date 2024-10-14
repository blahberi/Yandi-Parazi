using System;

namespace Test
{
    internal class Bar : IBar
    {
        IBaz baz;
        public Bar(IBaz baz) 
        {
            this.baz = baz;
            Console.WriteLine($"Bar has been initialized {this.GetHashCode()}");
        }

        public int Method(int x)
        {
            Console.WriteLine($"The Bar method was called: {GetHashCode()}");
            Console.WriteLine(this.baz.Method());
            return x * 2;
        }
    }
}
