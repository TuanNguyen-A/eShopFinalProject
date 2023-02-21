using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Brands
{
    public interface IBrandService
    {
        Task<ResultWrapperDto<PagingResult<BrandVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<BrandVM>> GetAsync(int id);
        Task<ResultWrapperDto<Brand>> CreateAsync(CreateBrandRequest request);
        Task<ResultWrapperDto<Brand>> UpdateAsync(UpdateBrandRequest request);
        Task<ResultWrapperDto<Brand>> DeleteAsync(DeleteBrandRequest request);
    }
}
