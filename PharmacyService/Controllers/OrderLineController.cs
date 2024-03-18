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
    [Authorize] // Requires authorization for all endpoints in this controller
    public class OrderLineController : GenericController<OrderLine, OrderLineDTO, IGenericService<OrderLine, OrderLineDTO>>
    {
        public OrderLineController(IGenericService<OrderLine, OrderLineDTO> service, ILogger<OrderLineController> logger)
            : base(service, logger)
        {
        }
    }

}
