using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class ProductInColor
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
    }
}
