using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using eShopFinalProject.Utilities.ViewModel.Products;
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

        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<PagingResult<ProductVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _unitOfWork.ProductRepository.AllAsync();

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
                var entity = await _unitOfWork.ProductRepository.GetAsync(id);
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
                var existedEntity = await _unitOfWork.ProductRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Product>(400, Resource.Product_Existed);
                }

                var color = await _unitOfWork.ColorRepository.GetAsync(request.ColorId);
                if (color == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                }

                var brand = await _unitOfWork.BrandRepository.GetAsync(request.BrandId);
                if (brand == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }

                var category = await _unitOfWork.CategoryRepository.GetAsync(request.CategoryId);
                if (category == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Category));
                }

                Product entity = _mapper.Map<Product>(request);
                await _unitOfWork.ProductRepository.AddAsync(entity);


                ProductInColor pic = new ProductInColor()
                {
                    Product = entity,
                    Color = color
                };

                await _unitOfWork.ProductInColorRepository.AddAsync(pic);
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
                var existedEntity = await _unitOfWork.ProductRepository.GetAsync(request.Id);
                if (existedEntity == null)
                {
                    return new ResultWrapperDto<Product>(400, Resource.Product_Existed);
                }

                var brand = await _unitOfWork.BrandRepository.GetAsync(request.BrandId);
                if (brand == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Brand));
                }

                var category = await _unitOfWork.CategoryRepository.GetAsync(request.CategoryId);
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

                var result = _unitOfWork.ProductRepository.Update(existedEntity);
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
                var entity = await _unitOfWork.ProductRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Product>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Product));
                }

                var result = _unitOfWork.ProductRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Product>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Product));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Product));
            }
        }
    }
}
