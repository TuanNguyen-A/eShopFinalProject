using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<Brand>> CreateAsync(CreateBrandRequest request)
        {
            try
            {
                var existedEntity = await _unitOfWork.BrandRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Brand>(400, Resource.Brand_Existed);
                }

                Brand entity = _mapper.Map<Brand>(request);
                var result = await _unitOfWork.BrandRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Brand>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Brand));
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Brand));
            }
        }

        public async Task<ResultWrapperDto<Brand>> DeleteAsync(DeleteBrandRequest request)
        {
            try
            {
                var entity = await _unitOfWork.BrandRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Brand>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }
                _unitOfWork.BrandRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Brand>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Brand));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Brand));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<BrandVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try {
                var listItem = await _unitOfWork.BrandRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x=>x.Title.Contains(req.Search.ToLower())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Brand>,List<BrandVM>>(listPagingItem);

                var result = new PagingResult<BrandVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<BrandVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Brand));
            }
        }

        public async Task<ResultWrapperDto<BrandVM>> GetAsync(int id)
        {
            try {
                var entity = await _unitOfWork.BrandRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<BrandVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }
                var result = _mapper.Map<BrandVM>(entity);
                return new ResultWrapperDto<BrandVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Brand));
            }
        }

        public async Task<ResultWrapperDto<Brand>> UpdateAsync(UpdateBrandRequest request)
        {
            try
            {
                var foundEntity = await _unitOfWork.BrandRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Brand>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }

                var existedEntity = await _unitOfWork.BrandRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Brand>(400, Resource.Brand_Existed);
                }

                foundEntity.Title = request.Title;
                var result = _unitOfWork.BrandRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Brand>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Brand));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Brand));
            }
            throw new NotImplementedException();
        }
    }
}
