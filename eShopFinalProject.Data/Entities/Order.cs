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
        public OrderStatus OrderStatus { get; set; }
        public Guid UserId { get; set; }
        public int CouponId { get; set; }
        public int Total { get; set; }
        public int TotalAfterDiscount { get; set; }

        //Relationship
        public virtual AppUser User { get; set; }
        public virtual Coupon Coupon { get; set; } 
        public virtual List<ProductInOrder> ProductInOrders { get; set; }
    }
}
