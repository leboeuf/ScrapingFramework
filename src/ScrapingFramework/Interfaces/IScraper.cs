using ScrapingFramework.ScrapingObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScraper
    {
        Task<ScrapingResult> Scrape(ScrapingContext context);
    }
}
