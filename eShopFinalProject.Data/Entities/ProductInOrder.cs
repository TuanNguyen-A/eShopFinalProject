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

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
