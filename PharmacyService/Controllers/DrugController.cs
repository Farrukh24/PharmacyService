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
    public class DrugController : GenericController<Drug, DrugDTO, IGenericService<Drug, DrugDTO>>
    {
        private readonly ILogger<DrugController> _logger;
        public DrugController(IGenericService<Drug, DrugDTO> service, ILogger<DrugController> logger)
        : base(service, logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
