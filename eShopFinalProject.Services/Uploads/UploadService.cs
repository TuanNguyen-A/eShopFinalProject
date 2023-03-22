using AutoMapper;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Enqs;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using eShopFinalProject.Utilities.Resources;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using static System.Net.Mime.MediaTypeNames;
using Resource = eShopFinalProject.Utilities.Resources.Resource;
using Microsoft.Extensions.Configuration;

namespace eShopFinalProject.Services.Uploads
{
    public class UploadService : IUploadService
    {
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        
        public UploadService(
            IMapper mapper,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            var cloudinarySection = configuration.GetSection("Cloudinary");
            string cloudName = cloudinarySection.GetSection("CloudName").Value;
            string apiKey = cloudinarySection.GetSection("ApiKey").Value;
            string apiSecret = cloudinarySection.GetSection("ApiSecret").Value;
            CloudinaryDotNet.Account cloudinaryAccount = new CloudinaryDotNet.Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(cloudinaryAccount);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ResultWrapperDto<List<UploadImageReponse>>> UploadProductImage(IFormFileCollection images)
        {
            try
            {
                MemoryStream ms;
                var result = new List<UploadImageReponse>() { } ;
                foreach (var img in images)
                {
                    ms = new MemoryStream();
                    await img.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    string fileName = Guid.NewGuid().ToString();

                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, ms),
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true
                    };
                    ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    uploadResult.Url.ToString();
                    result.Add(new UploadImageReponse()
                    {
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.Url.ToString()
                    });
                }
                return new ResultWrapperDto<List<UploadImageReponse>>(result);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.UploadImage_Failed));
            }
        }
    }
}
