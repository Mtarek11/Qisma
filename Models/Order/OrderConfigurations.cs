﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.NumberOfShares).IsRequired(true);
            builder.Property(i => i.OrderNumber).IsRequired(true);
            builder.Property(i => i.SharePrice).IsRequired(true);
            builder.Property(i => i.TransactionFees).IsRequired(false);
            builder.Property(i => i.DownPayment).IsRequired(false);
            builder.Property(i => i.MonthlyInstallment).IsRequired(false);
            builder.Property(i => i.NumberOfYears).IsRequired(false);
            builder.Property(i => i.MaintenaceInstallment).IsRequired(false);
            builder.Property(i => i.DeliveryInstallment).IsRequired(false);
            builder.Property(i => i.OrderDate).IsRequired(true);
            builder.Property(i => i.OrderStatus).IsRequired(true);
            builder.Property(i => i.ConfirmationDate).IsRequired(false);
            builder.HasIndex(i => new { i.OrderDate, i.OrderStatus });
            builder.HasIndex(i => i.OrderStatus );
            builder.HasIndex(i => i.UserId);
            builder.HasIndex(i => new { i.UserId, i.OrderStatus });
            builder.HasOne(i => i.Property).WithMany(i => i.Orders).HasForeignKey(i => i.PropertyId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            builder.HasOne(i => i.User).WithMany(i => i.Orders).HasForeignKey(i => i.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
        }
    }
}
