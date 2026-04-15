using Common.DTOs;
using Common.Models;
 
namespace Common.Interfaces
{
    public interface IEvaluatorListDAL
    {
        public Task<EvaluatorListDto> GetDtoAsync();
        public Task<List<Evaluator>?> GetAllAsync();
        public Task<Evaluator?> GetByIdAsync(Guid id);
        public Task AddAsync(Evaluator evaluator);
        public Task UpdateAsync(Evaluator evaluator);
        public Task DeleteAsync(Guid id);
    }
}