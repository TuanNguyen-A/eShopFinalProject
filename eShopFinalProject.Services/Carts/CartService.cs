using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Carts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Utilities.ViewModel.Products;

namespace eShopFinalProject.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductInCartRepository _productInCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;


        public CartService(
            IUnitOfWork unitOfWork,
            ICartRepository cartRepository,
            UserManager<AppUser> userManager,
            IProductInCartRepository productInCartRepository,
            IProductRepository productRepository,
            IMapper mapper
            )
        {
            _productRepository = productRepository;
            _productInCartRepository = productInCartRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<ResultWrapperDto<Cart>> CreateOrUpdate(CreateCartRequest request, string email)
        {
            try
            {
                if (!request.Products.Any())
                {
                    return new ResultWrapperDto<Cart>(400, "Bad Request");
                }

                var user = await _userManager.FindByEmailAsync(email);

                List<ProductInCart> productInCarts = new List<ProductInCart>() { };

                foreach (CreateProductInCart pic in request.Products)
                {
                    var product = await _productRepository.GetAsync(pic.ProductId);
                    if (product == null)
                    {
                        return new ResultWrapperDto<Cart>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                    }

                    productInCarts.Add(new ProductInCart() { 
                        ProductId = pic.ProductId,
                        Quantity = pic.Quantity
                    });
                };

                //Check if cart exists 
                var carts = await _cartRepository.FindAsync(x => x.UserId == user.Id);
                Cart cart = carts.FirstOrDefault();
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        User = user,
                        ProductInCarts = productInCarts
                    };
                    await _cartRepository.AddAsync(cart);
                }
                else
                {
                    cart.ProductInCarts = productInCarts;
                    _cartRepository.Update(cart);
                }

                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Cart>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Cart));

            }
            catch(Exception ex)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Cart));
            }
        }
    }
}
