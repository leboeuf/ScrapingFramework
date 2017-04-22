using System.Threading.Tasks;

namespace ScrapingFramework.Interfaces
{
    public interface IScrapedObjectPersister { }

    public interface IScrapedObjectPersister<in T> : IScrapedObjectPersister
    {
        Task<int> Persist(T scrapedObject);
    }
}
