using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class ProductInOrder
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public int Quantity { set; get; }
        public decimal Price { set; get; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
