using Common.Interaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Data.DALs
{
    public class BadgesDAL(ApplicationDbContext context) : IBadgesDAL
    {
        public async Task<List<Badge>?> GetBadgesAsync()
        {
            return await context.Badges.ToListAsync();
        }

        public async Task<Badge?> GetBadgeByIdAsync(Guid id)
        {
            return await context.Badges.Where(b => b.Id == id).
            FirstOrDefaultAsync();
        }

        public async Task AddBadgeAsync(Badge badge)
        {
            badge.Id = Guid.NewGuid();
            context.Badges.Add(badge);
            await context.SaveChangesAsync();
        }

        public async Task UpdateBadgeAsync(Badge badge)
        {
            var existingBadge = await context.Badges.Where(b => b.Id == badge.Id).FirstOrDefaultAsync();
            if (existingBadge != null)
            {
                existingBadge.Title = badge.Title;
                existingBadge.Description = badge.Description;
                existingBadge.TurnInInstructions = badge.TurnInInstructions;
                existingBadge.IsVisible = badge.IsVisible;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteBadgeAsync(Guid id)
        {
            var existingBadge = await context.Badges.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (existingBadge != null)
            {
                context.Badges.Remove(existingBadge);
            }
        }
    }

    public interface IBadgesDAl
    {
    }
}