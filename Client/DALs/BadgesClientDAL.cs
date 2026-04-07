using System.Net;
using System.Net.Http.Json;
using Common.Interfaces;
using Common.Models;
 
namespace Client.DALs
{
    public class BadgesClientDAL : IBadgesDAL
    {
        private readonly HttpClient httpClient;
 
        public BadgesClientDAL(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
 
        public async Task<List<Badge>?> GetAllAsync()
        {
            return await this.httpClient.GetFromJsonAsync<List<Badge>>("api/Badges");
        }
 
        public async Task<Badge?> GetByIdAsync(Guid id)
        {
            var response = await this.httpClient.GetAsync($"api/Badges/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Badge>();
        }
 
        public async Task AddAsync(Badge badge)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/Badges", badge);
            response.EnsureSuccessStatusCode();
        }
 
        public async Task UpdateAsync(Badge badge)
        {
            var response = await this.httpClient.PutAsJsonAsync("api/Badges", badge);
            response.EnsureSuccessStatusCode();
        }
 
        public async Task DeleteAsync(Guid id)
        {
            var response = await this.httpClient.DeleteAsync($"api/Badges/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

    public interface IBadgesDAL
    {
    }
}