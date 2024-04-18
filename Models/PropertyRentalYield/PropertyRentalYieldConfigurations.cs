using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyRentalYieldConfigurations : IEntityTypeConfiguration<PropertyRentalYield>
    {
        public void Configure(EntityTypeBuilder<PropertyRentalYield> builder)
        {
            builder.ToTable("PropertyRentalYields");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.RentalYield).IsRequired(true);
            builder.Property(i => i.From).IsRequired(true);
            builder.Property(i => i.To).IsRequired(false);
            builder.HasOne(i => i.Property).WithMany(i => i.PropertyRentalYields).HasForeignKey(i => i.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
        }
    }
}