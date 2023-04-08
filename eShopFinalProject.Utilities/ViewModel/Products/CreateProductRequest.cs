using eShopFinalProject.Utilities.ViewModel.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Products
{
    public class CreateProductRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<int> Colors { get; set; }
        public List<string> Images { get; set; }
        public string Tag { get; set; }
    }
}
