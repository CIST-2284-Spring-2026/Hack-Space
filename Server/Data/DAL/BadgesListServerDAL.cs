using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Server.Data.DALs
{
    public class BadgeListServerDAL : IBadgeListDAL
    {
        private readonly ApplicationDbContext context;
 
        public BadgeListServerDAL(ApplicationDbContext context)
        {
            this.context = context;
        }
 
        public async Task<List<Badge>?> GetAllAsync()
        {
            return await this.context.Badges.ToListAsync();
        }
 
        public async Task DeleteAsync(Guid id)
        {
            var existingBadge = this.context.Badges.Where(b => b.Id == id).FirstOrDefault();
            if (existingBadge != null)
            {
                this.context.Badges.Remove(existingBadge);
                await this.context.SaveChangesAsync();
            }
        }
    }
}