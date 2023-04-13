using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Carts
{
    public class CreateCartRequest
    {
        public List<CreateProductInCart> Products { get; set; }
    }

    public class CreateProductInCart
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
