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
public class SomePageScraper : IScraper
{
    public Encoding WebsiteEncoding => Encoding.GetEncoding("iso-8859-1"); // You can specify the encoding

    public async Task<ScrapingResult> Scrape(ScrapingContext context)
    {
        var resultScrapedObject = new MyCustomScrapedObjectType
        {
            ScrapingContext = context,
            ScrapedUrl = context.ScrapingRequest.Url
        };
        
        var document = ScrapingHelper.Parse(context.Html);
        
        // Perform data extraction here, possibly using AngleSharp
        // [...]

        return new ScrapingResult
        {
            Url = context.ScrapingRequest.Url,
            ResultObject = resultScrapedObject,
            ResultObjectType = typeof(MyCustomScrapedObjectType)
        };
    }
}
```

### Example of Persister

```
public class MyCustomScrapedObjectTypePersister : IScrapedObjectPersister<MyCustomScrapedObjectType>
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
                cmd.CommandText = "INSERT INTO sometable (name, description, price, url) VALUES (@name, @description, @price, @url) RETURNING id";

                cmd.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("@description", NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("@price", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("@url", NpgsqlDbType.Varchar));

                PersisterHelper.SetValue(cmd, 0, scrapedObject.Name);
                PersisterHelper.SetValue(cmd, 1, scrapedObject.Description);
                PersisterHelper.SetValue(cmd, 2, scrapedObject.Price);
                PersisterHelper.SetValue(cmd, 3, scrapedObject.ScrapedUrl);

                return (int)await cmd.ExecuteScalarAsync();
            }
        }
    }
}
```

