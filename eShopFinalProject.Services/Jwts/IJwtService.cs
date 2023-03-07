using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Jwts
{
    public interface IJwtService
    {
        public Task<AuthenticationResponse> CreateTokenAsync(AppUser user);
    }
}
