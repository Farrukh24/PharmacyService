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
    [Authorize]
    public class PatientController : GenericController<Patient, PatientDTO, IGenericService<Patient, PatientDTO>>
    {
        public PatientController(IGenericService<Patient, PatientDTO> service, ILogger<PatientController> logger)
        : base(service, logger)
        {
        }

        // we can add additional endpoints if needed
    }
}
