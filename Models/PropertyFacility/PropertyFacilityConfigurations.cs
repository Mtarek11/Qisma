using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyFacilityConfigurations : IEntityTypeConfiguration<PropertyFacility>
    {
        public void Configure(EntityTypeBuilder<PropertyFacility> builder)
        {
            builder.ToTable("PropertyFacilities");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Description).IsRequired(false);
            builder.Property(i => i.Number).IsRequired(true);
            builder.HasOne(i => i.Facility).WithMany(i => i.PropertyFacilities).HasForeignKey(i => i.FacilityId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasOne(i => i.Property).WithMany(i => i.PropertyFacilities).HasForeignKey(i => i.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasIndex(i => i.PropertyId);
            builder.HasIndex(i => new {i.PropertyId, i.Number}).IsUnique();
        }
    }
}