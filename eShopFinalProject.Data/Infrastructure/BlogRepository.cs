using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class BlogRepository : BaseRepository<Blog>
    {
        public BlogRepository(eShopDbContext context) : base(context) { }
    }
}
