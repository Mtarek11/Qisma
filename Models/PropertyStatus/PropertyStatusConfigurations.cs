using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyStatusConfigurations : IEntityTypeConfiguration<PropertyStatus>
    {
        public void Configure(EntityTypeBuilder<PropertyStatus> builder)
        {
            builder.ToTable("PropertyStatuses");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Status).IsRequired(true);
            builder.Property(i => i.From).IsRequired(true);
            builder.Property(i => i.To).IsRequired(false);
            builder.HasOne(i => i.Property).WithMany(i => i.PropertyStatus).HasForeignKey(i => i.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasIndex(i => new { i.PropertyId, i.To }).IsUnique();
        }
    }
}