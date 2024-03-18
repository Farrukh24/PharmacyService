using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.Interfaces;
using System.Linq.Expressions;

namespace PharmacyService.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<Repository<TEntity>> _logger;

        public Repository(AppDbContext dbContext, ILogger<Repository<TEntity>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting entity by ID: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting all entities: {ex.Message}");
                throw; 
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding entity: {ex.Message}");
                throw; 
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating entity: {ex.Message}");
                throw; 
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null)
                    return false;

                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting entity: {ex.Message}");
                throw; 
            }
        }
    }
}
