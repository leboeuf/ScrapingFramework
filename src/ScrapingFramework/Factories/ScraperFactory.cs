using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrapingFramework.Factories
{
    public class ScraperFactory : IScraperFactory
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private IFactoryHelper _factoryHelper;

        private Dictionary<string, IScraper> _scrapers = new Dictionary<string, IScraper>();

        public ScraperFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IFactoryHelper factoryHelper)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _factoryHelper = factoryHelper;
        }

        public void RegisterScraper(Type scraperType, string baseUrl)
        {
            var dependencies = _factoryHelper.GetDependencies(scraperType);
            _scrapers.Add(baseUrl, (IScraper)Activator.CreateInstance(scraperType, dependencies));
        }

        public IScraper GetScraperForUrl(string url)
        {
            return _scrapers.FirstOrDefault(s => url.StartsWith(s.Key)).Value;
        }
    }
}
