using eShopFinalProject.Services.Colors;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(
            IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PagingGetAllRequest req)
        {
            var result = await _colorService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get( int id)
        {
            var result = await _colorService.GetAsync(id);
            
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateColorRequest req)
        {
            var result = await _colorService.CreateAsync(req);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateColorRequest request)
        {
            var result = await _colorService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteColorRequest request)
        {
            var result = await _colorService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
