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
    public class ProductInColorConfiguration : IEntityTypeConfiguration<ProductInColor>
    {
        public void Configure(EntityTypeBuilder<ProductInColor> builder)
        {
            builder.ToTable("ProductInColors");
            builder.HasKey(t => new { t.ColorId, t.ProductId });

            builder.HasOne(x => x.Product).WithMany(p => p.ProductInColors).HasForeignKey(pic => pic.ProductId);
            builder.HasOne(x => x.Color).WithMany(p => p.ProductInColors).HasForeignKey(pic => pic.ColorId);
        }
    }
}