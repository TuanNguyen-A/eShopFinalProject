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
    }
}
