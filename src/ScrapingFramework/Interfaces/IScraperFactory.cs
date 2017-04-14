using System;

namespace ScrapingFramework.Interfaces
{
    public interface IScraperFactory
    {
        /// <summary>
        /// Registers a scraper that can parse a given base URL.
        /// </summary>
        void RegisterScraper(Type scraperType, string baseUrl);

        /// <summary>
        /// Returns the scraper instance able to parse the given URL, null if no scraper exists.
        /// </summary>
        IScraper GetScraperForUrl(string url);
    }
}
