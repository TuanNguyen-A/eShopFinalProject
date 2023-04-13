using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual List<ProductInCart> ProductInCarts { get; set; }

    }
}
