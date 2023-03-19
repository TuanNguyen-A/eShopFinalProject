using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
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
    }
}
