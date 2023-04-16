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
    public interface IOrderRepository : IBaseRepository<Order>
    {
    }
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(eShopDbContext context) : base(context) { }

        public override async Task<List<Order>> AllAsync()
        {
            return await _context.Set<Order>()
                .Include(x => x.User)
                .Include(x => x.Coupon)
                .Include(x => x.ProductInOrders)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public override async Task<Order> GetAsync(int id)
        {
            return await _context.Set<Order>()
                .Include(x => x.User)
                .Include(x => x.Coupon)
                .Include(x => x.ProductInOrders)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
