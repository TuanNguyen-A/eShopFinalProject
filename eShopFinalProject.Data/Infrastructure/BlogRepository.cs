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
    public interface IBlogRepository : IBaseRepository<Blog>
    {
    }
    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        public BlogRepository(eShopDbContext context) : base(context) { }

        public override async Task<List<Blog>> AllAsync()
        {
            return await _context.Set<Blog>()
                .Include(i => i.Category)
                .Include(i => i.User)
                .ToListAsync();
        }

        public override async Task<Blog> GetAsync(int id)
        {
            return await _context.Set<Blog>()
                .Include(i => i.Category)
                .Include(i => i.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
