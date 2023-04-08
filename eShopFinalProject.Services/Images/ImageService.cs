using AutoMapper;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using Resource = eShopFinalProject.Utilities.Resources.Resource;
using Microsoft.Extensions.Configuration;
using eShopFinalProject.Services.Images;
using eShopFinalProject.Utilities.ViewModel.Images;
using MailKit.Net.Imap;

namespace eShopFinalProject.Services.Uploads
{
    public class ImageService : IImageService
    {
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageRepository _imageRepository;

        public ImageService(
            IMapper mapper,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IImageRepository imageRepository
            )
        {
            _unitOfWork = unitOfWork;
            _imageRepository = imageRepository;
            _mapper = mapper;
            var cloudinarySection = configuration.GetSection("Cloudinary");
            string cloudName = cloudinarySection.GetSection("CloudName").Value;
            string apiKey = cloudinarySection.GetSection("ApiKey").Value;
            string apiSecret = cloudinarySection.GetSection("ApiSecret").Value;
            CloudinaryDotNet.Account cloudinaryAccount = new CloudinaryDotNet.Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(cloudinaryAccount);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ResultWrapperDto<List<ImageVM>>> UploadImageList(IFormFileCollection images)
        {
            try
            {
                MemoryStream ms;
                var result = new List<ImageVM>() { } ;
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
                    result.Add(new ImageVM()
                    {
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.Url.ToString()
                    });
                    await _imageRepository.AddAsync(new Data.Entities.Image()
                    {
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.Url.ToString()
                    });

                }
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<List<ImageVM>>(result);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.UploadImage_Failed));
            }
        }

        public async Task<ImageVM> UploadImage(IFormFile image)
        {
            try
            {
                MemoryStream ms;

                ms = new MemoryStream();
                await image.CopyToAsync(ms);
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

                var result = await _imageRepository.AddAsync(new Data.Entities.Image()
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString()
                });

                await _unitOfWork.SaveChangesAsync();

                return new ImageVM()
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString()
                };
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.UploadImage_Failed));
            }
        }

        public async Task<ResultWrapperDto<bool>> DeleteImage(DeleteImageRequest request)
        {
            try
            {
                var entity = await _imageRepository.FindAsync(x=>x.PublicId == request.PublicId);
                var img = entity.FirstOrDefault();
                if (entity == null)
                {
                    return new ResultWrapperDto<bool>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                }

                var deletionParams = new DeletionParams(request.PublicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                _imageRepository.Delete(img);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<bool>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Image));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(Resource.DeleteImage_Failed));
            }
        }
    }
}
