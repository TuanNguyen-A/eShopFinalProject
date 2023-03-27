using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Services.Jwts;
using eShopFinalProject.Services.Mails;
using eShopFinalProject.Services.Uploads;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Mails;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Web;

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
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMailService _mailService;
        private readonly IUploadService _uploadService;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IAppUserRepository appUserRepository,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IMailService mailService,
            IUploadService uploadService,
            IMapper mapper,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appUserRepository = appUserRepository;
            _mailService = mailService;
            _uploadService = uploadService;
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<ResultWrapperDto<AppUser>> CreateAsync(CreateUserRequest request, string role)
        {
            try
            {
                var existedEntity = await _userManager.FindByEmailAsync(request.Email);
                if (existedEntity != null)
                {
                    return new ResultWrapperDto<AppUser>(400, Resource.UserEmail_Existed);
                }

                AppUser user = _mapper.Map<AppUser>(request);

                user.IsBlock = false;
                user.UserName = user.Email;

                if(request.AvatarImage != null)
                {
                    var uploadImageResponse = await _uploadService.UploadImage(request.AvatarImage);
                    user.Avatar = uploadImageResponse.Url;
                }

                await _userManager.CreateAsync(user, request.Password);

                await _userManager.AddToRoleAsync(user, role);

                await SendConfirmationEmail(user);
                return new ResultWrapperDto<AppUser>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_User));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_User));
            }
        }

        public async Task<ResultWrapperDto<AppUser>> ActivateUser(ActiveRequest req)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(req.Email);
                if (user == null)
                {
                    return new ResultWrapperDto<AppUser>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }
                var result = await _userManager.ConfirmEmailAsync(user, req.Token);
                return new ResultWrapperDto<AppUser>(200, String.Format(Resource.Activate_Success));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Activate, Resource.Resource_User));
            }
        }

        public async Task SendConfirmationEmail(AppUser user)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"{_config.GetSection("FrontendURL").Value}/activation/{token}";
                var message = new MailRequest()
                {
                    ToEmail= user.Email,
                    Subject="Confirm Email",
                    Body = $"This is activation link for your account: {confirmationLink}"
                };
                await _mailService.SendEmailAsync(message);
            }
            catch (Exception)
            {
                throw;
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

        public async Task<ResultWrapperDto<AppUser>> DeleteAsync(IdUserRequest request)
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
                var listItem = _appUserRepository.AllUserVMAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Email.Contains(req.Search) || x.FullName.Contains(req.Search)).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var result = new PagingResult<UserVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listPagingItem
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
                var roles = await _userManager.GetRolesAsync(entity);

                var result = _mapper.Map<UserVM>(entity);
                result.Role = roles[0];
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

                if (foundEntity.Email != request.Email)
                {
                    return new ResultWrapperDto<AppUser>(400, String.Format(Resource.User_Cannot_Change_Email));
                }

                if(request.FullName != null)
                {
                    foundEntity.FullName = request.FullName;
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

        public async Task<ResultWrapperDto<AppUser>> BlockOrUnblockUser(string id, bool isBlock)
        {
            try
            {
                var foundEntity = await _userManager.FindByIdAsync(id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<AppUser>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                foundEntity.IsBlock = isBlock;

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
