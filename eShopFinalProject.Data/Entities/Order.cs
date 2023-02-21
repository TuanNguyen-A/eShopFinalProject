using eShopFinalProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus orderStatus { get; set; }
        public Guid UserId { get; set; }
        
        //Relationship
        public virtual AppUser User { get; set; }
        public virtual List<ProductInOrder> ProductInOrders { get; set; }
    }
}
