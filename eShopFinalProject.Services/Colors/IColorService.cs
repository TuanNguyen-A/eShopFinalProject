using eShopFinalProject.Data.Entities;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Colors
{
    public interface IColorService
    {
        //Async
        Task<ResultWrapperDto<PagingResult<ColorVM>>> GetAllAsync(PagingGetAllRequest req);
        Task<ResultWrapperDto<ColorVM>> GetAsync(int id);
        Task<ResultWrapperDto<Color>> CreateAsync(CreateColorRequest request);
        Task<ResultWrapperDto<Color>> UpdateAsync(UpdateColorRequest request);
        Task<ResultWrapperDto<Color>> DeleteAsync(DeleteColorRequest request);
    }
}
