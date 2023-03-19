using AutoMapper;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Enqs;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopFinalProject.Utilities.Resources;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Data.Enums;

namespace eShopFinalProject.Services.Enqs
{
    public class EnqService : IEnqService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnqRepository _endRepository;
        private readonly IMapper _mapper;

        public EnqService(
            IUnitOfWork unitOfWork,
            IEnqRepository enqRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _endRepository = enqRepository;
            _mapper = mapper;
        }

        public async Task<ResultWrapperDto<Enq>> CreateAsync(CreateEnqRequest request)
        {
            try
            {
                Enq entity = _mapper.Map<Enq>(request);
                var result = await _endRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Enq>(201, String.Format(Resource.Create_Succes_Template, Resource.Resource_Enq));
            }
            catch (Exception e)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Create, Resource.Resource_Enq));
            }
        }

        public async Task<ResultWrapperDto<Enq>> DeleteAsync(DeleteEnqRequest request)
        {
            try
            {
                var entity = await _endRepository.GetAsync(request.Id);
                if (entity == null)
                {
                    return new ResultWrapperDto<Enq>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Enq));
                }
                _endRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Enq>(200, String.Format(Resource.Delete_Succes_Template, Resource.Resource_Enq));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Delete, Resource.Resource_Enq));
            }
        }

        public async Task<ResultWrapperDto<PagingResult<EnqVM>>> GetAllAsync(PagingGetAllRequest req)
        {
            try
            {
                var listItem = await _endRepository.AllAsync();

                if (!string.IsNullOrEmpty(req.Search))
                {
                    listItem = listItem.Where(x => x.Email == req.Search || x.PhoneNumber == req.Search || x.Name == req.Search).ToList();
                }

                var listPagingItem = listItem
                    .Skip((req.PageIndex - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

                var listVMItem = _mapper.Map<List<Enq>, List<EnqVM>>(listPagingItem);

                var result = new PagingResult<EnqVM>
                {
                    pageIndex = req.PageIndex,
                    pageSize = req.PageSize,
                    totalPage = (int)Math.Ceiling((double)listItem.Count / req.PageSize),
                    totalItem = listItem.Count,
                    items = listVMItem
                };
                return new ResultWrapperDto<PagingResult<EnqVM>>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Enq));
            }
        }

        public async Task<ResultWrapperDto<EnqVM>> GetAsync(int id)
        {
            try
            {
                var entity = await _endRepository.GetAsync(id);
                if (entity == null)
                {
                    return new ResultWrapperDto<EnqVM>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Enq));
                }
                var result = _mapper.Map<EnqVM>(entity);
                return new ResultWrapperDto<EnqVM>(result);
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Get, Resource.Resource_Enq));
            }
        }

        public async Task<ResultWrapperDto<Enq>> UpdateAsync(UpdateEnqRequest request)
        {
            try
            {
                var foundEntity = await _endRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Enq>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Enq));
                }

                foundEntity.Comment = request.Comment;
                var result = _endRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Enq>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Enq));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Enq));
            }
        }

        public async Task<ResultWrapperDto<Enq>> UpdateStatusAsync(UpdateStatusEnqRequest request)
        {
            try
            {
                var foundEntity = await _endRepository.GetAsync(request.Id);
                if (foundEntity == null)
                {
                    return new ResultWrapperDto<Enq>(404, String.Format(Resource.NotFound_Template, Resource.Resource_Enq));
                }

                switch (foundEntity.Status)
                {
                    case EnqStatus.Submitted:
                        foundEntity.Status = EnqStatus.Contacted;
                        break;

                    case EnqStatus.Contacted:
                        foundEntity.Status = EnqStatus.InProgress;
                        break;

                    case EnqStatus.InProgress:
                        foundEntity.Status = EnqStatus.Resolved;
                        break;
                }

                var result = _endRepository.Update(foundEntity);
                await _unitOfWork.SaveChangesAsync();
                return new ResultWrapperDto<Enq>(200, String.Format(Resource.Update_Succes_Template, Resource.Resource_Enq));
            }
            catch (Exception)
            {
                throw new Exception(String.Format(Resource.ActionFail_Template, Resource.Action_Update, Resource.Resource_Enq));
            }
        }
    }
}
