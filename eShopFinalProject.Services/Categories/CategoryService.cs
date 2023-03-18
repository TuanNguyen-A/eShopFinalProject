using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Categories
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<Category>> CreateAsync(CreateCategoryRequest request)
        {
            try {
                var existedEntity = await _categoryRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Category>(400, Resource.Category_Existed);
                }

                Category entity = _mapper.Map<Category>(request);
                var result = await _categoryRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Category>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Category));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Category));
            }
        }

        public async Task<ResultWrapperDto<Category>> DeleteAsync(DeleteCategoryRequest request)
        {
            try {
                var entity = await _categoryRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Category>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                var result = _categoryRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Category>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Category));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Category));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<CategoryVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _categoryRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Title.Contains(req.Search.ToLower())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Category>, List<CategoryVM>>(listPagingItem);


                var result = new PagingResult<CategoryVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<CategoryVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Category));
            }
        }

        public async Task<ResultWrapperDto<CategoryVM>> GetAsync(int id)
        {
            try {
                var foundEntity = await _categoryRepository.GetAsync(id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<CategoryVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                var result = _mapper.Map<CategoryVM>(foundEntity);
                return new ResultWrapperDto<CategoryVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Category));
            }
        }

        public async Task<ResultWrapperDto<Category>> UpdateAsync(UpdateCategoryRequest request)
        {
            try {
                var foundEntity = await _categoryRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Category>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                var existedEntity = await _categoryRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Category>(400, Resource.Category_Existed);
                }

                foundEntity.Title = request.Title;
                var result = _categoryRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Category>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Category));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Category));
            }
        }
    }
}
