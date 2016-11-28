﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScrapingOrchestrator
    {
        /// <summary>
        /// Start scraping.
        /// </summary>
        /// <returns></returns>
        Task Start();

        /// <summary>
        /// Register a scraper to use when a given URL is matched.
        /// </summary>
        void RegisterScraper(Type scraperClass, string baseUrl);

        /// <summary>
        /// Register a persister to use when saving scraped results of a given type.
        /// </summary>
        void RegisterPersister(Type scrapedObjectType, Type persister);

        /// <summary>
        /// Add an URL to the scraping queue.
        /// </summary>
        void AddUrlToQueue(string url);
    }
}