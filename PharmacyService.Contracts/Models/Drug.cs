using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int WarehouseId { get; set; }

        // Navigation property for Warehouse
        public Warehouse Warehouse { get; set; }

        // Navigation property for the many-to-many relationship with Order
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
