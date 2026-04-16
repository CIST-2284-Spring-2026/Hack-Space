using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class EvaluatorListServerDAL : IEvaluatorListDAL
    {
        private readonly IDbContextFactory<ApplicationDbContext> factory;
 
        // Cached data (per user session)
        private EvaluatorListDto? cachedDto;
        private List<Evaluator>? cachedEvaluators;
 
        public EvaluatorListServerDAL(IDbContextFactory<ApplicationDbContext> factory)
        {
            this.factory = factory;
        }
 
        public async Task<EvaluatorListDto> GetDtoAsync()
        {
            if (this.cachedDto != null)
            {
                return this.cachedDto;
            }
 
            // Only delay when loading from DB
            await Task.Delay(5000);
 
            await using var context = await this.factory.CreateDbContextAsync();
 
            EvaluatorListDto dto = new EvaluatorListDto
            {
                Evaluators = await context.Evaluators
                    .Include(e => e.Badges)
                    .ToListAsync(),
 
                Badges = await context.Badges.ToListAsync()
            };
 
            this.cachedDto = dto;
            this.cachedEvaluators = dto.Evaluators;
 
            return dto;
        }
 
        public async Task<List<Evaluator>?> GetAllAsync()
        {
            if (this.cachedEvaluators != null)
            {
                return this.cachedEvaluators;
            }
 
            await using var context = await this.factory.CreateDbContextAsync();
 
            this.cachedEvaluators = await context.Evaluators
                .Include(e => e.Badges)
                .ToListAsync();
 
            return this.cachedEvaluators;
        }
 
        public async Task<Evaluator?> GetByIdAsync(Guid id)
        {
            if (this.cachedEvaluators != null)
            {
                return this.cachedEvaluators.FirstOrDefault(e => e.Id == id);
            }
 
            await using var context = await this.factory.CreateDbContextAsync();
 
            return await context.Evaluators
                .FirstOrDefaultAsync(e => e.Id == id);
        }
 
        public async Task AddAsync(Evaluator evaluator)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            evaluator.Id = Guid.NewGuid();
            context.Evaluators.Add(evaluator);
            await context.SaveChangesAsync();
 
            // Update cache
            if (this.cachedEvaluators != null)
            {
                this.cachedEvaluators.Add(evaluator);
            }
 
            this.cachedDto = null; // invalidate DTO cache
        }
 
        public async Task UpdateAsync(Evaluator evaluator)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            Evaluator? existingEvaluator = await context.Evaluators
                .FirstOrDefaultAsync(e => e.Id == evaluator.Id);
 
            if (existingEvaluator != null)
            {
                existingEvaluator.Name = evaluator.Name;
                existingEvaluator.Email = evaluator.Email;
                existingEvaluator.Background = evaluator.Background;
 
                await context.SaveChangesAsync();
 
                // Update cache
                if (this.cachedEvaluators != null)
                {
                    Evaluator? cached = this.cachedEvaluators
                        .FirstOrDefault(e => e.Id == evaluator.Id);
 
                    if (cached != null)
                    {
                        cached.Name = evaluator.Name;
                        cached.Email = evaluator.Email;
                        cached.Background = evaluator.Background;
                    }
                }
 
                this.cachedDto = null; // invalidate DTO cache
            }
        }
 
        public async Task DeleteAsync(Guid id)
        {
            await using var context = await this.factory.CreateDbContextAsync();
 
            Evaluator? existingEvaluator = await context.Evaluators
                .FirstOrDefaultAsync(e => e.Id == id);
 
            if (existingEvaluator != null)
            {
                context.Evaluators.Remove(existingEvaluator);
                await context.SaveChangesAsync();
 
                // Update cache
                if (this.cachedEvaluators != null)
                {
                    this.cachedEvaluators = this.cachedEvaluators
                        .Where(e => e.Id != id)
                        .ToList();
                }
 
                this.cachedDto = null; // invalidate DTO cache
            }
        }
 
        // Optional helpers
 
        public void ClearCache()
        {
            this.cachedDto = null;
            this.cachedEvaluators = null;
        }
    }

    internal class EvaluatorListDto
    {
    }

    public interface IEvaluatorListDAL
    {
    }
}