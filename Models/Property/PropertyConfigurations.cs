using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyConfigurations : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Location).IsRequired(true);
            builder.Property(i => i.UnitPrice).IsRequired(true);
            builder.Property(i => i.Description).IsRequired(true);
            builder.Property(i => i.MaintenanceCost).IsRequired(false);
            builder.Property(i => i.TransactionFees).IsRequired(false);
            builder.Property(i => i.NumberOfShares).IsRequired(true);
            builder.Property(i => i.Status).IsRequired(true);
            builder.Property(i => i.AvailableShares).HasComputedColumnSql("NumberOfShares - UsedShares");
            builder.Property(i => i.UsedShares).HasDefaultValue(0);
            builder.Property(i => i.SharePrice).IsRequired(true);
            builder.Property(i => i.AnnualRentalYield).IsRequired(true);
            builder.Property(i => i.AnnualPriceAppreciation).IsRequired(true);
            builder.Property(i => i.DownPayment).IsRequired(false);
            builder.Property(i => i.MonthlyInstallment).IsRequired(false);
            builder.Property(i => i.NumberOfYears).IsRequired(false);
            builder.Property(i => i.MaintenaceInstallment).IsRequired(false);
            builder.Property(i => i.DeliveryInstallment).IsRequired(false);
            builder.Property(i => i.Type).IsRequired(true);
            builder.Property(i => i.MinOfShares).IsRequired(true);
            builder.Property(i => i.IsDeleted).HasDefaultValue(false);
            builder.Property(i => i.LastModificationDate).IsRequired(true);
            builder.HasIndex(i => i.Location);
            builder.HasIndex(i => i.UnitPrice);
            builder.HasIndex(i => i.Type);
            builder.HasIndex(i => i.SharePrice);
            builder.HasIndex(i => new { i.Location, i.UnitPrice, i.Type });
            builder.HasIndex(i => new { i.Location, i.UnitPrice, i.SharePrice });
            builder.HasIndex(i => new { i.Location, i.Type, i.SharePrice });
            builder.HasIndex(i => new { i.UnitPrice, i.Type, i.SharePrice });
            builder.HasIndex(i => new { i.Location, i.UnitPrice });
            builder.HasIndex(i => new { i.Location, i.Type });
            builder.HasIndex(i => new { i.Location, i.SharePrice });
            builder.HasIndex(i => new { i.UnitPrice, i.Type });
            builder.HasIndex(i => new { i.UnitPrice, i.SharePrice });
            builder.HasIndex(i => new { i.Type, i.SharePrice });
            builder.HasIndex(i => new { i.Location, i.UnitPrice, i.Type, i.SharePrice });
            builder.HasIndex(i => i.IsDeleted);
            builder.HasIndex(i => new { i.Id, i.IsDeleted });
            builder.HasOne(i => i.Governorate).WithMany(i => i.Properties).HasForeignKey(i => i.GovernorateId).OnDelete(DeleteBehavior.NoAction).IsRequired(true);
            builder.HasOne(i => i.City).WithMany(i => i.Properties).HasForeignKey(i => i.CityId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
        }
    }
}