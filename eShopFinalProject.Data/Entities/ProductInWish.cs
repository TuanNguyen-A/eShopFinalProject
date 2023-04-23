using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class ProductInWish
    {
        public int ProductId { get; set; }
        public Guid UserId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}
