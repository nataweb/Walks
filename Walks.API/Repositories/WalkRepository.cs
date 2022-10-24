using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Models;

namespace Walks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly WalksDbContext walksDbContext;

        public WalkRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new id
            walk.Id = Guid.NewGuid();
            await walksDbContext.AddAsync(walk);
            await walksDbContext.SaveChangesAsync(); 
            return walk;    
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await walksDbContext.Walks.FindAsync(id);
            if (existingWalk == null) 
            {
                return null;
            }
            walksDbContext.Walks.Remove(existingWalk);
            await walksDbContext.SaveChangesAsync();
            return existingWalk;    
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await walksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return  walksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk = await walksDbContext.Walks.FindAsync(id);

           if (existingWalk != null) 
           {
                existingWalk.Length = walk.Length; 
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await walksDbContext.SaveChangesAsync();
                return existingWalk;    
           }
           return null;
        }
    }
}
