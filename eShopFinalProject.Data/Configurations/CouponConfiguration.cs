using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eShopFinalProject.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasConversion(new ValueConverter<string, string>(v => v.ToUpper(), v => v.ToUpper()));

            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Expiry).IsRequired();
            builder.Property(x => x.Discount).IsRequired();
        }
    }
}
