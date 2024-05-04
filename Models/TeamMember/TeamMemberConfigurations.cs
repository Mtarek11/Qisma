using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TeamMemberConfigurations : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.ImageUrl).IsRequired(true);
            builder.Property(i => i.Name).IsRequired(true);
            builder.Property(i => i.JobTitle).IsRequired(true);
            builder.Property(i => i.Summary).IsRequired(true);
            builder.Property(i => i.FacebookLink).IsRequired(false);
            builder.Property(i => i.XLink).IsRequired(false);
            builder.Property(i => i.InstagramLink).IsRequired(false);
            builder.Property(i => i.LinkedInLink).IsRequired(false);
            builder.Property(i => i.IsManager).IsRequired(true);
        }
    }
}