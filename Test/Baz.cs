using Cornflakes;
using System;

namespace Test
{
    internal class Baz : IBaz
    {
        public Baz()
        {
            //Console.WriteLine($"Baz has been initialized: {this.GetHashCode()}");
        }

        public string Method()
        {
            return $"This is Baz: {this.GetHashCode()}";
        }
    }
}
