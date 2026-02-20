using Common.Models;
 
namespace Common.Interaces
{
    public interface IBadgesDAL
    {
        public Task<List<Badge>?> GetBadgesAsync();
    }
}