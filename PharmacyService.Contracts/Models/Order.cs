using Microsoft.AspNetCore.Identity;
using PharmacyService.Contracts.Enums;
using PharmacyService.Contracts.Interfaces;

namespace PharmacyService.Contracts.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int EmployeeId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }

        // Navigation property for ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // Navigation property for OrderLines
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        // Navigation property for Payment
        public Payment Payment { get; set; }
    }
}
