using Common.Interaces;
using Common.Models;
using Microsoft.AspNetCore.Components;


namespace Server.Data.DALS
{
public class BadgesDAL: IBadgesDAL
{
        
    private object? context;

        public async Task<List<Badge>?> GetAllAsync()
{
return await this.context.badges.ToListAsync();
}

public async Task<Badge?>GetByIdAsync(Guid id)
{

return await this.context.Badges. Include (b => b.Evaluators). Where (b => b.Id == id). FirstOrDefaultAsync();
}

public async Task AddAsync (Badge badge)
{
this.context.Badges.Add(badge);
}

        public Task<List<Badge>?> GetBadgesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Badge?> GetBadgeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task AddBadgeAsync(Badge badge)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBadgeAsync(Badge badge)
        {
            throw new NotImplementedException();
        }
    }
} 