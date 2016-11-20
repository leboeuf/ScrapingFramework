using ScrapingFramework.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScrapingFramework
{
    public class DownloadManager : IDownloadManager
    {
        private ICachingProvider _cachingProvider;
        private HttpClient _httpClient = new HttpClient();

        public DownloadManager(ICachingProvider cachingProvider)
        {
            _cachingProvider = cachingProvider;
        }

        public async Task<string> Download(string url)
        {
            if (await _cachingProvider.HasKey(url))
            {
                return await _cachingProvider.GetValue(url);
            }

            var html = await _httpClient.GetStringAsync(url);
            await _cachingProvider.CacheItem(url, html);
            return html;
        }
    }
}
