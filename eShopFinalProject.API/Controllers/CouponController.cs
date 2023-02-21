using eShopFinalProject.Services.Categories;
using eShopFinalProject.Services.Coupons;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Page;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(
            ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _couponService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _couponService.GetAsync(id);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateCouponRequest request)
        {
            var result = await _couponService.CreateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCouponRequest request)
        {
            var result = await _couponService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCouponRequest request)
        {
            var result = await _couponService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
