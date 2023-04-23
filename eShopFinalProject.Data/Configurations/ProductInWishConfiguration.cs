using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Configurations
{
    public class ProductInWishConfiguration : IEntityTypeConfiguration<ProductInWish>
    {
        public void Configure(EntityTypeBuilder<ProductInWish> builder)
        {
            builder.ToTable("ProductInWishes");
            builder.HasKey(t => new { t.UserId, t.ProductId });

            builder.HasOne(x => x.User).WithMany(u => u.ProductInWishes).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Product).WithMany(u => u.ProductInWishes).HasForeignKey(x => x.ProductId);
        }
    }
}
