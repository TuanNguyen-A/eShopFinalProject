using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Page
{
    public class PagingResult<T>
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalItem { get; set; }
        public List<T> items { get; set; }
    }
}
