using eShopFinalProject.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eShopFinalProject.Utilities.ViewModel.Users;
using eShopFinalProject.Services.Users;
using eShopFinalProject.Utilities.ViewModel.Brands;
using Microsoft.AspNetCore.Authorization;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.Resources;
using System.IdentityModel.Tokens.Jwt;
using eShopFinalProject.Utilities.Common;
using System.Security.Claims;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
        )
        {
            _userService = userService;
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request, AppConstants.Role.User);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("create-shop")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSeller([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request, AppConstants.Role.Seller);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("activate")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateUser([FromBody] ActiveRequest request)
        {
            var result = await _userService.ActivateUser(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("confirm-reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordRequest req)
        {
            var result = await _userService.SendResetPasswordConfirmMail(req.Email);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request, AppConstants.Role.Admin);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authenticate(request);

            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _userService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        
        [HttpGet("{email}")]
        [Authorize]
        public async Task<IActionResult> Get(string email)
        {
            var result = await _userService.GetAsync(email);

            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpGet("load-user")]
        [Authorize]
        public async Task<IActionResult> LoadUser([FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);
            var result = await _userService.GetAsync((string)email);

            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        //FAILED API
        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> Signout([FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);
            var result = await _userService.Signout((string)email);

            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromBody] IdUserRequest request)
        {
            var result = await _userService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("block")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BlockUser([FromBody] IdUserRequest request)
        {
            var result = await _userService.BlockOrUnblockUser(request.Id, true);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("unblock")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnlockUser([FromBody] IdUserRequest request)
        {
            var result = await _userService.BlockOrUnblockUser(request.Id, false);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
