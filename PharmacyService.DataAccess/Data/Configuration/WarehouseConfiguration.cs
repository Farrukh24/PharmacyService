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
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
            builder.Property(w => w.Address).IsRequired().HasMaxLength(500);
            builder.Property(w => w.Capacity).IsRequired();
            // Relationships
            builder.HasMany(w => w.Drugs).WithOne(d => d.Warehouse).HasForeignKey(d => d.WarehouseId);
        }
    }
}
