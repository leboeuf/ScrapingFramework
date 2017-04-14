using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScrapingOrchestrator
    {
        /// <remark>
        /// Temporary. TODO: find a better way to start dependant scraping tasks.
        /// </remark>
        IDownloadManager DownloadManager { get; }
        ILogger<ScrapingOrchestrator> Logger { get; }

        /// <summary>
        /// Start scraping.
        /// </summary>
        /// <returns></returns>
        Task Start();

        /// <summary>
        /// Register a persister to use when saving scraped results of a given type.
        /// </summary>
        void RegisterPersister(Type scrapedObjectType, Type persister);

        /// <summary>
        /// Add an URL to the scraping queue (with optional metadata).
        /// </summary>
        void AddUrlToQueue(string url, Dictionary<string, string> metadata = null);
    }
}
