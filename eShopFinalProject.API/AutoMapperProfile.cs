using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Coupons;
using eShopFinalProject.Utilities.ViewModel.Products;

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

            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Colors, 
                source => source.MapFrom(source=> source.ProductInColors.Select(pic => pic.Color).ToList()));

        }
    }
}
