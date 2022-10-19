using Walks.API.Models;

namespace Walks.API.Repositories
{
    public interface IRegionRepository
    {
        IEnumerable<Region> GetAll();
    }
}
 