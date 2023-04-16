using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Orders
{
    public class OrderVM
    {
        public int Id { get; set; } 
        public int Total { get; set; }
        public int TotalAfterDiscount { get; set; }
        public string OrderStatus { get; set; }
        public List<ProductInOrderVM> ProductInOrders { get; set; }
        public UserVM User { get; set; }
        public CouponVM Coupon { get; set; }
    }

    public class ProductInOrderVM
    {
        public ProductVM Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { set; get; }
    }
}
