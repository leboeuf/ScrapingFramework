using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScrapingFramework.Factories
{
    public class ScraperFactory : IScraperFactory
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;

        private Dictionary<string, IScraper> _scrapers = new Dictionary<string, IScraper>();

        public ScraperFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
        }

        public void RegisterScraper(Type scraperType, string baseUrl)
        {
            var dependencies = GetScraperDependencies(scraperType);
            _scrapers.Add(baseUrl, (IScraper)Activator.CreateInstance(scraperType, dependencies));
        }

        public IScraper GetScraperForUrl(string url)
        {
            return _scrapers.FirstOrDefault(s => url.StartsWith(s.Key)).Value;
        }

        /// <summary>
        /// Get the dependencies for the given type by checking its constructor parameters.
        /// </summary>
        private object[] GetScraperDependencies(Type scraperType)
        {
            var result = new List<object>();
            var constructor = scraperType.GetConstructors().Single(); // Ensure only one constructor
            foreach (var parameter in constructor.GetParameters())
            {
                result.Add(_serviceProvider.GetService(parameter.ParameterType));
            }
            return result.ToArray();
        }
    }
}
