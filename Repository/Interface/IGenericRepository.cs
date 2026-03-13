using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> getAllAsync();
        Task<T?> getByIdAsync(int id);
        Task<T> addAsync(T t);
        Task<T> updateAsync(T t);
        Task<bool> deleteAsync(int id);
    }
}
