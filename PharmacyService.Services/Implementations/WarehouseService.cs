using AutoMapper;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Services.Implementations
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IRepository<Drug> _drugRepository;
        private readonly ILogger<WarehouseService> _logger;
        private readonly IMapper _mapper;

        public WarehouseService(IRepository<Warehouse> warehouseRepository, IRepository<Drug> drugRepository, ILogger<WarehouseService> logger, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _drugRepository = drugRepository ?? throw new ArgumentNullException(nameof(drugRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        public async Task<WarehouseDTO> CreateAsync(WarehouseDTO entityDto)
        {
            try
            {
                var warehouse = _mapper.Map<Warehouse>(entityDto);
                var createdWarehouse = await _warehouseRepository.CreateAsync(warehouse);

                _logger.LogInformation("Warehouse created: {@Warehouse}", createdWarehouse);

                return _mapper.Map<WarehouseDTO>(createdWarehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a warehouse.");
                throw;
            }
        }

        public async Task<IEnumerable<WarehouseDTO>> GetAllAsync()
        {
            try
            {
                var warehouses = await _warehouseRepository.GetAllAsync();
                var warehouseDTOs = _mapper.Map<IEnumerable<WarehouseDTO>>(warehouses);

                return warehouseDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all warehouses.");
                throw;
            }
        }

        public async Task<WarehouseDTO> GetByIdAsync(int id)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(id);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}", id);
                    return null;
                }

                var warehouseDTO = _mapper.Map<WarehouseDTO>(warehouse);
                return warehouseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving warehouse with ID: {WarehouseId}", id);
                throw;
            }
        }

        public async Task<WarehouseDTO> UpdateAsync(int id, WarehouseDTO entityDto)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(id);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}. Update operation aborted.", id);
                    throw new Exception($"Warehouse with ID {id} not found.");
                }

                _mapper.Map(entityDto, warehouse);

                await _warehouseRepository.UpdateAsync(warehouse);

                _logger.LogInformation("Warehouse with ID: {WarehouseId} updated successfully.", id);

                return _mapper.Map<WarehouseDTO>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating warehouse with ID: {WarehouseId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(id);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}. Delete operation aborted.", id);
                    return false;
                }

                await _warehouseRepository.DeleteAsync(id);

                _logger.LogInformation("Warehouse with ID: {WarehouseId} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting warehouse with ID: {WarehouseId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DrugDTO>> GetDrugsInWarehouseAsync(int warehouseId)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}. Get drugs operation aborted.", warehouseId);
                    return Enumerable.Empty<DrugDTO>();
                }

                var drugDTOs = warehouse.Drugs.Select(drug => _mapper.Map<DrugDTO>(drug));
                return drugDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting drugs for warehouse with ID: {WarehouseId}", warehouseId);
                throw;
            }
        }

        public async Task<bool> AddDrugToWarehouseAsync(int warehouseId, IEnumerable<DrugDTO> drugDTOs)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}. Add drugs operation aborted.", warehouseId);
                    return false;
                }

                var drugs = drugDTOs.Select(dto => _mapper.Map<Drug>(dto));
                foreach (var drug in drugs)
                {
                    warehouse.Drugs.Add(drug);
                }

                await _warehouseRepository.UpdateAsync(warehouse);

                _logger.LogInformation("Drugs added to warehouse with ID: {WarehouseId}", warehouseId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding drugs to warehouse with ID: {WarehouseId}", warehouseId);
                throw;
            }
        }

        public async Task<bool> RemoveDrugFromWarehouseAsync(int warehouseId, IEnumerable<int> drugIds)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse not found with ID: {WarehouseId}. Remove drugs operation aborted.", warehouseId);
                    return false;
                }

                var drugsToRemove = warehouse.Drugs.Where(drug => drugIds.Contains(drug.Id)).ToList();
                foreach (var drugToRemove in drugsToRemove)
                {
                    warehouse.Drugs.Remove(drugToRemove);
                }

                await _warehouseRepository.UpdateAsync(warehouse);

                _logger.LogInformation("Drugs removed from warehouse with ID: {WarehouseId}", warehouseId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing drugs from warehouse with ID: {WarehouseId}", warehouseId);
                throw;
            }

        }
    }
}
