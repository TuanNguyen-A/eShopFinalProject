using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using eShopFinalProject.Services.Coupons;

namespace eShopFinalProject.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly ICouponService _couponService;
        private readonly ICouponRepository _couponRepository;

        public OrderService(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ICouponService couponService,
            ICouponRepository couponRepository,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _couponService = couponService;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<Order>> CreateAsync(CreateOrderRequest request, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResultWrapperDto<Order>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                var carts = await _cartRepository.FindAsync(x => x.UserId == user.Id);
                var cart = carts.FirstOrDefault();

                //Check Counpon
                Coupon coupon = null;
                if(request.CouponId != null)
                {
                    coupon = await _couponRepository.GetAsync((int)request.CouponId);

                    if (coupon == null)
                    {
                        return new ResultWrapperDto<Order>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Coupon));
                    }

                    if (!_couponService.IsValidCoupon(coupon))
                    {
                        return new ResultWrapperDto<Order>(400, Resource.Invalid_Coupon);
                    }
                }

                //Copy Product List to Order
                var productInOrders = new List<ProductInOrder>();
                foreach(ProductInCart pic in cart.ProductInCarts)
                {
                    productInOrders.Add(new ProductInOrder()
                    {
                        ProductId = pic.ProductId,
                        Price = pic.Product.Price,
                        Quantity = pic.Quantity
                    });
                }

                //Calculate Total
                var total = productInOrders.Sum(x => x.Price * x.Quantity);
                int totalAfterDiscount = coupon != null ? (int)total - coupon.Discount : (int)total;

                var order = new Order()
                {
                    UserId = user.Id,
                    ProductInOrders = productInOrders,
                    Total = (int)total,
                    TotalAfterDiscount = totalAfterDiscount,
                    Coupon = coupon
                };

                await _orderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Order>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Order));
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Order));
            }
        }
    }
}
