using eShopFinalProject.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eShopFinalProject.Utilities.ViewModel.Users;
using eShopFinalProject.Services.Users;
using eShopFinalProject.Utilities.ViewModel.Brands;
using Microsoft.AspNetCore.Authorization;
using eShopFinalProject.Utilities.ViewModel.Page;

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
            var result = await _userService.CreateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAdminAsync(request);
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
    }
}
