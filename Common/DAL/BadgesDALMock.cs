using Common.Interaces;
using Common.Models;
 
namespace Common.DAL
{
    public class BadgesDALMock : IBadgesDAL
    {
        private List<Badge>? badges = new List<Badge>()
        {
            new Badge {Id = Guid.NewGuid(), Title = "Contributor", Description = "Awarded for contributing to the project.", TurnInInstructions = "Submit a pull request with your contribution." },
            new Badge {Id = Guid.NewGuid(), Title = "Early Adopter", Description = "Awarded for being an early adopter of the platform.", TurnInInstructions = "Sign up and start using the platform early." },
            new Badge {Id = Guid.NewGuid(), Title = "Community Helper", Description = "Awarded for helping other members of the community.", TurnInInstructions = "Assist other members in the community forums." }
        };
 
        public Task AddBadgeAsync(Badge badge)
        {
            badge.Id = Guid.NewGuid();
            badges?.Add(badge);
            return Task.CompletedTask;
        }
 
        public Task<Badge?> GetBadgeByIdAsync(Guid id)
        {
            var badge = badges?.Where(b => b.Id == id).FirstOrDefault();
            return Task.FromResult(badge);
        }
 
        public Task<List<Badge>?> GetBadgesAsync()
        {
            return Task.FromResult(badges);
        }
 
        public Task UpdateBadgeAsync(Badge badge)
        {
            // find the badge in the list and update it
            var existingBadge = badges?.Where(b => b.Id == badge.Id).FirstOrDefault();
            if (existingBadge != null)
            {
                existingBadge.Title = badge.Title;
                existingBadge.Description = badge.Description;
                existingBadge.TurnInInstructions = badge.TurnInInstructions;
                existingBadge.IsVisible = badge.IsVisible;
            }
            return Task.CompletedTask;
        }
    }
}