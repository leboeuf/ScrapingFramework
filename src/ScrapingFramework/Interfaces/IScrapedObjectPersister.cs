using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScrapedObjectPersister<in T>
    {
        Task Persist(T scrapedObject);
    }
}
