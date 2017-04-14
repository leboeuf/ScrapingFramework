using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace ScrapingFramework.Factories
{
    public class ScraperFactory : IScraperFactory
    {
        private Dictionary<string, IScraper> _scrapers = new Dictionary<string, IScraper>();

        public void RegisterScraper(Type scraperType, string baseUrl)
        {
            _scrapers.Add(baseUrl, (IScraper)Activator.CreateInstance(scraperType));
        }

        public IScraper GetScraperForUrl(string url)
        {
            return null;
        }
    }
}
