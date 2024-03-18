using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Search.SearchDTOs
{
    public class PrescriptionSearchDTO : BaseSearch
    {
        public int? PatientId { get; set; } 
        public int? DoctorId { get; set; } 
        public DateTime? IssueDateFrom { get; set; } 
        public DateTime? IssueDateTo { get; set; } 
    }
}
