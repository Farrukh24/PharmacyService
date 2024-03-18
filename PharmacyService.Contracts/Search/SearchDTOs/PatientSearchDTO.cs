using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Search.SearchDTOs
{
    public class PatientSearchDTO : BaseSearch
    {
        public string? Gender { get; set; }
        public string? Address { get; set; }

    }
}
