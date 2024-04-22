using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AboutQismaConfigurations : IEntityTypeConfiguration<AboutQisma>
    {
        public void Configure(EntityTypeBuilder<AboutQisma> builder)
        {
            builder.ToTable("AboutQisma");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Content).IsRequired(true);
        }
    }
}
