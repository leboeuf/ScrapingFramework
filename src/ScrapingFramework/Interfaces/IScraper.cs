using ScrapingFramework.ScrapingObjects;
using System.Text;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScraper
    {
        Encoding WebsiteEncoding { get; }

        Task<ScrapingResult> Scrape(ScrapingContext context);
    }
}
