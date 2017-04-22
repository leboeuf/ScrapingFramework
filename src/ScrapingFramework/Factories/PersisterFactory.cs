using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace ScrapingFramework.Factories
{
    public class PersisterFactory : IPersisterFactory
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private IFactoryHelper _factoryHelper;

        private Dictionary<Type, IScrapedObjectPersister> _persisters = new Dictionary<Type, IScrapedObjectPersister>();

        public PersisterFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IFactoryHelper factoryHelper)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _factoryHelper = factoryHelper;
        }

        public void RegisterPersister<TScrapedObjectType>(Type persisterType)
        {
            var dependencies = _factoryHelper.GetDependencies(persisterType);
            var persisterInstance = (IScrapedObjectPersister<TScrapedObjectType>)Activator.CreateInstance(persisterType, dependencies);

            _persisters.Add(typeof(TScrapedObjectType), persisterInstance);
        }

        public IScrapedObjectPersister<T> GetPersister<T>()
        {
            return (IScrapedObjectPersister<T>) _persisters[typeof(T)];
        }

        public IScrapedObjectPersister GetPersister(Type objectType)
        {
            return _persisters[objectType];
        }
    }
}
