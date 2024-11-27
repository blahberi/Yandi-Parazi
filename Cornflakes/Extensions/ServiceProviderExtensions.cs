﻿namespace Cornflakes
{
    public static class ServiceProviderExtensions
    {
        public static TService GetService<TService>(this IProviderOfServices serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }
    }
}