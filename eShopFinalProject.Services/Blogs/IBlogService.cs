using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Blogs
{
    public interface IBlogService
    {
        Task<ResultWrapperDto<PagingResult<BlogVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<BlogVM>> GetAsync(int id);
        Task<ResultWrapperDto<Blog>> CreateAsync(CreateBlogRequest request);
        Task<ResultWrapperDto<Blog>> UpdateAsync(UpdateBlogRequest request);
        Task<ResultWrapperDto<Blog>> DeleteAsync(DeleteBlogRequest request);
    }
}
