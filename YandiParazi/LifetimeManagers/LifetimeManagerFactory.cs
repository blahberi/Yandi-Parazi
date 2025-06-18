using Yandi.ServiceCreation;

namespace Yandi.LifetimeManagers;

public delegate ILifetimeManager LifetimeManagerFactory(ServiceFactory serviceFactory);
