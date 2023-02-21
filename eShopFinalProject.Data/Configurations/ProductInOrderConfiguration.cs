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
    public class ProductInOrderConfiguration : IEntityTypeConfiguration<ProductInOrder>
    {
        public void Configure(EntityTypeBuilder<ProductInOrder> builder)
        {
            builder.ToTable("ProductInOrders");
            builder.HasKey(t => new { t.OrderId, t.ProductId });

            builder.HasOne(x => x.Order).WithMany(o => o.ProductInOrders).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Product).WithMany(p => p.ProductInOrders).HasForeignKey(x => x.ProductId);
        }
    }
}
