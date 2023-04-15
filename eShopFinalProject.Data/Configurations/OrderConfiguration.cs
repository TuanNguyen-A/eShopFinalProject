using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Data.Enums;

namespace eShopFinalProject.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.orderStatus).HasDefaultValue(OrderStatus.NotProcessed);

            builder.HasOne(x => x.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            builder.HasOne(x => x.Coupon).WithMany(c => c.Orders).HasForeignKey(o => o.CouponId).IsRequired(false);
        }
    }
}
