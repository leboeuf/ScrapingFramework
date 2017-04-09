using System.Text;
using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IDownloadManager
    {
        /// <summary>
        /// Download an URL and return its body. Implementation may use caching via ICachingPovider.
        /// </summary>
        Task<string> Download(string url, Encoding websiteEncoding);
        Task<string> DownloadImageToBase64(string imageUrl);
    }
}
