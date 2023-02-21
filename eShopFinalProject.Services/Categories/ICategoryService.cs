using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Categories
{
    public interface ICategoryService
    {
        Task<ResultWrapperDto<PagingResult<CategoryVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<CategoryVM>> GetAsync(int id);
        Task<ResultWrapperDto<Category>> CreateAsync(CreateCategoryRequest request);
        Task<ResultWrapperDto<Category>> UpdateAsync(UpdateCategoryRequest request);
        Task<ResultWrapperDto<Category>> DeleteAsync(DeleteCategoryRequest request);
    }
}
