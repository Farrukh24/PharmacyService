using PharmacyService.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.DTOs
{
    public class OrderSearchDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PatientId { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
