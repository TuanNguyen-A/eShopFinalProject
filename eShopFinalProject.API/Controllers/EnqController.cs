using eShopFinalProject.Services.Blogs;
using eShopFinalProject.Services.Enqs;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Utilities.ViewModel.Enqs;
using eShopFinalProject.Utilities.ViewModel.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/enq")]
    [ApiController]
    public class EnqController : ControllerBase
    {
        private readonly IEnqService _enqService;

        public EnqController(
            IEnqService enqService)
        {
            _enqService = enqService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll([FromQuery] PagingGetAllRequest req)
        {
            var result = await _enqService.GetAllAsync(req);
            return result.StatusCode != 200 ?
               StatusCode(result.StatusCode, result.Message) :
               Ok(result.Dto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _enqService.GetAsync(id);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateEnqRequest request)
        {
            var result = await _enqService.CreateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] UpdateEnqRequest request)
        {
            var result = await _enqService.UpdateAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("update-status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusEnqRequest request)
        {
            var result = await _enqService.UpdateStatusAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteEnqRequest request)
        {
            var result = await _enqService.DeleteAsync(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
