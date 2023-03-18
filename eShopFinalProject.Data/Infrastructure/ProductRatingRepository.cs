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
    public interface IProductRatingRepository : IBaseRepository<ProductRating>
    {
    }
    public class ProductRatingRepository : BaseRepository<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(eShopDbContext context) : base(context) { }
    }
}
