using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Coupons
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<Coupon>> CreateAsync(CreateCouponRequest request)
        {
            try {
                var existedEntity = await _unitOfWork.CouponRepository.FindAsync(x => x.Name == request.Name);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Coupon>(400, Resource.Coupon_Existed);
                }
                Coupon entity = _mapper.Map<Coupon>(request);
                var result = await _unitOfWork.CouponRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Coupon>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Coupon));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Coupon));
            }
        }

        public async Task<ResultWrapperDto<Coupon>> DeleteAsync(DeleteCouponRequest request)
        {
            try {
                var entity = await _unitOfWork.CouponRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Coupon>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Coupon));
                }

                var result = _unitOfWork.CouponRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Coupon>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Coupon));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Coupon));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<CouponVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try {
                var listItem = await _unitOfWork.CouponRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Name.Contains(req.Search.ToUpper())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Coupon>, List<CouponVM>>(listPagingItem); 

                var result = new PagingResult<CouponVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<CouponVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Coupon));
            }
        }

        public async Task<ResultWrapperDto<CouponVM>> GetAsync(int id)
        {
            try {
                var entity = await _unitOfWork.CouponRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<CouponVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Coupon));
                }

                var result = _mapper.Map<CouponVM>(entity);

                return new ResultWrapperDto<CouponVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Coupon));
            }
        }

        public async Task<ResultWrapperDto<Coupon>> UpdateAsync(UpdateCouponRequest request)
        {
            try {
                var foundEntity = await _unitOfWork.CouponRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Coupon>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Coupon));
                }

                var existedEntity = await _unitOfWork.CouponRepository.FindAsync(x => x.Name == request.Name);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Coupon>(400, Resource.Coupon_Existed);
                }

                foundEntity.Name = request.Name;
                var result = _unitOfWork.CouponRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Coupon>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Coupon));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Coupon));
            }
        }
    }
}
