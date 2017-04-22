using System;

namespace ScrapingFramework.Interfaces
{
    public interface IPersisterFactory
    {
        /// <summary>
        /// Register a persister to use when saving scraped results of a given type.
        /// </summary>
        void RegisterPersister<T>(Type persisterType);

        /// <summary>
        /// Returns the persister instance able to persist the given object type, null if no persister exists.
        /// </summary>
        IScrapedObjectPersister<T> GetPersister<T>();
        IScrapedObjectPersister GetPersister(Type objectType);
    }
}
