using System.Net.Http.Json;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;

namespace avanphamaceuticalsmanagement.Client.Services
{
    public class GenericService : IGenericService
    {
        private readonly HttpClient _httpClient;

        public GenericService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string url) where T : class
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<T>>(url);
                return result;
            }
            catch (Exception e)
            {
                var _ = e.Message;
                throw;
            }

        }

        public async Task<T> SaveAllAsync<T>(string url, T value) where T : class
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(url, value);
                return await result.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception e)
            {
                var _ = e.Message;
                throw;
            }
        }
        public async Task<T> UpdateAsync<T>(string url, T value) where T: class
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync(url, value);
                return await result.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
            
        }
    }
}
