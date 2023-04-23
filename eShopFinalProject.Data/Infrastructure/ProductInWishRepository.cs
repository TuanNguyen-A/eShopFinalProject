using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public interface IProductInWishRepository : IBaseRepository<ProductInWish>
    {
        Task<List<Product>> GetWishListByUserId(Guid userId);
    }

    public class ProductInWishRepository : BaseRepository<ProductInWish>, IProductInWishRepository
    {
        public ProductInWishRepository(eShopDbContext context) : base(context) { }

        public async Task<List<Product>> GetWishListByUserId(Guid userId)
        {
            var productInWishes = await _context.Set<ProductInWish>()
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .ToListAsync();
            List<Product> result = new List<Product>();
            foreach(ProductInWish piw in productInWishes)
            {
                result.Add(piw.Product);
            }
            return result;
        }
    }
}
