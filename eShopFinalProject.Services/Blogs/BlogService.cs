using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Brands;
using Microsoft.AspNetCore.Identity;
using eShopFinalProject.Utilities.ViewModel.Categories;

namespace eShopFinalProject.Services.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageRepository _imageRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public BlogService(
            IUnitOfWork unitOfWork,
            IBlogRepository blogRepository,
            ICategoryRepository categoryRepository,
            UserManager<AppUser> userManager,
            IImageRepository imageRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
            _imageRepository = imageRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ResultWrapperDto<Blog>> CreateAsync(CreateBlogRequest request, string email)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(request.CategoryId);
                if (category == null)
                {
                    return new ResultWrapperDto<Blog>(400, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                //Validate Image
                List<Image> imageList = new List<Image>() { };
                foreach (string publicId in request.Images)
                {
                    var image = await _imageRepository.FindAsync(x => x.PublicId == publicId);
                    if (!image.Any())
                    {
                        return new ResultWrapperDto<Blog>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Image));
                    }

                    imageList.Add(image.FirstOrDefault());
                }

                var user = await _userManager.FindByEmailAsync(email);

                Blog entity = _mapper.Map<Blog>(request);
                entity.User = user;
                var result = await _blogRepository.AddAsync(entity);

                //Add Image
                foreach (Image image in imageList)
                {
                    image.Blog = entity;
                }

                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Blog>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Blog));
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Blog));
            }
        }

        public async Task<ResultWrapperDto<Blog>> DeleteAsync(DeleteBlogRequest request)
        {
            try
            {
                var entity = await _blogRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Blog>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Blog));
                }
                _blogRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Blog>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Blog));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Blog));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<BlogVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _blogRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Title.Contains(req.Search.ToLower())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Blog>, List<BlogVM>>(listPagingItem);

                var result = new PagingResult<BlogVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<BlogVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Blog));
            }
        }

        public async Task<ResultWrapperDto<BlogVM>> GetAsync(int id)
        {
            try
            {
                var entity = await _blogRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<BlogVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Blog));
                }
                var result = _mapper.Map<BlogVM>(entity);
                return new ResultWrapperDto<BlogVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Blog));
            }
        }

        public async Task<ResultWrapperDto<Blog>> UpdateAsync(UpdateBlogRequest request)
        {
            try
            {
                var foundEntity = await _blogRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Blog>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Blog));
                }

                var category = await _categoryRepository.GetAsync(request.CategoryId);
                if (category == null)
                {
                    return new ResultWrapperDto<Blog>(400, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ResultWrapperDto<Blog>(400, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                foundEntity.Title = request.Title;
                foundEntity.Description = request.Description;
                var result = _blogRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Blog>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Blog));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Blog));
            }
        }
    }
}
