using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Orders;
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
    }
}
