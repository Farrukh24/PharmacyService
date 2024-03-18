using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.BaseControllers;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;
using Serilog;

namespace PharmacyService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : GenericController<Order, OrderDTO, IOrderService>
    {
        public OrderController(IOrderService service, ILogger<OrderController> logger)
        : base(service, logger)
        {
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> SearchOrders([FromQuery] OrderSearchDTO search)
        {
            try
            {
                var orders = await _service.SearchOrdersAsync(search);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error searching orders");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("history/{patientId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderHistory(int patientId)
        {
            try
            {
                var orderHistory = await _service.GetOrderHistoryAsync(patientId);
                return Ok(orderHistory);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving order history for patient with ID {PatientId}", patientId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
