using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IProductInColorRepository _productInColorRepository;
        private readonly IProductInWishRepository _productInWishRepository;
        private readonly UserManager<AppUser> _userManager;

        public ProductService(
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IColorRepository colorRepository,
            IProductInColorRepository productInColorRepository,
            IProductInWishRepository productInWishRepository,
            IImageRepository imageRepository,
            UserManager<AppUser> userManager,
            IMapper mapper
            )
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _productInColorRepository = productInColorRepository;
            _productInWishRepository = productInWishRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<PagingResult<ProductVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _productRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Title.Contains(req.Search.ToLower())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Product>,List<ProductVM>>(listPagingItem);

                var result = new PagingResult<ProductVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };

                return new ResultWrapperDto<PagingResult<ProductVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<ProductVM>> GetAsync(int id)
        
        {
            try
            {
                var entity = await _productRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<ProductVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                }
                var result = _mapper.Map<ProductVM>(entity);
                return new ResultWrapperDto<ProductVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.GetFailed_Template, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<Product>> CreateAsync(CreateProductRequest request)
        {
            try
            {
                var existedEntity = await _productRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Product>(400, Resource.Product_Existed);
                }

                //Validate Color
                List<Color> colorList = new List<Color>() { };
                foreach( int id in request.Colors)
                {
                    var color = await _colorRepository.GetAsync(id);
                    if (color == null)
                    {
                        return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                    }

                    colorList.Add(color);
                }

                //Validate Image
                List<Image> imageList = new List<Image>() { };
                foreach (string publicId in request.Images)
                {
                    var image = await _imageRepository.FindAsync(x => x.PublicId == publicId);
                    if (!image.Any())
                    {
                        return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Image));
                    }

                    imageList.Add(image.FirstOrDefault());
                }

                var brand = await _brandRepository.GetAsync(request.BrandId);
                if (brand == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }

                var category = await _categoryRepository.GetAsync(request.CategoryId);
                if (category == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                Product entity = _mapper.Map<Product>(request);
                entity.Slug = request.Title.Slugify();

                await _productRepository.AddAsync(entity);

                //Add Color
                foreach(Color c in colorList)
                {
                    ProductInColor pic = new ProductInColor()
                    {
                        Product = entity,
                        Color = c
                    };

                    await _productInColorRepository.AddAsync(pic);
                }

                //Add Image
                foreach (Image image in imageList)
                {
                    image.Product = entity;
                }

                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<Product>> UpdateAsync(UpdateProductRequest request)
        {
            try
            {
                var existedEntity = await _productRepository.GetAsync(request.Id);
                if (existedEntity == null)
                {
                    return new ResultWrapperDto<Product>(400, Resource.Product_Existed);
                }

                var brand = await _brandRepository.GetAsync(request.BrandId);
                if (brand == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }

                var category = await _categoryRepository.GetAsync(request.CategoryId);
                if (category == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                existedEntity.Title = request.Title;
                existedEntity.Slug = request.Slug;
                existedEntity.Description = request.Description;
                existedEntity.Price = request.Price;
                existedEntity.Quantity = request.Quantity;
                existedEntity.Sold = request.Sold;
                existedEntity.BrandId = request.BrandId;
                existedEntity.CategoryId = request.CategoryId;

                var result = _productRepository.Update(existedEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Product));
            }
            throw new NotImplementedException();
        }

        public async Task<ResultWrapperDto<Product>> DeleteAsync(DeleteProductRequest request)
        {
            try
            {
                var entity = await _productRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                }

                var result = _productRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<Product>> RatingProduct(RatingProductRequest req, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                var product = await _productRepository.GetAsync(req.ProductId);
                if (product == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                }

                product.ProductRatings.Add(new ProductRating()
                {
                    User = user,
                    Product = product,
                    Star = req.Star,
                    Comment = req.Comment
                });

                _productRepository.Update(product);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Rating, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<Product>> AddWishList(AddWishListProductRequest req, string? email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                var product = await _productRepository.GetAsync(req.ProductId);
                if (product == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                }

                var wishList = await _productInWishRepository.GetWishListByUserId(user.Id);

                if (wishList.Contains(product))
                {
                    return new ResultWrapperDto<Product>(400, String.Format(Resource.Product_Already_In_Wishlist));
                }

                await _productInWishRepository.AddAsync(new ProductInWish()
                {
                    Product = product,
                    User = user
                });

                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(200, String.Format(Resource.ActionSuccess_Template, Resource.Action_Add_WishList, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Add_WishList, Resource.Resource_Product));
            }
        }

        public async Task<ResultWrapperDto<List<ProductVM>>> GetWishList(string? email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResultWrapperDto<List<ProductVM>>(404, String.Format(Resource.NotFound_Template, Resource.Resource_User));
                }

                var wishList = await _productInWishRepository.GetWishListByUserId(user.Id);

                var result = _mapper.Map<List<Product>, List<ProductVM>>(wishList);
                return new ResultWrapperDto<List<ProductVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get_WishList, Resource.Resource_Product));
            }
        }
    }
}
