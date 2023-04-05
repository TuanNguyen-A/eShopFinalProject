using eShopFinalProject.Services.Images;
using eShopFinalProject.Utilities.ViewModel.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopFinalProject.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(
            IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("product-images")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadProductImage()
        {
            var files = HttpContext.Request.Form.Files;
            var result = await _imageService.UploadProductImage(files);
            return result.StatusCode != 200 ?
                StatusCode(result.StatusCode, result.Message) :
                Ok(result.Dto);
        }

        [HttpPost("delete-image")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteImage(DeleteImageRequest request)
        {
            var result = await _imageService.DeleteImage(request);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
