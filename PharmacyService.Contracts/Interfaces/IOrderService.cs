using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Enums;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Interfaces
{
    public interface IOrderService : IGenericService<Order, OrderDTO>
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync(OrderStatus? status = null, int? patientId = null);
        Task<IEnumerable<OrderDTO>> SearchOrdersAsync(OrderSearchDTO search);
        Task<IEnumerable<OrderDTO>> GetOrderHistoryAsync(int patientId);
    }
}
