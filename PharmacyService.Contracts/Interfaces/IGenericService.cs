using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Interfaces
{
    public interface IGenericService<TEntity,TEntityDTO> where TEntity : class where TEntityDTO : class
    {
        Task<IEnumerable<TEntityDTO>> GetAllAsync();
        Task<TEntityDTO> GetByIdAsync(int id);
        Task<TEntityDTO> CreateAsync(TEntityDTO entityDto);
        Task<TEntityDTO> UpdateAsync(int id, TEntityDTO entityDto);
        Task<bool> DeleteAsync(int id);
    }
}
