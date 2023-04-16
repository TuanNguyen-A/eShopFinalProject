using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Orders;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Orders
{
    public interface IOrderService
    {
        Task<ResultWrapperDto<Order>> CreateAsync(CreateOrderRequest request, string email);
        Task<ResultWrapperDto<PagingResult<OrderVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<OrderVM>> GetAsync(int id);
        Task<ResultWrapperDto<Order>> UpdateStatus(UpdateOrderRequest request);
    }
}
