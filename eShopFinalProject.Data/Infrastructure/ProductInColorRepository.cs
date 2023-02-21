using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class ProductInColorRepository : BaseRepository<ProductInColor>
    {
        public ProductInColorRepository(eShopDbContext context) : base(context) { }

        //public override async Task<bool> AddAsync(ProductInColor pic)
        //{
        //    var product = pic.Product;
        //    pic.ProductId = _context.Entry(product).Property(x => x.Id).CurrentValue;

        //    var color = pic.Color;
        //    pic.ColorId = _context.Entry(color).Property(x => x.Id).CurrentValue;

        //    var result = await _context.AddAsync(pic);
        //    return true;
        //}
    }
}
