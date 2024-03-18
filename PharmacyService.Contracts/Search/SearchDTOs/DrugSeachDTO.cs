using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Search.SearchDTOs
{
    public class DrugSeachDTO : BaseSearch
    {
        public string? Manufacturer { get; set; } 
        public double? MaxPrice { get; set; } 
        public double? MinPrice { get; set; } 
    }
}
