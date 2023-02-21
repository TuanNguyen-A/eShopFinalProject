using eShopFinalProject.Services.Colors;
using eShopFinalProject.Services.Products;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Add([FromBody] CreateProductRequest req)
        {
            var result = await _productService.CreateAsync(req);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var result = await _productService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductRequest request)
        {
            var result = await _productService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

    }
}
