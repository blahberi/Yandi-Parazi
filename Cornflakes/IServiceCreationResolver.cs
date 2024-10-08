using System;

namespace ZeTaim
{
    public interface IServiceCreationResolver
    {
        void RegisterServiceCreator<TAttribute, TCreator>() 
            where TAttribute : Attribute 
            where TCreator : ICreationStrategy;
        ICreationStrategy GetServiceCreator(Type implementationType);
    }
}