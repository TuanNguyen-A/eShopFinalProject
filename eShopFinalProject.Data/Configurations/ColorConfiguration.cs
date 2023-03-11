using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eShopFinalProject.Data.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Colors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired()
                .HasConversion(new ValueConverter<string, string>(v => v.ToLower(), v => v.ToLower()));
            builder.HasIndex(x => x.Title).IsUnique();

        }
    }
}
