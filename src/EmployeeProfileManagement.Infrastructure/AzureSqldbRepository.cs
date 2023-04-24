using EmployeeProfileManagement.Core.Model;
using EmployeeProfileManagement.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace EmployeeProfileManagement.Infrastructure
{
    public class AzureSqldbRepository<T> : IRepository<T> where T : EntityBase
    {
        private readonly AzureSqldbContext _context;
        private readonly ILogger<AzureSqldbRepository<T>> _logger;

        public AzureSqldbRepository(AzureSqldbContext context, ILogger<AzureSqldbRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Adds an entity to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T item)
        {
            try
            {
                var newEntity = await _context.Set<T>().AddAsync(item);
                await _context.SaveChangesAsync();
                return newEntity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding entity, error : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes an entity from the database matching the supplied id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true - on deletion, false - no match or error</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingEntity = await GetByIdAsync(id);
                if (existingEntity != null)
                {
                    var result = _context.Set<T>().Remove(existingEntity);
                    var success = await _context.SaveChangesAsync();
                    if (success != 0)
                        return true;
                    else return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting entity, error : {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Returns all entities in the database
        /// </summary>
        /// <returns>Entities </returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _context.Set<T>().ToListAsync();
                return entities;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error retrieving entities, error : {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().Where(e => e.Id == id).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving entities, error : {ex.Message}");
                throw;
            }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                var existingEntity = await GetByIdAsync(item.Id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                return existingEntity;
            }
            catch(Exception ex )
            {
                _logger.LogError($"Error updating entity {item.Id}, error : {ex.Message}");
                throw;
            }
        }
    }
}
