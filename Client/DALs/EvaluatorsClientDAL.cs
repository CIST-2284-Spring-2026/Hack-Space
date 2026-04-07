using System.Net;
using System.Net.Http.Json;
using Common.Interfaces;
using Common.Models;
 
namespace Client.DALs
{
    public class EvaluatorsClientDAL : IEvaluatorsDAL
    {
        private readonly HttpClient httpClient;
 
        public EvaluatorsClientDAL(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
 
        public async Task<List<Evaluator>?> GetAllAsync()
        {
            return await this.httpClient.GetFromJsonAsync<List<Evaluator>>("api/Evaluators");
        }
 
        public async Task<Evaluator?> GetByIdAsync(Guid id)
        {
            var response = await this.httpClient.GetAsync($"api/Evaluators/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Evaluator>();
        }
 
        public async Task AddAsync(Evaluator evaluator)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/Evaluators", evaluator);
            response.EnsureSuccessStatusCode();
        }
 
        public async Task UpdateAsync(Evaluator evaluator)
        {
            var response = await this.httpClient.PutAsJsonAsync("api/Evaluators", evaluator);
            response.EnsureSuccessStatusCode();
        }
 
        public async Task DeleteAsync(Guid id)
        {
            var response = await this.httpClient.DeleteAsync($"api/Evaluators/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

    public interface IEvaluatorsDAL
    {
    }
}