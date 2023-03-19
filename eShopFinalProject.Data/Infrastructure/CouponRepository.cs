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
    public interface ICouponRepository : IBaseRepository<Coupon>
    {
    }
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(eShopDbContext context) : base(context) { }
    }
}
