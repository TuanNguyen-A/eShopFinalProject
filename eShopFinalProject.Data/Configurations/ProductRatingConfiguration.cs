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
    public class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder.ToTable("ProductRatings");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.UserId).IsRequired();
            builder.Property(t => t.ProductId).IsRequired();

            builder.HasOne(x => x.Product).WithMany(p => p.ProductRatings).HasForeignKey(pic => pic.ProductId);
            builder.HasOne(x => x.User).WithMany(p => p.ProductRatings).HasForeignKey(pic => pic.UserId);
        }
    }
}
