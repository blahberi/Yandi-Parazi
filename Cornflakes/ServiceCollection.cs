using Cornflakes.LifetimeStrategies;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cornflakes
{
    public class ServiceCollection : IServiceCollection
    {
        public ServiceDescriptor this[int index] 
        { 
            get => Services[index];
            set
            {
                if (IsReadOnly)
                {
                    ThrowReadOnlyException();
                }
                Services[index] = value;
            }
        }

        public List<ServiceDescriptor> Services { get; } = new List<ServiceDescriptor>();

        public IServiceCollection AddService<TService>(ILifetimeStrategy creationStrategy)
        {
            this.Add(new ServiceDescriptor(
                typeof(TService), 
                creationStrategy
            ));

            return this;
        }

        public int Count => Services.Count;

        public bool IsReadOnly { get; set; }

        public void Add(ServiceDescriptor item)
        {
            Services.Add(item);
        }


        public void Clear()
        {
            Services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return Services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Services.CopyTo(array, arrayIndex);
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