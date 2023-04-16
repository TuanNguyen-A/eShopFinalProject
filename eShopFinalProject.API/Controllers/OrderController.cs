using eShopFinalProject.Services.Brands;
using eShopFinalProject.Services.Orders;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Orders;
using eShopFinalProject.Utilities.ViewModel.Page;
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
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CreateOrderRequest request, [FromHeader(Name = "Authorization")] string authorization)
        {
            JwtSecurityToken jwtToken = ApplicationUtils.ReadJwtToken(authorization);
            object email;
            jwtToken.Payload.TryGetValue(ClaimTypes.Email, out email);
            var result = await _orderService.CreateAsync(request, (string)email);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _orderService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _orderService.GetAsync(id);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderRequest request)
        {
            var result = await _orderService.UpdateStatus(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
