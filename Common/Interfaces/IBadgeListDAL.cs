using Common.Models;
 
namespace Common.Interfaces
{
    public interface IBadgeListDAL
    {
        public Task<List<Badge>?> GetAllAsync();
        public Task DeleteAsync(Guid id);
    }
}