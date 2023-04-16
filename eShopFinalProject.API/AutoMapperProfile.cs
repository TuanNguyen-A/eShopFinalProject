using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.ViewModel.Blogs;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Carts;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Enqs;
using eShopFinalProject.Utilities.ViewModel.Orders;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using eShopFinalProject.Utilities.ViewModel.Users;

namespace eShopFinalProject.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateColorRequest, Color>();
            CreateMap<UpdateColorRequest, Color>();
            CreateMap<Color, ColorVM>();

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryVM>();

            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<UpdateBrandRequest, Brand>();
            CreateMap<Brand, BrandVM>();

            CreateMap<CreateCouponRequest, Coupon>();
            CreateMap<UpdateCouponRequest, Coupon>();
            CreateMap<Coupon, CouponVM>();

            CreateMap<CreateBlogRequest, Blog>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Blog, BlogVM>();

            CreateMap<CreateEnqRequest, Enq>();
            CreateMap<Enq, EnqVM>();

            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Colors, 
                source => source.MapFrom(source=> source.ProductInColors.Select(pic => pic.Color).ToList()));

            CreateMap<CreateUserRequest, AppUser>();
            CreateMap<AppUser, UserVM>();

            CreateMap<Image, ImageVM>();

            CreateMap<Cart, CartVM>();

            CreateMap<ProductInCart, ProductInCartVM>();

            CreateMap<Order, OrderVM>();

            CreateMap<ProductInOrder, ProductInOrderVM>();
        }
    }
}
