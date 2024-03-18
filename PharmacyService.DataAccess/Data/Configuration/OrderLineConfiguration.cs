using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.DataAccess.Data.Configuration
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(ol => new { ol.OrderId, ol.DrugId });
            builder.Property(ol => ol.Quantity).IsRequired();
            // Relationships
            builder.HasOne(ol => ol.Order).WithMany(o => o.OrderLines).HasForeignKey(ol => ol.OrderId);
            builder.HasOne(ol => ol.Drug).WithMany(d => d.OrderLines).HasForeignKey(ol => ol.DrugId);
        }
    }
}
