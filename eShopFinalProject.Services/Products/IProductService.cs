using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Products
{
    public interface IProductService
    {
        Task<ResultWrapperDto<PagingResult<ProductVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<ProductVM>> GetAsync(int id);
        Task<ResultWrapperDto<Product>> CreateAsync(CreateProductRequest request);
        Task<ResultWrapperDto<Product>> UpdateAsync(UpdateProductRequest request);
        Task<ResultWrapperDto<Product>> DeleteAsync(DeleteProductRequest request);
        Task<ResultWrapperDto<Product>> RatingProduct(RatingProductRequest req, string email);
        Task<ResultWrapperDto<Product>> AddWishList(AddWishListProductRequest req, string? email);
        Task<ResultWrapperDto<List<ProductVM>>> GetWishList( string? email);
        Task<ResultWrapperDto<Product>> RemoveProductFromWishList(DeleteProductRequest req, string email);
    }
}
