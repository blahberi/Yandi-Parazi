using System.Collections;

namespace Yandi;

public class ServiceCollection : IServiceCollection
{
    public ServiceCollection(IServiceCollection collection)
    {
        foreach (ServiceDescriptor service in collection)
        {
            this.Add(service);
        }
    }

    public ServiceDescriptor this[int index]
    {
        get => this.Services[index];
        set
        {
            this.ReadOnlyCheck();
            this.Services[index] = value;
        }
    }

    private List<ServiceDescriptor> Services { get; } = [];

    public int Count => this.Services.Count;

    public bool IsReadOnly { get; private set; }

    public void Add(ServiceDescriptor item)
    {
        this.Services.Add(item);
    }


    public void Clear()
    {
        this.Services.Clear();
    }

    public bool Contains(ServiceDescriptor item)
    {
        return this.Services.Contains(item);
    }

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        this.Services.CopyTo(array, arrayIndex);
    }

    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return this.Services.GetEnumerator();
    }

    public int IndexOf(ServiceDescriptor item)
    {
        return this.Services.IndexOf(item);
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        this.ReadOnlyCheck();
        this.Services.Insert(index, item);
    }

    public bool Remove(ServiceDescriptor item)
    {
        this.ReadOnlyCheck();
        return this.Services.Remove(item);
    }

    public void RemoveAt(int index)
    {
        this.ReadOnlyCheck();
        this.Services.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.Services.GetEnumerator();
    }

    private void ReadOnlyCheck()
    {
        if (this.IsReadOnly)
        {
            ThrowReadOnlyException();
        }
    }

    private static void ThrowReadOnlyException()
    {
        throw new InvalidOperationException("Can't set readonly ServiceCollection");
    }
}
