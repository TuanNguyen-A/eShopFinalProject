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
    public interface IProductInColorRepository : IBaseRepository<ProductInColor>
    {
    }
    public class ProductInColorRepository : BaseRepository<ProductInColor>, IProductInColorRepository
    {
        public ProductInColorRepository(eShopDbContext context) : base(context) { }

    }
}
