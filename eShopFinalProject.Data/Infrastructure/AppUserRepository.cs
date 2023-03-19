using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public interface IAppUserRepository : IBaseRepository<AppUser>
    {
        List<UserVM> AllUserVMAsync();
    }
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        private readonly eShopDbContext _context;
        public AppUserRepository(eShopDbContext context) : base(context) {
            _context = context;
        }

        public List<UserVM> AllUserVMAsync()
        {
            var result = from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id
                         select new UserVM()
                         {
                             Id = u.Id,
                             Email = u.Email,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Address = u.Address,
                             PhoneNumber = u.PhoneNumber,
                             Role = r.Name,
                             IsBlock = u.IsBlock
                         };
            return result.ToList();
        }
    }
}
