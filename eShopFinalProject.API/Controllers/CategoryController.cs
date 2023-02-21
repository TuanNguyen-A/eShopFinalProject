using eShopFinalProject.Services.Categories;
using eShopFinalProject.Services.Colors;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _categoryService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _categoryService.GetAsync(id);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryRequest request)
        {
            var result = await _categoryService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
