using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eShopFinalProject.Data.Infrastructure
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
    }
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(eShopDbContext context) : base(context) { }

        public override async Task<List<Cart>> FindAsync(Expression<Func<Cart, bool>> predicate)
        {
            return await _context.Set<Cart>()
                .Include(x => x.ProductInCarts)
                .ThenInclude(x => x.Product)
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }
    }
}
