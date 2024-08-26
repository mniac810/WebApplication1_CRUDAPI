using BookStoreApi.Models;

public interface IRepository<T> where T : BaseEntity
{
    Task CreateAsync(T entity);
    Task CreateMultipleAsync(T[] entities);
    Task UpdateAsync(string entityId, T entity);
    Task RemoveAsync(string entityId);
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(string entityId);
}