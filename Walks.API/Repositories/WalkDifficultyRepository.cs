using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Models;

namespace Walks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly WalksDbContext walksDbContext;

        public WalkDifficultyRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
           walkDifficulty.Id = Guid.NewGuid();
           await walksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
           await walksDbContext.SaveChangesAsync();
           return walkDifficulty;  
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await walksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
           return await walksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await walksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty == null) 
            {
                return null;
            }
            existingWalkDifficulty.Code = walkDifficulty.Code;
            await walksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;  
        }
    }
}
