using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Products
{
    public class RatingProductRequest
    {
        public int ProductId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
    }
}
