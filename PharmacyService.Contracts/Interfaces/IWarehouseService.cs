using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Interfaces
{
    public interface IWarehouseService : IGenericService<Warehouse, WarehouseDTO>
    {
        Task<IEnumerable<DrugDTO>> GetDrugsInWarehouseAsync(int warehouseId);
        Task<bool> AddDrugToWarehouseAsync(int warehouseId, IEnumerable<DrugDTO> drugDTOs);
        Task<bool> RemoveDrugFromWarehouseAsync(int warehouseId, IEnumerable<int> drugIds);
    }
}
