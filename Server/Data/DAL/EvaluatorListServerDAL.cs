using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class EvaluatorListServerDAL : IEvaluatorListDAL
    {
        private readonly ApplicationDbContext context;
 
        public EvaluatorListServerDAL(ApplicationDbContext context)
        {
            this.context = context;
        }
 
        public async Task<EvaluatorListDto> GetDtoAsync()
        {
            EvaluatorListDto dto = new EvaluatorListDto();
            dto.Evaluators = await this.context.Evaluators.Include(e => e.Badges).ToListAsync();
            dto.Badges = await this.context.Badges.ToListAsync();
            return dto;
        }
 
        public async Task<List<Evaluator>?> GetAllAsync()
        {
            return await this.context.Evaluators.Include(e => e.Badges).ToListAsync();
        }
 
        public async Task<Evaluator?> GetByIdAsync(Guid id)
        {
            return await this.context.Evaluators.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
 
        public async Task AddAsync(Evaluator evaluator)
        {
            evaluator.Id = Guid.NewGuid();
            this.context.Evaluators.Add(evaluator);
            await this.context.SaveChangesAsync();
        }
 
        public async Task UpdateAsync(Evaluator evaluator)
        {
            var existingEvaluator = await this.context.Evaluators.Where(e => e.Id == evaluator.Id).FirstOrDefaultAsync();
            if (existingEvaluator != null)
            {
                existingEvaluator.Name = evaluator.Name;
                existingEvaluator.Email = evaluator.Email;
                existingEvaluator.Background = evaluator.Background;
                await this.context.SaveChangesAsync();
            }
        }
 
        public async Task DeleteAsync(Guid id)
        {
            var existingEvaluator = await this.context.Evaluators.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (existingEvaluator != null)
            {
                this.context.Evaluators.Remove(existingEvaluator);
                await this.context.SaveChangesAsync();
            }
        }
    }
}