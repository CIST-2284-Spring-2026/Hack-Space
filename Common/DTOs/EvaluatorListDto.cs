using Common.Models;
 
namespace Common.DTOs
{
    public class EvaluatorListDto
    {
        public List<Evaluator> Evaluators { get; set; } = new List<Evaluator>();
        public List<Badge> Badges { get; set; } = new List<Badge>();
    }
}