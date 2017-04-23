using ScrapingFramework.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task<string> Download(string url, Encoding websiteEncoding)
        {
            if (await _cachingProvider.HasKey(url))
            {
                return await _cachingProvider.GetValue(url);
            }

            var responseBytes = await _httpClient.GetByteArrayAsync(url);
            var html = websiteEncoding.GetString(responseBytes);

            await _cachingProvider.CacheItem(url, html);
            return html;
        }

        public async Task<string> DownloadImageToBase64(string url)
        {
            const string b64scheme = "data:image/jpeg;base64,";
            if (url.StartsWith(b64scheme))
            {
                return url.Replace(b64scheme, string.Empty);
            }

            if (await _cachingProvider.HasKey(url))
            {
                return await _cachingProvider.GetValue(url);
            }

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            var base64 = Convert.ToBase64String(responseBytes);

            await _cachingProvider.CacheItem(url, base64);
            return base64;
        }
    }
}
