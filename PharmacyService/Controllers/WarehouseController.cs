using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.BaseControllers;
using PharmacyService.Contracts.DTOs;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;

namespace PharmacyService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class WarehouseController : GenericController<Warehouse, WarehouseDTO, IWarehouseService>
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(IWarehouseService warehouseService, ILogger<WarehouseController> logger)
            : base(warehouseService, logger)
        {
            _warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Warehouse/{warehouseId}/drugs
        [AllowAnonymous]
        [HttpGet("{warehouseId}/drugs")]
        public async Task<ActionResult<IEnumerable<DrugDTO>>> GetDrugsInWarehouse(int warehouseId)
        {
            try
            {
                var drugs = await _warehouseService.GetDrugsInWarehouseAsync(warehouseId);
                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drugs in warehouse with ID: {WarehouseId}", warehouseId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/Warehouse/{warehouseId}/drugs
        [HttpPost("{warehouseId}/drugs")]
        public async Task<ActionResult> AddDrugsToWarehouse(int warehouseId, [FromBody] IEnumerable<DrugDTO> drugDTOs)
        {
            try
            {
                var success = await _warehouseService.AddDrugToWarehouseAsync(warehouseId, drugDTOs);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Warehouse with ID {warehouseId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding drugs to warehouse with ID: {WarehouseId}", warehouseId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE: api/Warehouse/{warehouseId}/drugs
        [HttpDelete("{warehouseId}/drugs")]
        public async Task<ActionResult> RemoveDrugsFromWarehouse(int warehouseId, [FromBody] IEnumerable<int> drugIds)
        {
            try
            {
                var success = await _warehouseService.RemoveDrugFromWarehouseAsync(warehouseId, drugIds);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Warehouse with ID {warehouseId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing drugs from warehouse with ID: {WarehouseId}", warehouseId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
