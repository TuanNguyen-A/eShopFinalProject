using eShopFinalProject.Utilities.ViewModel.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Carts
{
    public class CartVM
    {
        public List<ProductInCartVM> ProductInCarts { get; set; }
    }

    public class ProductInCartVM
    {
        public ProductVM Product { get; set; }
        public int Quantity { get; set; }
    }
}
