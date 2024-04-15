using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyImageConfigurations : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.ToTable("PropertyImages");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.ImageUrl).IsRequired(true);
            builder.HasOne(i => i.Property).WithMany(i => i.PropertyImages).HasForeignKey(i => i.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasIndex(i => i.PropertyId);
        }
    }
}