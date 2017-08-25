# ScrapingFramework

## Example usage

### In Program.cs or another entry point

```
// Register scrapers and persisters
_scraperFactory.RegisterScraper(typeof(Scrapers.SomeDomain.SomePageScraper), "http://example.com/");
_persisterFactory.RegisterPersister<MyCustomScrapedObjectType>(typeof(MyCustomScrapedObjectTypePersister));

// Add URLs to queue
_scrapingOrchestrator.AddUrlToQueue("http://example.com/a-specific-page.html");
_scrapingOrchestrator.AddUrlToQueue("http://example.com/another-page.html");

// Start scraping!
await _scrapingOrchestrator.Start();
```

### Example of ScrapedObject

```
public class MyCustomScrapedObjectType
{
    public ScrapingContext ScrapingContext { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string ScrapedUrl { get; set; }
}
```

### Example of Scraper

```
```


### Example of Persister

```
public class EventPersister : IScrapedObjectPersister<MyCustomScrapedObjectType>
{
    public async Task<int> Persist(MyCustomScrapedObjectType scrapedObject)
    {
        var object_id = await PersistToPostgres(scrapedObject);
        return object_id;
    }

    private async Task<int> PersistToPostgres(MyCustomScrapedObjectType scrapedObject)
    {
        // Note: prepared statements are reused throughout connections, see: http://www.npgsql.org/doc/prepare.html#persistency
        using (var conn = new NpgsqlConnection(EnvironmentConfiguration.DatabaseConnectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "";

                // ...

                return (int)await cmd.ExecuteScalarAsync();
            }
        }
    }
}
```

