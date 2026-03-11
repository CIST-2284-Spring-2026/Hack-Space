using Common.Interaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Data.DALs
{
    public class BadgesDAL : IBadgesDAL
    {
        private readonly ApplicationDbContext _context;

        public BadgesDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Badge>?> GetBadgesAsync()
        {
            return await _context.Badges.ToListAsync();
        }

        public async Task<Badge?> GetBadgeByIdAsync(Guid id)
        {
            return await _context.Badges.Where(b => b.Id == id).
            FirstOrDefaultAsync();
        }

        public async Task AddBadgeAsync(Badge badge)
        {
            badge.Id = Guid.NewGuid();
            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBadgeAsync(Badge badge)
        {
            var existingBadge = await _context.Badges.Where(b => b.Id == badge.Id).FirstOrDefaultAsync();
            if (existingBadge != null)
            {
                existingBadge.Title = badge.Title;
                existingBadge.Description = badge.Description;
                existingBadge.TurnInInstructions = badge.TurnInInstructions;
                existingBadge.IsVisible = badge.IsVisible;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBadgeAsync(Guid id)
        {
            var existingBadge = await _context.Badges.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (existingBadge != null)
            {
                _context.Badges.Remove(existingBadge);
            }
        }
    }

    public interface IBadgesDAl
    {
    }
}