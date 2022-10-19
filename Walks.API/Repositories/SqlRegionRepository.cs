using Walks.API.Models;

namespace Walks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        public SqlRegionRepository(WalksDbContext)
        {

        }
        public IEnumerable<Region> GetAll()
        {
           
        }
    }
}
