using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Coupons
{
    public interface ICouponService
    {
        Task<ResultWrapperDto<PagingResult<CouponVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<CouponVM>> GetAsync(int id);
        Task<ResultWrapperDto<Coupon>> CreateAsync(CreateCouponRequest request);
        Task<ResultWrapperDto<Coupon>> UpdateAsync(UpdateCouponRequest request);
        Task<ResultWrapperDto<Coupon>> DeleteAsync(DeleteCouponRequest request);
    }
}
