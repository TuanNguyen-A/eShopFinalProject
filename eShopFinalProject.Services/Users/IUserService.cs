using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Users
{
    public interface IUserService
    {
        Task<ResultWrapperDto<PagingResult<UserVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<UserVM>> GetAsync(string email);
        Task<ResultWrapperDto<AppUser>> CreateAsync(CreateUserRequest request);
        Task<ResultWrapperDto<AppUser>> CreateAdminAsync(CreateUserRequest request);
        Task<ResultWrapperDto<AuthenticationResponse>> Authenticate(LoginRequest request);
        Task<ResultWrapperDto<AppUser>> UpdateAsync(UpdateUserRequest request);
        Task<ResultWrapperDto<AppUser>> DeleteAsync(DeleteUserRequest request);
    }
}
