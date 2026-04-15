using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class BadgeViewServerDAL : IBadgeViewDAL
    {
        private readonly ApplicationDbContext context = default!;
 
        public BadgeViewServerDAL(ApplicationDbContext context)
        {
            this.context = context;
        }
 
        public async Task<Badge?> GetByIdAsync(Guid id)
        {
            var badge = await this.context.Badges.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (badge == null)
            {
                return null;
            }
            return badge;
        }
    }
}