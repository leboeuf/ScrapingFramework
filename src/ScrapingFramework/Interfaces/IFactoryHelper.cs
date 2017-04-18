using System;

namespace ScrapingFramework.Interfaces
{
    public interface IFactoryHelper
    {
        /// <summary>
        /// Get the dependencies for the given type by checking its constructor parameters.
        /// </summary>
        object[] GetDependencies(Type scraperType);
    }
}
