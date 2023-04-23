using eShopFinalProject.Utilities.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Products
{
    public class ProductRatingVM
    {
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public ProductVM Product { get; set; }
        public UserVM User { get; set; }
    }
}
