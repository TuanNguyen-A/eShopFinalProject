using eShopFinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure.Interface
{
    public interface IUnitOfWork
    {
        IBaseRepository<AppRole> AppRoleRepository { get; }
        IBaseRepository<AppUser> AppUserRepository { get; }
        IBaseRepository<Blog> BaseRepository { get; }
        IBaseRepository<Brand> BrandRepository { get; }
        IBaseRepository<Category> CategoryRepository { get; }
        IBaseRepository<Color> ColorRepository { get; }
        IBaseRepository<Coupon> CouponRepository { get; }
        IBaseRepository<Order> OrderRepository { get; }
        IBaseRepository<Product> ProductRepository { get; }
        IBaseRepository<ProductInColor> ProductInColorRepository { get; }
        IBaseRepository<ProductInOrder> ProductInOrderRepository { get; }
        IBaseRepository<ProductRating> ProductRatingRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
