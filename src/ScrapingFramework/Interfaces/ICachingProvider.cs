using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface ICachingProvider
    {
        /// <summary>
        /// Store an item to the cache.
        /// </summary>
        /// <param name="key">The key used to identify this item.</param>
        /// <param name="value">The value to store at the given key.</param>
        Task CacheItem(string key, string value);

        /// <summary>
        /// Delete the key and its stored value from the cache. 
        /// </summary>
        /// <param name="key">The key to remove.</param>
        Task RemoveItem(string key);

        /// <summary>
        /// Return whether the given key is present in the cache.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <returns>Whether the given key exists.</returns>
        Task<bool> HasKey(string key);

        /// <summary>
        /// Get the value stored for the given key.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <returns>The value stored at the key. Returns null if the key was not found.</returns>
        Task<string> GetValue(string key);
    }
}
