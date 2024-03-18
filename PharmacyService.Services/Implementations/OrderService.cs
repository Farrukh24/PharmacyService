using AutoMapper;
using Microsoft.Extensions.Logging;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Enums;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Drug> _drugRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrderService(IRepository<Order> orderRepository, IRepository<Drug> drugRepository, IRepository<Patient> patientRepository, IMapper mapper, ILogger logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _drugRepository = drugRepository ?? throw new ArgumentNullException(nameof(drugRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task<OrderDTO> CreateAsync(OrderDTO orderDto)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(orderDto.PatientId);
                if (patient == null)
                {
                    _logger.LogError("Invalid Patient ID: {PatientId}", orderDto.PatientId);
                    throw new ArgumentException("Invalid Patient ID.");
                }

                decimal totalPrice = 0;
                foreach (var lineDto in orderDto.OrderLines)
                {
                    var drug = await _drugRepository.GetByIdAsync(lineDto.DrugId);
                    if (drug == null || drug.Stock < lineDto.Quantity)
                    {
                        _logger.LogError("Drug with ID {DrugId} is out of stock or insufficient quantity", lineDto.DrugId);
                        throw new InvalidOperationException($"Drug with ID {lineDto.DrugId} is out of stock or insufficient quantity.");
                    }

                    totalPrice += drug.Price * lineDto.Quantity;
                    drug.Stock -= lineDto.Quantity;
                    await _drugRepository.UpdateAsync(drug);
                }

                var order = _mapper.Map<Order>(orderDto);
                order.TotalPrice = totalPrice;
                order.Status = OrderStatus.Placed;

                var savedOrder = await _orderRepository.CreateAsync(order);

                return _mapper.Map<OrderDTO>(savedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the order");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                return orders.Select(order => _mapper.Map<OrderDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all orders");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync(OrderStatus? status = null, int? patientId = null)
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                var filteredOrders = orders
                    .Where(order => !status.HasValue || order.Status == status.Value)
                    .Where(order => !patientId.HasValue || order.PatientId == patientId.Value);

                return filteredOrders.Select(order => _mapper.Map<OrderDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders");
                throw;
            }
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogError("Order with ID {OrderId} not found", id);
                    throw new Exception($"Order with ID {id} not found.");
                }

                return _mapper.Map<OrderDTO>(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderHistoryAsync(int patientId)
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                var patientOrders = orders.Where(order => order.PatientId == patientId);

                return patientOrders.Select(order => _mapper.Map<OrderDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order history for patient ID {PatientId}", patientId);
                throw;
            }
        }

        public async Task<OrderDTO> UpdateAsync(int id, OrderDTO entityDto)
        {
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    _logger.LogError("Order with ID {OrderId} not found", id);
                    throw new Exception($"Order with ID {id} not found.");
                }

                _mapper.Map(entityDto, existingOrder);
                var updatedOrder = await _orderRepository.UpdateAsync(existingOrder);

                return _mapper.Map<OrderDTO>(updatedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    _logger.LogError("Order with ID {OrderId} not found", id);
                    throw new Exception($"Order with ID {id} not found.");
                }

                return await _orderRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDTO>> SearchOrdersAsync(OrderSearchDTO search)
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                var query = orders.AsQueryable();

                query = search.StartDate.HasValue ? query.Where(order => order.OrderDate >= search.StartDate.Value) : query;
                query = search.EndDate.HasValue ? query.Where(order => order.OrderDate <= search.EndDate.Value) : query;
                query = search.PatientId.HasValue ? query.Where(order => order.PatientId == search.PatientId.Value) : query;
                query = search.Status.HasValue ? query.Where(order => order.Status == search.Status.Value) : query;

                return query.Select(order => _mapper.Map<OrderDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching orders");
                throw;
            }
        }
    }

}
