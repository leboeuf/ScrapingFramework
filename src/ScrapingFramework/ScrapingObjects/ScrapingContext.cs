using ScrapingFramework.Interfaces;

namespace ScrapingFramework.ScrapingObjects
{
    public class ScrapingContext
    {
        public ScrapingRequest ScrapingRequest { get; set; }
        public string Html { get; set; }
        public IScrapingOrchestrator ScrapingOrchestrator { get; set; }
    }
}
