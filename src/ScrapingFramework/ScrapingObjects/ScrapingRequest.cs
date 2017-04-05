using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapingFramework.ScrapingObjects
{
    public class ScrapingRequest
    {
        /// <summary>
        /// Url to scrape.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Can be used to pass information from the caller to the scraper of the URL.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
