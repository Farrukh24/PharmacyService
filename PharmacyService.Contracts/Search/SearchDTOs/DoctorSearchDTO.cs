using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Search.SearchDTOs
{
    public class DoctorSearchDTO : BaseSearch
    {
        public string? Specialization { get; set; }
        public string? ContactInfo { get; set; }
    }
}
