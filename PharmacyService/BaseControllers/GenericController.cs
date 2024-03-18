using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Contracts.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyService.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict access to users with the "admin" role
    public class GenericController<TEntity, TEntityDTO, TService> : ControllerBase
        where TEntity : class
        where TEntityDTO : class
        where TService : IGenericService<TEntity, TEntityDTO>
    {
        protected readonly TService _service;
        protected readonly Microsoft.Extensions.Logging.ILogger _logger;

        public GenericController(TService service, Microsoft.Extensions.Logging.ILogger logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all entities of type TEntity.
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // Allow access to all users for GET methods
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> GetAll()
        {
            try
            {
                var entities = await _service.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all items of type {TEntity}", typeof(TEntity));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving all items.");
            }
        }

        /// <summary>
        /// Retrieves an entity of type TEntity by its id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous] // Allow access to all users for GET methods
        public async Task<ActionResult<TEntityDTO>> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound($"Item of type {typeof(TEntity)} with id {id} not found.");
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item of type {TEntity} with id {Id}", typeof(TEntity), id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the item.");
            }
        }

        /// <summary>
        /// Creates a new entity of type TEntity.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TEntityDTO>> Create([FromBody] TEntityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdDto = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = (int)(typeof(TEntityDTO).GetProperty("Id")?.GetValue(createdDto) ?? 0) }, createdDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item of type {TEntity}", typeof(TEntity));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the item.");
            }
        }

        /// <summary>
        /// Updates an entity of type TEntity by its id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TEntityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item of type {TEntity} with id {Id}", typeof(TEntity), id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the item.");
            }
        }

        /// <summary>
        /// Deletes an entity of type TEntity by its id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedEntity = await _service.DeleteAsync(id);
                if (deletedEntity is false)
                    return StatusCode(StatusCodes.Status404NotFound,$"Item of type {typeof(TEntity)} with id {id} not found.");

                return Ok($"Item of type {typeof(TEntity)} with id {id} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting item of type {TEntity} with id {Id}", typeof(TEntity), id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the item.");
            }
        }
    }
}
