using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CityConfigurations : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.NameAr).IsRequired(true);
            builder.Property(i => i.NameEn).IsRequired(true);
            builder.HasOne(c => c.Governorate).WithMany(g => g.Cities).HasForeignKey(c => c.GovernorateId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasIndex(i => i.GovernorateId);
        }
    }
}
