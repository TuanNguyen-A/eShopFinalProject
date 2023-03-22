using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Uploads
{
    public interface IUploadService
    {
        Task<ResultWrapperDto<List<UploadImageReponse>>> UploadProductImage(IFormFileCollection images);
    }
}
