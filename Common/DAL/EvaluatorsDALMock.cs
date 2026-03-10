using Common.Interaces;
using Common.Interfaces;
using Common.Models;
 
namespace Common.DAL
{
    public class EvaluatorsDALMock : IEvaluatorsDAL
    {
        private List<Evaluator>? evaluators = new List<Evaluator>()
        {
            new Evaluator {Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com", Background = "Admin" },
            new Evaluator {Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane.smith@example.com", Background = "Moderator" },
            new Evaluator {Id = Guid.NewGuid(), Name = "Bob Johnson", Email = "bob.johnson@example.com", Background = "Reviewer" }
        };

        public Task<List<Evaluator>?> GetAllAsync()
        {
            return Task.FromResult(evaluators);
        }
 
        public Task<Evaluator?> GetByIdAsync(Guid id)
        {
            var evaluator = evaluators?.Where(e => e.Id == id).FirstOrDefault();
            return Task.FromResult(evaluator);
        }
 
        public Task AddAsync(Evaluator evaluator)
        {
            evaluator.Id = Guid.NewGuid();
            evaluators?.Add(evaluator);
            return Task.CompletedTask;
        }
 
        public Task UpdateAsync(Evaluator evaluator)
        {
            // find the evaluator in the list and update it
            var existingEvaluator = evaluators?.Where(e => e.Id == evaluator.Id).FirstOrDefault();
            if (existingEvaluator != null)
            {
                existingEvaluator.Name = evaluator.Name;
                existingEvaluator.Email = evaluator.Email;
                existingEvaluator.Background = evaluator.Background;
            }
            return Task.CompletedTask;
        }
 
        public Task DeleteAsync(Guid id)
        {
            // find the evaluator in the list and update it
            var existingEvaluator = evaluators?.Where(e => e.Id == id).FirstOrDefault();
            if (existingEvaluator != null)
            {
                evaluators?.Remove(existingEvaluator);
            }
            return Task.CompletedTask;
        }
    }
}

