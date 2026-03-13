using ECommerceAPI.Data;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repository
{
    public class GenericRepository<T> :DbProvider , IGenericRepository<T> where T : class
    {
        public GenericRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<T> addAsync(T t)
        {
            _dbContext.Set<T>().Add(t);
            await _dbContext.SaveChangesAsync();
            return t;
        }

        public async Task<bool> deleteAsync(int id)
        {
            var t = await getByIdAsync(id);
            if (t == null) return false;

            _dbContext.Set<T>().Remove(t);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> getAllAsync()
        {
            return _dbContext.Set<T>().ToList();
        }

        public async Task<T?> getByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> updateAsync(T t)
        {
            _dbContext.Set<T>().Update(t);
            await _dbContext.SaveChangesAsync();
            return t;
        }
    }
}
