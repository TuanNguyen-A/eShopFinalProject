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
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");
            builder.HasKey(x => x.PublicId);
            builder.Property(x => x.Url).IsRequired();
            builder.HasOne(x => x.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i=>i.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); ;
        }
    }
}
