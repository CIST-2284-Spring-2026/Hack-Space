using Common.Interaces;
using Common.Models;

namespace Common.DAL;
 
public class BadgesDALMock : IBadgesDAL
{
    // Local in-memory data for development/testing
    private List<Badge>? badges = new List<Badge>() 
    {
        new Badge { Title = "Contributor", Description = "Awarded for contributing to the project.", 
        TurnInInstructions = "Submit a pull request with your contribution." }, 
        new Badge { Title = "Early Adopter", Description = "Awarded for being an early adopter of the platform.", 
        TurnInInstructions = "Sign up and start using the platform early." },
        new Badge { Title = "Community Helper", Description = "Awarded for helping other members of the community.", 
        TurnInInstructions = "Assist other members in the community forums." }
    };
 
    public Task<List<Badge>?> GetBadgesAsync()
    {
        return Task.FromResult(badges);
    }

}