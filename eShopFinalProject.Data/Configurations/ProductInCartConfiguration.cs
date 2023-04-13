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
    public class ProductInCartConfiguration : IEntityTypeConfiguration<ProductInCart>
    {
        public void Configure(EntityTypeBuilder<ProductInCart> builder)
        {
            builder.ToTable("ProductInCarts");
            builder.HasKey(t => new { t.CartId, t.ProductId });

            builder.HasOne(x => x.Cart).WithMany(o => o.ProductInCarts).HasForeignKey(x => x.CartId);
            builder.HasOne(x => x.Product).WithMany(p => p.ProductInCarts).HasForeignKey(x => x.ProductId);
        }
    }
}
