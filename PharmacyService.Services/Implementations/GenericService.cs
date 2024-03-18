using AutoMapper;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Services.Implementations
{
    public class GenericService<TEntity, TEntityDTO> : IGenericService<TEntity, TEntityDTO> where TEntity : class where TEntityDTO : class
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GenericService<TEntity, TEntityDTO>> _logger;
        public GenericService(IRepository<TEntity> repository, IMapper mapper, ILogger<GenericService<TEntity, TEntityDTO>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }      

        public async Task<IEnumerable<TEntityDTO>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();

                return _mapper.Map<IEnumerable<TEntityDTO>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting all {typeof(TEntity).Name}s.");
                throw;
            }
        }

        public async Task<TEntityDTO> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning($"Entity with ID {id} not found.");
                    return null;
                }

                return _mapper.Map<TEntityDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting {typeof(TEntity).Name} by ID: {id}");
                throw;
            }
        }     

        public async Task<TEntityDTO> CreateAsync(TEntityDTO entityDto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(entityDto);
                var addedEntity = await _repository.CreateAsync(entity);
                return _mapper.Map<TEntityDTO>(addedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }       

        public async Task<TEntityDTO> UpdateAsync(int id, TEntityDTO entityDto)
        {
            try
            {
                var entityToUpdate = await _repository.GetByIdAsync(id);
                if (entityToUpdate == null)
                {
                    _logger.LogWarning($"Entity with ID {id} not found.");
                    return null;
                }

                // Update entity properties from DTO
                _mapper.Map(entityDto, entityToUpdate);

                var updatedEntity = await _repository.UpdateAsync(entityToUpdate);
                return _mapper.Map<TEntityDTO>(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entityToDelete = await _repository.GetByIdAsync(id);
                if (entityToDelete == null)
                {
                    _logger.LogWarning($"Entity with ID {id} not found.");
                    return false;
                }

                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting {typeof(TEntity).Name} with ID: {id}");
                throw;
            }
        }
    }
}
