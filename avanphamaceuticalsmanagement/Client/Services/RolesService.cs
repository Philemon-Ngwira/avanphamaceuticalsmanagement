using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using Newtonsoft.Json;

namespace avanphamaceuticalsmanagement.Client.Services
{
    public class RolesService : IRolesService
    {
        private readonly HttpClient _httpClient;
        public RolesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SaveRole(string url, string roleName)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(roleName), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(url, content);

                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<string>(responseContent);
                }
                else
                {
                    throw new Exception($"Failed to save role: {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
        }
    }
}
