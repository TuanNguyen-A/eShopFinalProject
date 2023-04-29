using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Carts
{
    public interface ICartService
    {
        Task<ResultWrapperDto<Cart>> CreateOrUpdate(CreateCartRequest request, string email);
        Task<ResultWrapperDto<Cart>> AddProductToCart(AddCartRequest request, string email);
        Task<ResultWrapperDto<CartVM>> GetCart(string? email);
        Task<ResultWrapperDto<Cart>> RemoveProductFromCart(RemoveProductRequest request, string email);
        Task<ResultWrapperDto<Cart>> UpdateProductFromCart(AddCartRequest request, string email);
    }
}
