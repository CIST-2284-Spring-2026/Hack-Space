using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class BadgeEditServerDAL : IBadgeEditDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> factory = default!;
 
        // Per-user in-memory cache because this DAL should be registered as scoped
        private readonly Dictionary<Guid, BadgeEditDto> badgeEditCache = new();
 
        public BadgeEditServerDAL(IDbContextFactory<ApplicationDbContext> factory)
        {
            this.factory = factory;
        }
 
        public async Task<BadgeEditDto?> GetBadgeEditDtoAsync(Guid id)
        {
            if (this.badgeEditCache.TryGetValue(id, out BadgeEditDto? cachedDto))
            {
                return cachedDto;
            }
 
            // Wait 5 seconds to demonstrate loading state only when not cached
            await Task.Delay(5000);
 
            await using var context = await this.factory.CreateDbContextAsync();
 
            BadgeEditDto dto = new BadgeEditDto();
 
            if (id == Guid.Empty)
            {
                dto.Badge = new Badge();
            }
            else
            {
                dto.Badge = await context.Badges
                    .Where(b => b.Id == id)
                    .Include(b => b.Evaluators)
                    .FirstOrDefaultAsync() ?? new Badge();
            }
 
            dto.Evaluators = await context.Evaluators.ToListAsync();
 
            this.badgeEditCache[id] = dto;
 
            return dto;
        }
 
        public async Task AddAsync(Badge badge)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            Badge newBadge = new Badge
            {
                Title = badge.Title,
                Description = badge.Description,
                TurnInInstructions = badge.TurnInInstructions,
                IsVisible = badge.IsVisible,
            };
 
            foreach (Evaluator evaluator in badge.Evaluators.DistinctBy(e => e.Id))
            {
                Evaluator? existingEvaluator = await context.Evaluators
                    .FirstOrDefaultAsync(e => e.Id == evaluator.Id);
 
                if (existingEvaluator != null)
                {
                    newBadge.Evaluators.Add(existingEvaluator);
                }
            }
 
            context.Badges.Add(newBadge);
            await context.SaveChangesAsync();
 
            // Rebuild and cache the saved DTO using the generated ID
            BadgeEditDto dto = await this.BuildBadgeEditDtoAsync(newBadge.Id);
            this.badgeEditCache[newBadge.Id] = dto;
 
            // Clear the "new badge" cache entry so the next new form starts clean
            this.badgeEditCache.Remove(Guid.Empty);
        }
 
        public async Task UpdateAsync(Badge badge)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            Badge? existingBadge = await context.Badges
                .Include(b => b.Evaluators)
                .FirstOrDefaultAsync(b => b.Id == badge.Id);
 
            if (existingBadge == null)
            {
                return;
            }
 
            existingBadge.Title = badge.Title;
            existingBadge.Description = badge.Description;
            existingBadge.TurnInInstructions = badge.TurnInInstructions;
            existingBadge.IsVisible = badge.IsVisible;
 
            existingBadge.Evaluators.Clear();
 
            foreach (Evaluator evaluator in badge.Evaluators.DistinctBy(e => e.Id))
            {
                Evaluator? existingEvaluator = await context.Evaluators
                    .FirstOrDefaultAsync(e => e.Id == evaluator.Id);
 
                if (existingEvaluator != null)
                {
                    existingBadge.Evaluators.Add(existingEvaluator);
                }
            }
 
            await context.SaveChangesAsync();
 
            // Refresh cache with the latest saved version
            BadgeEditDto dto = await this.BuildBadgeEditDtoAsync(existingBadge.Id);
            this.badgeEditCache[existingBadge.Id] = dto;
        }
 
        private async Task<BadgeEditDto> BuildBadgeEditDtoAsync(Guid id)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            BadgeEditDto dto = new BadgeEditDto();
 
            if (id == Guid.Empty)
            {
                dto.Badge = new Badge();
            }
            else
            {
                dto.Badge = await context.Badges
                    .Where(b => b.Id == id)
                    .Include(b => b.Evaluators)
                    .FirstOrDefaultAsync() ?? new Badge();
            }
 
            dto.Evaluators = await context.Evaluators.ToListAsync();
 
            return dto;
        }
    }

    internal class BadgeEditDto
    {
    }

    public interface IBadgeEditDAL
    {
    }
}