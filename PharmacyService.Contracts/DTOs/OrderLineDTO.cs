using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.DTOs
{
    public class OrderLineDTO
    {
        public int Id { get; set; } 
        public int DrugId { get; set; }
        public int Quantity { get; set; }
    }
}
