using ScrapingFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ScrapingFramework.ScrapingObjects;
using System.Reflection;

namespace ScrapingFramework
{
    /// <summary>
    /// Setup and start scraping.
    /// </summary>
    public class ScrapingOrchestrator : IScrapingOrchestrator
    {
        private readonly ILogger<ScrapingOrchestrator> _logger;
        private readonly IDownloadManager _downloadManager;
        private readonly IScraperFactory _scraperFactory;
        private Dictionary<Type, Type> _persisters = new Dictionary<Type, Type>(); // Referenced type must implmement IScrapedObjectPersister

        // The list of URLs to scrape
        private ConcurrentQueue<ScrapingRequest> _scrapingQueue = new ConcurrentQueue<ScrapingRequest>();

        // The list of scraping tasks in progress
        private List<Task> _scrapingTasks = new List<Task>();

        /// <summary>
        /// This list of scrapers to orchestrate.
        /// </summary>
        public List<IScraper> Scrapers { get; set; }

        public IDownloadManager DownloadManager
        {
            get
            {
                return _downloadManager;
            }
        }

        public ILogger<ScrapingOrchestrator> Logger
        {
            get
            {
                return _logger;
            }
        }

        // The orchestrator will stop when nothing has happened for [_maxRetryCount * _millisecondsBetweenRetries] ms (no new task, no new item in the queue).
        private int _retryCount = 0;
        private int _maxRetryCount = 50;
        private int _millisecondsBetweenRetries = 200;

        public ScrapingOrchestrator(ILoggerFactory loggerFactory, IDownloadManager downloadManager, IScraperFactory scraperFactory)
        {
            _logger = loggerFactory.CreateLogger<ScrapingOrchestrator>();
            _downloadManager = downloadManager;
            _scraperFactory = scraperFactory;
        }

        public async Task Start()
        {
            _logger.LogInformation("Orchestrator started");

            // Start scraping objects from the queue
            while (_retryCount < _maxRetryCount)
            {
                await Task.Run(HandleQueue);
            }

            _logger.LogInformation("Orchestrator finished");
        }

        public void RegisterPersister(Type scrapedObjectType, Type persister)
        {
            _persisters.Add(scrapedObjectType, persister);
        }

        public void AddUrlToQueue(string url, Dictionary<string, string> metadata = null)
        {
            _scrapingQueue.Enqueue(new ScrapingRequest { Url = url, Metadata = metadata });
        }

        private async Task HandleQueue()
        {
            while (CanContinue())
            {
                _retryCount = 0;
                ScrapingRequest scrapingRequest;
                if (!_scrapingQueue.TryDequeue(out scrapingRequest) || scrapingRequest == null)
                {
                    // Queue is empty
                    continue;
                }

                var scraper = _scraperFactory.GetScraperForUrl(scrapingRequest.Url);
                if (scraper == null)
                {
                    // No scraper for this URL, add to list of URLs not scraped
                    _logger.LogWarning($"No scraper found for URL {scrapingRequest.Url}");
                    continue;
                }

                // Start scraping of URL
                _logger.LogInformation($"Scraping {scrapingRequest.Url}");
                var scrapingTask = new Task(async () =>
                {
                    var scrapingResult = await scraper.Scrape(new ScrapingContext
                    {
                        ScrapingOrchestrator = this,
                        ScrapingRequest = scrapingRequest,
                        Html = await _downloadManager.Download(scrapingRequest.Url, scraper.WebsiteEncoding)
                    });

                    if (scrapingResult == null)
                    {
                        // No response to handle (scraper doesn't return a ScrapingResult)
                        return;
                    }

                    if (scrapingResult.Exception != null)
                    {
                        _logger.LogError(scrapingResult.Exception.Message, scrapingResult.Exception);
                        return;
                    }

                    await SaveScrapingResult(scrapingResult);
                });

                _scrapingTasks.Add(scrapingTask);
                scrapingTask.Start();
            }

            // No more work for now, wait a bit and retry
            System.Threading.Thread.Sleep(_millisecondsBetweenRetries);
            _retryCount++;
            _logger.LogTrace($"retyCount={_retryCount}");
        }

        private bool CanContinue()
        {
            // Clean up completed tasks
            _scrapingTasks.RemoveAll(t => t.IsCompleted);

            // If nothing in queue and all tasks are completed, there's nothing more to do. Otherwise, we can continue.
            return !_scrapingQueue.IsEmpty || _scrapingTasks.Any();
        }

        private async Task SaveScrapingResult(ScrapingResult scrapingResult)
        {
            if (scrapingResult == null)
            {
                // Some scrapers are only used to start other scrapers. Those don't return results.
                return;
            }

            _logger.LogInformation($"Saving scraping results for {scrapingResult.Url}");

            // Find appropriate persister
            var persisterType = _persisters.FirstOrDefault(s => s.Key == scrapingResult.ResultObjectType).Value;
            if (persisterType == null)
            {
                // No persister for this type of scraped object
                _logger.LogWarning($"No persister found for object {scrapingResult.ResultObjectType}");
                return;
            }

            // Use reflection to invoke method with generic parameter
            var persister = Activator.CreateInstance(persisterType);
            var method = persisterType.GetMethod("Persist");
            await Task.Factory.StartNew(() => method.Invoke(persister, new[] { scrapingResult.ResultObject }));
        }
    }
}
