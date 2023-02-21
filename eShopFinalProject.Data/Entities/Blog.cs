using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumView { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Category Category { get; set; }
    }
}
