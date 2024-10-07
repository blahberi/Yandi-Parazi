using System;
using ZeTaim;

namespace Test
{
    [Prototype]
    internal class Bar : IBar
    {
        public Bar() 
        {
            Console.WriteLine("Created a bar instance");
        }

        public int Method(int x)
        {
            Console.WriteLine($"The Bar method was called from {GetHashCode()}");
            return x * 2;
        }
    }
}
