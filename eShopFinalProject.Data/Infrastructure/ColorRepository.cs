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
    public interface IColorRepository : IBaseRepository<Color>
    {
    }
    public class ColorRepository : BaseRepository<Color>, IColorRepository
    {
        public ColorRepository(eShopDbContext context) : base(context) { }
    }
}
