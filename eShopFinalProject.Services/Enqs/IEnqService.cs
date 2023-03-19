using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Enqs;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Enqs
{
    public interface IEnqService
    {
        Task<ResultWrapperDto<PagingResult<EnqVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<EnqVM>> GetAsync(int id);
        Task<ResultWrapperDto<Enq>> CreateAsync(CreateEnqRequest request);
        Task<ResultWrapperDto<Enq>> UpdateAsync(UpdateEnqRequest request);
        Task<ResultWrapperDto<Enq>> UpdateStatusAsync(UpdateStatusEnqRequest request);
        Task<ResultWrapperDto<Enq>> DeleteAsync(DeleteEnqRequest request);
    }
}
