namespace avanphamaceuticalsmanagement.Client.Services.Interfaces
{
    public interface IRolesService
    {
        Task<string> SaveRole(string url, string value);
    }
}