using System;

namespace ScrapingFramework.ScrapingObjects
{
    public class ScrapingResult
    {
        public string Url { get; set; }

        public Type ResultObjectType { get; set; }
        public object ResultObject { get; set; }
    }
}
