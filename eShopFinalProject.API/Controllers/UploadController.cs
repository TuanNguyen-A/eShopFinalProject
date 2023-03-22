using eShopFinalProject.Services.Blogs;
using eShopFinalProject.Services.Uploads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(
            IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("product-images")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadProductImage()
        {
            var files = HttpContext.Request.Form.Files;
            var result = await _uploadService.UploadProductImage(files);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }
    }
}
