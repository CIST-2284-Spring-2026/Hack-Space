using Common.Models;
 
namespace Common.Interfaces
{
    public interface IEvaluatorsDAL
    {
        public Task<List<Evaluator>?> GetAllAsync();
        public Task<Evaluator?> GetByIdAsync(Guid id);
        public Task AddAsync(Evaluator evaluator);
        public Task UpdateAsync(Evaluator evaluator);
        public Task DeleteAsync(Guid id);
    }
}