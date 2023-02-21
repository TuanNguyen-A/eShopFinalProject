using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd()));
            builder.Property(x => x.Slug).IsRequired()
                .HasConversion(new ValueConverter<string, string>(v => v.ToLower(), v => v.ToLower()));
            builder.Property(x => x.Description).IsRequired();
            //TODO: Test trim, unique
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Sold).HasDefaultValue(0);
            builder.Property(x => x.TotalRating).HasDefaultValue(0);
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.BrandId).IsRequired();

            //Relationship
            builder.HasOne(x => x.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId);
            builder.HasOne(x => x.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);

        }
    }
}
