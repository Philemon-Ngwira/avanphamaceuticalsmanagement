namespace AvanPharmacyDomain.Interfaces
{
    public interface IGenericRepository
    {
        Task<IQueryable<T>> GetAll<T>() where T : class;
        Task<IEnumerable<T>> GetAsync<T>() where T : class;
        Task<T> GetbyIdAsync<T>(int Id) where T : class, new();
        Task<T> updateAsync<T>(T Value) where T : class;
    }
}