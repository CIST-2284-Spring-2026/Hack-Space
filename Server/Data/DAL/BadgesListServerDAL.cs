using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class BadgeListServerDAL : IBadgeListDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> factory = default!;
 
        // Cached list for this user session (scoped)
        private List<Badge>? cachedBadges;
 
        public BadgeListServerDAL(IDbContextFactory<ApplicationDbContext> factory)
        {
            this.factory = factory;
        }
 
        public async Task<List<Badge>?> GetAllAsync()
        {
            if (this.cachedBadges != null)
            {
                return this.cachedBadges;
            }
 
            // Only delay when actually loading from DB
            await Task.Delay(5000);
 
            await using var context = await this.factory.CreateDbContextAsync();
 
            this.cachedBadges = await context.Badges.ToListAsync();
 
            return this.cachedBadges;
        }
 
        public async Task DeleteAsync(Guid id)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            Badge? existingBadge = await context.Badges
                .FirstOrDefaultAsync(b => b.Id == id);
 
            if (existingBadge != null)
            {
                context.Badges.Remove(existingBadge);
                await context.SaveChangesAsync();
 
                // Update cache
                if (this.cachedBadges != null)
                {
                    this.cachedBadges = this.cachedBadges
                        .Where(b => b.Id != id)
                        .ToList();
                }
            }
        }
 
        // Optional: force refresh if needed
        public async Task<List<Badge>?> RefreshAsync()
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            this.cachedBadges = await context.Badges.ToListAsync();
 
            return this.cachedBadges;
        }
 
        // Optional: clear cache manually
        public void ClearCache()
        {
            this.cachedBadges = null;
        }
    }

    public interface IBadgeListDAL
    {
    }
}