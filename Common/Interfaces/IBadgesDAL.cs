using Common.Models;
 
namespace Common.Interaces
{
    public interface IBadgesDAL
    {
        public Task<List<Badge>?> GetBadgesAsync();
        public Task AddBadgeAsync(Badge badge);
        public Task<Badge?> GetBadgeByIdAsync(Guid id);
        public Task UpdateBadgeAsync(Badge badge);
    }
}