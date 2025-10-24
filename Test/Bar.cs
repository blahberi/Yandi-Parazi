namespace Test;

public class Bar : IBar
{
    public void BarMethod()
    {
        Console.WriteLine($"I am the Bar {this.GetHashCode()}");
    }
}
