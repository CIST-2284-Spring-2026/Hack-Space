using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluatorsController(IEvaluatorsDAL dal) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var evaluators = await dal.GetAllAsync();
            return Ok(evaluators);
        }
 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var evaluator = await dal.GetByIdAsync(id);
            if (evaluator == null)
            {
                return NotFound();
            }
 
            return Ok(evaluator);
        }
 
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Evaluator evaluator)
        {
            await dal.AddAsync(evaluator);
            return Ok();
        }
 
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Evaluator evaluator)
        {
            await dal.UpdateAsync(evaluator);
            return Ok();
        }
 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await dal.DeleteAsync(id);
            return Ok();
        }
    }

    public interface IEvaluatorsDAL
    {
        Task<IEnumerable<Evaluator>> GetAllAsync();
        Task<Evaluator> GetByIdAsync(Guid id);
        Task AddAsync(Evaluator evaluator);
        Task UpdateAsync(Evaluator evaluator);
        Task DeleteAsync(Guid id);
    }
}