using Common.Models;
 
namespace Common.Interfaces
{
    public interface IBadgeViewDAL
    {
        public Task<Badge?> GetByIdAsync(Guid id);
    }
}