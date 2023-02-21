using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Coupons
{
    public class CreateCouponRequest
    {
        public string Name { get; set; }
        public DateTime Expiry { get; set; }
        public int Discount { get; set; }
    }
}
