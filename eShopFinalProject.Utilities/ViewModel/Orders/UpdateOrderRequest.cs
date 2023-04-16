using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Orders
{
    public class UpdateOrderRequest
    {
        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
    }
}
