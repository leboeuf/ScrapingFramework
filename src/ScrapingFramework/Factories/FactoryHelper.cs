using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScrapingFramework.Factories
{
    public class FactoryHelper : IFactoryHelper
    {
        private IServiceProvider _serviceProvider;

        public FactoryHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object[] GetDependencies(Type scraperType)
        {
            var result = new List<object>();
            var constructor = scraperType.GetConstructors().SingleOrDefault(); // Ensure only one constructor (or null, if no constructor then no dependencies)
            foreach (var parameter in constructor?.GetParameters())
            {
                result.Add(_serviceProvider.GetService(parameter.ParameterType));
            }
            return result.ToArray();
        }
    }
}
