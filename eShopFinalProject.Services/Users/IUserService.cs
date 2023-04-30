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
        Task<ResultWrapperDto<AppUser>> CreateAsync(CreateUserRequest request, string role);
        Task<ResultWrapperDto<AuthenticationResponse>> Authenticate(LoginRequest request);
        Task<ResultWrapperDto<AppUser>> UpdateAsync(UpdateUserRequest request);
        Task<ResultWrapperDto<AppUser>> DeleteAsync(IdUserRequest request);
        Task<ResultWrapperDto<AppUser>> BlockOrUnblockUser(string id, bool isBlock);
        Task<ResultWrapperDto<AppUser>> ActivateUser(ActiveRequest request);
        Task<ResultWrapperDto<AppUser>> Signout(string email);
        Task<ResultWrapperDto<AppUser>> SendResetPasswordConfirmMail(string email);
        Task<ResultWrapperDto<AppUser>> ResetPassword(ResetPasswordRequest request);
    }
}
