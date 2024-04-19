using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuyTrackerConfigurations : IEntityTypeConfiguration<BuyTracker>
    {
        public void Configure(EntityTypeBuilder<BuyTracker> builder)
        {
            builder.ToTable("BuyTrackers");
            builder.HasKey(i => new {i.UserId, i.PropertyId});
            builder.HasOne(c => c.User).WithMany(g => g.BuyTrackers).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasOne(c => c.Property).WithMany(g => g.BuyTrackers).HasForeignKey(c => c.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasIndex(i => new { i.UserId, i.PropertyId }).IsUnique();
        }
    }
}
