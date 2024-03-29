﻿using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;

namespace eShopFinalProject.Services.Colors
{
    public class ColorService : IColorService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public ColorService(
            IUnitOfWork unitOfWork,
            IColorRepository colorRepository,
            IMapper mapper
            )
        {
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ResultWrapperDto<Color>> CreateAsync(CreateColorRequest request)
        {
            try {
                var existedEntity = await _colorRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any()) {
                    return new ResultWrapperDto<Color>(400 ,Resource.Color_Existed);
                }
                Color entity = _mapper.Map<Color>(request);
                var result = await _colorRepository.AddAsync(entity);

                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Color>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Color));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Color));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<ColorVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try {
                var listItem = await _colorRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Title.Contains(req.Search.ToLower())).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex-1)* req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Color>,List<ColorVM>>(listPagingItem);

                var result = new PagingResult<ColorVM> {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count/ req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<ColorVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Color));
            }
        }

        public async Task<ResultWrapperDto<ColorVM>> GetAsync(int id)
        {
            try
            {
                var entity = await _colorRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<ColorVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                }
                var result = _mapper.Map<ColorVM>(entity);
                return new ResultWrapperDto<ColorVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.GetFailed_Template, Resource.Resource_Color));
            }
        }

        public async Task<ResultWrapperDto<Color>> UpdateAsync(UpdateColorRequest request)
        {
            try
            {
                var foundEntity = await _colorRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Color>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                }

                var existedEntity = await _colorRepository.FindAsync(x => x.Title == request.Title);
                if (existedEntity.Any())
                {
                    return new ResultWrapperDto<Color>(400, Resource.Color_Existed);
                }

                foundEntity.Title = request.Title;

                var result = _colorRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Color>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Color));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Color));
            }
        }

        public async Task<ResultWrapperDto<Color>> DeleteAsync(DeleteColorRequest request)
        {
            try
            {
                var entity = await _colorRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Color>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Color));
                }

                var result = _colorRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Color>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Color));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Color));
            }
        }
    }
}
