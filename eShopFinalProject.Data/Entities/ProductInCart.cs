using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class ProductInCart
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }

        public int Quantity { set; get; }

        public virtual Product Product { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
