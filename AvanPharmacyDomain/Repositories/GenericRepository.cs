using AvanPharmacyDomain.Data;
using AvanPharmacyDomain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvanPharmacyDomain.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly avanpharmacyDbContext _dbContext;

        public GenericRepository(avanpharmacyDbContext dbContext) { _dbContext = dbContext; }

     
        public async Task<IEnumerable<T>> GetAsync<T>() where T : class
        {
            var result = await _dbContext.Set<T>().AsQueryable().ToListAsync();
            return result;

        }

        public async Task<IQueryable<T>> GetAll<T>() where T : class
        {

            return _dbContext.Set<T>().AsQueryable();
        }
        public async Task<T> GetbyIdAsync<T>(int Id) where T : class, new()
        {
            try
            {
                var result = await _dbContext.Set<T>().FindAsync(Id);
                if (result == null)
                    return new T();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> updateAsync<T>(T Value) where T : class
        {
            try
            {
                var result = _dbContext.Update(Value);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> SaveAllAsync<T>(T value) where T : class
        {
            try
            {
                var result = await _dbContext.AddAsync<T>(value);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw;
            }
        }

        public async Task DeleteAllAsync<T>(int id) where T : class
        {
            try
            {
                var result = await _dbContext.Set<T>().FindAsync(id);
                if (result == null)
                    return;

                _dbContext.Remove(result);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
