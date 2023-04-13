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
    public interface ICartRepository : IBaseRepository<Cart>
    {
    }
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(eShopDbContext context) : base(context) { }
    }
}
