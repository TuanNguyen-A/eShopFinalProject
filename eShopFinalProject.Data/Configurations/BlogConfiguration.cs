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
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.NumView).HasDefaultValue(0);
            builder.Property(x => x.Title).IsRequired();

            builder.HasOne(x => x.User).WithMany(u => u.Blogs).HasForeignKey(b => b.UserId);
            builder.HasOne(x => x.Category).WithMany(u => u.Blogs).HasForeignKey(b => b.CategoryId);
            builder.HasMany(e => e.Images)
              .WithOne(e => e.Blog)
              .HasForeignKey(e => e.BlogId);
        }
    }
}
