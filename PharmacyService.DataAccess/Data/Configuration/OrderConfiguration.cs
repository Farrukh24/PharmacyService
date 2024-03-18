using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Contracts.Models;

namespace PharmacyService.DataAccess.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status).IsRequired();
            // Relationships
            builder.HasOne(o => o.Patient).WithMany(p => p.Orders).HasForeignKey(o => o.PatientId);
            builder.HasOne(o => o.ApplicationUser).WithMany().HasForeignKey(o => o.ApplicationUserId);
            builder.HasMany(o => o.OrderLines).WithOne(ol => ol.Order).HasForeignKey(ol => ol.OrderId);
        }
    }
}
