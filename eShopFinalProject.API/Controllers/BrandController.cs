using eShopFinalProject.Services.Brands;
using eShopFinalProject.Services.Categories;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Page;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(
            IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _brandService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _brandService.GetAsync(id);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateBrandRequest request)
        {
            var result = await _brandService.CreateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateBrandRequest request)
        {
            var result = await _brandService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteBrandRequest request)
        {
            var result = await _brandService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
