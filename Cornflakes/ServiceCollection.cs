using Cornflakes.LifetimeManagers;
using System.Collections;

namespace Cornflakes
{
    public class ServiceCollection : IServiceCollection
    {
        public ServiceDescriptor this[int index] 
        { 
            get => this.Services[index];
            set
            {
                if (this.IsReadOnly)
                {
                    this.ThrowReadOnlyException();
                }

                this.Services[index] = value;
            }
        }

        private List<ServiceDescriptor> Services { get; } = [];

        public IServiceCollection AddService<TService>(ILifetimeManager lifetimeManager)
        {
            this.Add(new ServiceDescriptor(
                typeof(TService), 
                lifetimeManager
            ));

            return this;
        }

        public int Count => this.Services.Count;

        public bool IsReadOnly { get; set; }

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
            return Services.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return Services.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            if (IsReadOnly)
            {
                ThrowReadOnlyException();
            }
            Services.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            if (IsReadOnly)
            {
                ThrowReadOnlyException();
            }
            return Services.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
            {
                ThrowReadOnlyException();
            }
            Services.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Services.GetEnumerator();
        }

        private void ThrowReadOnlyException()
        {
            throw new InvalidOperationException("Can't set readonly ServiceCollection");
        }
    }
}