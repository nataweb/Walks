using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Models;

namespace Walks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly WalksDbContext walksDbContext;

        public RegionRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {   
            region.Id = Guid.NewGuid();
            await walksDbContext.AddAsync(region);
            await walksDbContext.SaveChangesAsync(); 
            return region;  
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await walksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null) 
            {
                return null;
            }
            //Delete the region
            walksDbContext.Regions.Remove(region);
            await walksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
           return  await walksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
          return await walksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var existingRegion = await walksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
           if (region == null) 
           {
                return null;
           }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await walksDbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
