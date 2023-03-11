using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Services.Jwts;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eShopFinalProject.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<ResultWrapperDto<AppUser>> CreateAsync(CreateUserRequest request)
        {
            try
            {
                var existedEntity = await _userManager.FindByEmailAsync(request.Email);
                if (existedEntity != null)
                {
                    return new ResultWrapperDto<AppUser>(400, Resource.UserEmail_Existed);
                }

                AppUser entity = _mapper.Map<AppUser>(request);

                entity.IsBlock = false;
                entity.UserName = entity.Email;

                await _userManager.CreateAsync(entity, request.Password);

                await _userManager.AddToRoleAsync(entity, "user");
                
                return new ResultWrapperDto<AppUser>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_User));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<AppUser>> CreateAdminAsync(CreateUserRequest request)
        {
            try
            {
                var existedEntity = await _userManager.FindByEmailAsync(request.Email);
                if (existedEntity != null)
                {
                    return new ResultWrapperDto<AppUser>(400, Resource.UserEmail_Existed);
                }

                AppUser entity = _mapper.Map<AppUser>(request);

                entity.IsBlock = false;
                entity.UserName = entity.Email;

                await _userManager.CreateAsync(entity, request.Password);

                await _userManager.AddToRoleAsync(entity, "admin");

                return new ResultWrapperDto<AppUser>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_User));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<AuthenticationResponse>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new ResultWrapperDto<AuthenticationResponse>(401, Resource.Account_Not_Exist);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ResultWrapperDto<AuthenticationResponse>(401, Resource.Incorrect_Password);
            }

            var token = await _jwtService.CreateTokenAsync(user);

            return new ResultWrapperDto<AuthenticationResponse>(token);
        }

        public async Task<ResultWrapperDto<AppUser>> DeleteAsync(DeleteUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                if (user == null)
                {
                    return new ResultWrapperDto<AppUser>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                var result = await _userManager.DeleteAsync(user);

                return new ResultWrapperDto<AppUser>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_User));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<UserVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _userManager.Users.ToListAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Email.Contains(req.Search) || x.FirstName.Contains(req.Search) || x.LastName.Contains(req.Search)).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<AppUser>, List<UserVM>>(listPagingItem);

                var result = new PagingResult<UserVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<UserVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<UserVM>> GetAsync(string email)
        {
            try
            {
                var entity = await _userManager.FindByEmailAsync(email);
                if (entity == null)
                {
                    return new ResultWrapperDto<UserVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }
                var result = _mapper.Map<UserVM>(entity);
                return new ResultWrapperDto<UserVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.GetFailed_Template, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<AppUser>> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                var foundEntity = await _userManager.FindByIdAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<AppUser>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                if (foundEntity.Email == request.Email)
                {
                    return new ResultWrapperDto<AppUser>(400, String.Format(Resource.User_Cannot_Change_Email));
                }

                if(request.FirstName != null)
                {
                    foundEntity.FirstName = request.FirstName;
                }

                if (request.LastName != null)
                {
                    foundEntity.LastName = request.LastName;
                }

                if (request.Address != null)
                {
                    foundEntity.Address = request.Address;
                }

                if (request.PhoneNumber != null)
                {
                    foundEntity.PhoneNumber = request.PhoneNumber;
                }

                await _userManager.UpdateAsync(foundEntity);
                return new ResultWrapperDto<AppUser>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_User));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_User));
            }
        }
    }
}
