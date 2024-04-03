using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(i => i.PhoneNumber).IsRequired(true);
            builder.Property(i => i.Address).IsRequired(true);
            builder.Property(i => i.UserName).IsRequired(false);
            builder.Property(i => i.Email).IsRequired(true);
            builder.Property(i => i.IdentityNumber).IsRequired(true);
            builder.Property(i => i.CompanyName).IsRequired(false);
            builder.Property(i => i.DateOfBirth).IsRequired(true);
            builder.Property(i => i.FirstName).IsRequired(true);
            builder.Property(i => i.MiddleName).IsRequired(true);
            builder.Property(i => i.LastName).IsRequired(true);
            builder.Property(i => i.IdentityImageUrl).IsRequired(true);
            builder.Property(i => i.InvestoreType).IsRequired(true);
            builder.Property(i => i.ReciveEmails).IsRequired(true);
            builder.Property(i => i.Occupation).IsRequired(false);
            builder.HasIndex(i => i.PhoneNumber).IsUnique(true);
            builder.HasIndex(i => i.Email).IsUnique(true);
            builder.HasIndex(i => i.IdentityNumber).IsUnique(true);
        }
    }
}
