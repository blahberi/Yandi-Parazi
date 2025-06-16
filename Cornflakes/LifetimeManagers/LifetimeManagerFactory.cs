using Cornflakes.ServiceCreation;

namespace Cornflakes.LifetimeManagers;

public delegate ILifetimeManager LifetimeManagerFactory(ServiceFactory serviceFactory);
