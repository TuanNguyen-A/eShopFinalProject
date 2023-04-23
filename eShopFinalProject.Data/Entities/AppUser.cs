using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsBlock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual List<ProductRating> ProductRatings { get; set; }
        public virtual List<Blog> Blogs { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual List<ProductInWish> ProductInWishes { get; set; }
    }
}
