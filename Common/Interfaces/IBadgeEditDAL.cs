using Common.DTOs;
using Common.Models;
 
namespace Common.Interfaces
{
    public interface IBadgeEditDAL
    {
        public Task<BadgeEditDto?> GetBadgeEditDtoAsync(Guid id);
        public Task AddAsync(Badge badge);
        public Task UpdateAsync(Badge badge);
    }
}