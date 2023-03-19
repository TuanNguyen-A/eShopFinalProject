using eShopFinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Data.Enums;

namespace eShopFinalProject.Data.Configurations
{
    public class EnqConfiguration : IEntityTypeConfiguration<Enq>
    {
        public void Configure(EntityTypeBuilder<Enq> builder)
        {
            builder.ToTable("Enqs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.PhoneNumber).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(EnqStatus.Submitted);
        }
    }
}
