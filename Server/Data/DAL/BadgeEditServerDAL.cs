using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class BadgeEditServerDAL : IBadgeEditDAL
    {
        private readonly ApplicationDbContext context = default!;
 
        public BadgeEditServerDAL(ApplicationDbContext context)
        {
            this.context = context;
        }
 
        public async Task<BadgeEditDto?> GetBadgeEditDtoAsync(Guid id)
        {
            var dto = new BadgeEditDto();
            if (id == Guid.Empty)
            {
                dto.Badge = new Badge();
            }
            else
            {
                dto.Badge = await this.context
                .Badges.Where(b => b.Id == id)
                .Include(b => b.Evaluators)
                .FirstOrDefaultAsync() ?? new Badge();
            }
            dto.Evaluators = await this.context.Evaluators.ToListAsync();
            return dto;
        }
 
        public async Task AddAsync(Badge badge)
        {
            var newBadge = new Badge
            {
                Title = badge.Title,
                Description = badge.Description,
                TurnInInstructions = badge.TurnInInstructions,
                IsVisible = badge.IsVisible,
            };
 
            foreach (var evaluator in badge.Evaluators.DistinctBy(e => e.Id))
            {
                var existingEvaluator = await this.context.Evaluators
                    .FirstOrDefaultAsync(e => e.Id == evaluator.Id);
                if (existingEvaluator != null)
                {
                    newBadge.Evaluators.Add(existingEvaluator);
                }
            }
 
            this.context.Badges.Add(newBadge);
            await this.context.SaveChangesAsync();
        }
 
        public async Task UpdateAsync(Badge badge)
        {
            var existingBadge = await this.context.Badges
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
 
            foreach (var evaluator in badge.Evaluators.DistinctBy(e => e.Id))
            {
                var existingEvaluator = await this.context.Evaluators
                    .FirstOrDefaultAsync(e => e.Id == evaluator.Id);
                if (existingEvaluator != null)
                {
                    existingBadge.Evaluators.Add(existingEvaluator);
                }
            }
 
            await this.context.SaveChangesAsync();
        }
    }
}