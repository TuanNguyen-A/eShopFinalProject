using eShopFinalProject.Services.Colors;
using eShopFinalProject.Services.Products;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _productService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productService.GetAsync(id);

            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add([FromBody] CreateProductRequest req)
        {
            var result = await _productService.CreateAsync(req);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var result = await _productService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductRequest request)
        {
            var result = await _productService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("rating")]
        [Authorize]
        public async Task<IActionResult> RatingProduct([FromBody] RatingProductRequest req, [FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);

            var result = await _productService.RatingProduct(req, (string)email);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("add-wishlist")]
        [Authorize]
        public async Task<IActionResult> AddWishList([FromBody] AddWishListProductRequest req, [FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);

            var result = await _productService.AddWishList(req, (string)email);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("get-wishlist")]
        [Authorize]
        public async Task<IActionResult> GetWishList([FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);

            var result = await _productService.GetWishList((string)email);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }
    }
}
