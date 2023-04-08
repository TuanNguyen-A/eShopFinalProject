using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Images;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Images
{
    public interface IImageService
    {
        Task<ResultWrapperDto<List<ImageVM>>> UploadImageList(IFormFileCollection images);
        Task<ImageVM> UploadImage(IFormFile image);
        Task<ResultWrapperDto<bool>> DeleteImage(DeleteImageRequest request);
    }
}
