using eShopFinalProject.Services.Brands;
using eShopFinalProject.Services.Carts;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(
            ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CreateCartRequest request, [FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);
            var result = await _cartService.CreateOrUpdate(request, (string)email);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);
            var result = await _cartService.GetCart((string)email);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }
    }
}
