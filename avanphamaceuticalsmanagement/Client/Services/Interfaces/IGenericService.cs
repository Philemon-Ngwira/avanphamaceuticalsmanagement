namespace avanphamaceuticalsmanagement.Client.Services.Interfaces
{
    public interface IGenericService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(string url) where T : class;
        Task<T> SaveAllAsync<T>(string url, T value) where T : class;
        Task<T> UpdateAsync<T>(string url, T value) where T : class;
    }
}