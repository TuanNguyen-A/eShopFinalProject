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
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Data.Enums;
using Microsoft.AspNetCore.Components;

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
                _cartRepository.Delete(cart);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Order>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Order));
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Order));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<OrderVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _orderRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.User.NormalizedEmail.Contains(req.Search)).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Order>, List<OrderVM>>(listPagingItem);

                var result = new PagingResult<OrderVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<OrderVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Brand));
            }
        }

        public async Task<ResultWrapperDto<OrderVM>> GetAsync(int id)
        {
            try
            {
                var entity = await _orderRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<OrderVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Order));
                }
                var result = _mapper.Map<OrderVM>(entity);
                return new ResultWrapperDto<OrderVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Blog));
            }
        }

        public async Task<ResultWrapperDto<Order>> UpdateStatus(UpdateOrderRequest request)
        {
            try
            {
                var order = await _orderRepository.GetAsync(request.OrderID);
                if (order == null)
                {
                    return new ResultWrapperDto<Order>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Order));
                }

                
                switch (request.OrderStatus)
                {
                    case "NotProcessed":
                        order.OrderStatus = OrderStatus.NotProcessed;
                        break;

                    case "Processing":
                        order.OrderStatus = OrderStatus.Processing;
                        break;

                    case "Dispatched":
                        order.OrderStatus = OrderStatus.Dispatched;
                        break;

                    case "Cancelled":
                        order.OrderStatus = OrderStatus.Cancelled;
                        break;

                    case "CashOnDelivery":
                        order.OrderStatus = OrderStatus.CashOnDelivery;
                        break;

                    case "Delivered":
                        order.OrderStatus = OrderStatus.Delivered;
                        break;
                }

                var result = _orderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Order>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Order));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Order));
            }
        }
    }
}
