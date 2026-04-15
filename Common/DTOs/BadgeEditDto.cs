using Common.Models;
 
namespace Common.DTOs
{
    public class BadgeEditDto
    {
        public Badge Badge { get; set; } = new Badge();
        public List<Evaluator> Evaluators { get; set; } = new List<Evaluator>();
    }
}